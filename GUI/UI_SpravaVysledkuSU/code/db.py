from sqlalchemy import create_engine, text
from sqlalchemy.exc import SQLAlchemyError

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