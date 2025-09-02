import pandas as pd
from sqlalchemy import create_engine
from sqlalchemy import Column, Integer, String, ForeignKey, Float, BigInteger
from sqlalchemy.orm import declarative_base, sessionmaker

# připojení na docker postgres
# https://docs.sqlalchemy.org/en/13/core/engines.html

Base = declarative_base()

class Country(Base):
    __tablename__ = 'country'
    id = Column(Integer, primary_key=True, autoincrement=True)
    code = Column(String)
    name = Column(String)
    area = Column(Float)
    density = Column(Float)

class Year(Base):
    __tablename__ = 'year'
    id = Column(Integer, primary_key=True, autoincrement=True)
    value = Column(Integer)


class Fact_table(Base):
    __tablename__ = 'fact_table'
    id = Column(Integer, primary_key=True, autoincrement=True)
    fk_country = Column(Integer, ForeignKey('country.id'))
    gdp = Column(Float)
    pop = Column(BigInteger)
    fk_year = Column(Integer, ForeignKey('year.id'))

if __name__ == "__main__":
    engine = create_engine('postgresql://admin:admin@localhost:5332/postgres')
    Base.metadata.create_all(engine)

    #year
    session = sessionmaker()
    session.configure(bind=engine)
    s = session()

    #nahrání year dat - year
    df = pd.read_csv("./years.csv")
    df = df.dropna(subset=["value"])
    df["value"] = df["value"].astype("int64")

    try:
        print("***************YEAR***************")
        with s:
            for idx, row in df.iterrows():
                try:
                    print(idx, row["value"], type(row["value"]))
                    s.add(Year(value=int(row["value"])))
                    s.flush() #smaže předchozí comandy na db
                    print("Insertuji data")
                except Exception as e:
                    print(f"Výjimka na idx: {idx}: {repr(e)}")
                    raise
            s.commit()
    except Exception:
        print("Rollback!")

    #nahrání dat - country
    df = pd.read_csv("./country_data.csv")
    df["value"] = df["area"].astype("float64")
    df["value"] = df["density"].astype("float64")

    try:
        print("***************COUNTRY***************")
        with s:
            for idx, row in df.iterrows():
                try:
                    print(idx, row["code"], type(row["code"]))
                    s.add(Country(
                        code = row["code"],
                        name = row["name"],
                        area=row["area"],
                        density=row["density"]
                    ))
                    s.flush() #smaže předchozí comandy na db
                    print("Insertuji data")
                except Exception as e:
                    print(f"Výjimka na idx: {idx}: {repr(e)}")
                    raise
            s.commit()
    except Exception:
        print("Rollback!")

    #nahrání fact_table data
    df = pd.read_csv("./fact_table.csv")
    df["gdp"] = df["gdp"].astype("float64")
    df["pop"] = df["pop"].astype("int64")

    try:
        print("***************FACT_TABLE***************")
        with s:
            for idx, row in df.iterrows():
                try:
                    print(idx, row["fk_country"], type(row["fk_country"]))
                    s.add(Fact_table(
                        fk_country = row["fk_country"],
                        gdp= row["gdp"],
                        pop= row["pop"],
                        fk_year = row["fk_year"],

                    ))
                    s.flush() #smaže předchozí comandy na db
                    #print("Insertuji data")
                except Exception as e:
                    print(f"Výjimka na idx: {idx}: {repr(e)}")
                    raise
            s.commit()
    except Exception:
        print("Rollback!")
