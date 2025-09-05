import pandas as pd
from sqlalchemy import create_engine
from sqlalchemy import Column, Integer, String, ForeignKey, Float, BigInteger
from sqlalchemy.orm import declarative_base, sessionmaker

# pøipojení na docker postgres
# https://docs.sqlalchemy.org/en/13/core/engines.html

def getConnection():
    engine = create_engine('postgresql://admin:admin@localhost:5332/postgres')
    return engine


