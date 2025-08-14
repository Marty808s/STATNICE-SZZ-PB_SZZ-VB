from sqlalchemy import create_engine, text
from sqlalchemy.exc import SQLAlchemyError

# https://www.slingacademy.com/article/sqlalchemy-how-to-connect-to-mysql-database/
def get_connection():
    engine = create_engine("mysql+pymysql://mlapp:mlapp@127.0.0.1:3306/mlapp")
    try:
        with engine.connect() as conn:
            return conn
    except SQLAlchemyError as e:
        print(e)
