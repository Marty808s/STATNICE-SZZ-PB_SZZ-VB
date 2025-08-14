from sqlalchemy import create_engine, text
from sqlalchemy.exc import SQLAlchemyError, IntegrityError
import json

# https://www.slingacademy.com/article/sqlalchemy-how-to-connect-to-mysql-database/

def get_engine():
    return create_engine(
        "mysql+pymysql://mlapp:mlapp@127.0.0.1:3306/mlapp",
        pool_pre_ping=True, future=True
    )

def insert_experiment(name, description):
    engine = get_engine()
    try:
        # BEGIN … COMMIT se spravuje automaticky
        with engine.begin() as conn:
            conn.execute(
                text("INSERT INTO experiment (name, description) VALUES (:name, :description)"),
                {"name": name, "description": description},
            )
        print("Experiment vložen:", name)
    except SQLAlchemyError as e:
        print("Chyba při INSERT:", e)

def get_experiments():
    engine = get_engine()
    with engine.begin() as conn:
        res = conn.execute(text(
            "SELECT id, name, description, created_at FROM experiment ORDER BY id"
        ))
        return [dict(r._mapping) for r in res.fetchall()]  # převod na dicty

def get_experiment_details(exp_id: int):
    engine = get_engine()
    with engine.begin() as conn:
        res = conn.execute(text("""
            SELECT 
                e.id            AS experiment_id,
                e.name          AS experiment_name,
                e.description   AS experiment_desc,
                e.created_at    AS experiment_created,

                m.id            AS model_id,
                m.name          AS model_name,
                m.model_type    AS model_type,
                m.description   AS model_desc,
                m.created_at    AS model_created,
                m.params_json   AS params,

                r.id            AS result_id,
                r.metric_name   AS result_metric_name,
                r.metric_value  AS result_metric_value,
                r.created_at    AS result_created

            FROM experiment e
            JOIN experiment_model em ON e.id = em.experiment_id
            JOIN model m             ON em.model_id = m.id
            LEFT JOIN result r       ON r.experiment_model_id = em.id

            WHERE e.id = :exp_id
            ORDER BY m.id, r.created_at DESC
        """), {"exp_id": exp_id})

        return [dict(row._mapping) for row in res.fetchall()]

def insert_model_for_experiment(
    experiment_id,
    model_type,
    name,
    description,
    params,
    artifact_uri=None,
    artifact_blob=None,
):
    engine = get_engine()  # tvoje helper funkce na získání engine
    try:
        with engine.begin() as conn:  # begin → automaticky commit/rollback
            # 1. Insert model
            sql_model = text("""
                INSERT INTO model (model_type, name, description, params_json, artifact_uri, artifact_blob)
                VALUES (:model_type, :name, :description, :params_json, :artifact_uri, :artifact_blob)
            """)
            res = conn.execute(
                sql_model,
                {
                    "model_type": model_type,
                    "name": name,
                    "description": description,
                    "params_json": json.dumps(params),  # převedeme dict na JSON string
                    "artifact_uri": artifact_uri,
                    "artifact_blob": artifact_blob,
                },
            )
            model_id = res.lastrowid  # získáme ID vloženého modelu

            # 2. Insert vazby experiment <-> model
            sql_exp_model = text("""
                INSERT INTO experiment_model (experiment_id, model_id)
                VALUES (:experiment_id, :model_id)
            """)
            conn.execute(
                sql_exp_model,
                {"experiment_id": experiment_id, "model_id": model_id},
            )

            print(f"Model '{name}' vložen pod experiment {experiment_id}")
            return {"model_id": model_id, "experiment_id": experiment_id}

    except SQLAlchemyError as e:
        print("Chyba při insertu modelu:", e)
        return None

def insert_result_for_experiment_model(experiment_id: int, model_id: int,
                                       metric_name: str, metric_value: float):
    """
    Najde experiment_model.id pro (experiment_id, model_id) a vloží záznam do `result`.
    """
    engine = get_engine()
    with engine.begin() as conn:
        # najdi vazbu experiment<->model
        em_row = conn.execute(
            text("""
                SELECT id FROM experiment_model
                WHERE experiment_id = :exp_id AND model_id = :model_id
                LIMIT 1
            """),
            {"exp_id": experiment_id, "model_id": model_id}
        ).first()

        if not em_row:
            raise ValueError(f"Model #{model_id} není přiřazen k experimentu #{experiment_id}.")

        em_id = em_row.id

        # vlož výsledek
        conn.execute(
            text("""
                INSERT INTO result (experiment_model_id, metric_name, metric_value)
                VALUES (:em_id, :metric_name, :metric_value)
            """),
            {"em_id": em_id, "metric_name": metric_name, "metric_value": metric_value}
        )