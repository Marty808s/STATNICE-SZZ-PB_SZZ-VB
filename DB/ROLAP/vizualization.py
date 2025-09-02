import json
import matplotlib.pyplot as plt
import pandas as pd
from sqlalchemy import create_engine, select, func
from sqlalchemy import Column, Integer, String, ForeignKey, Float, BigInteger
from sqlalchemy.orm import declarative_base, sessionmaker
from dbInsertor import Fact_table, Year, Country


def gdpInYear(session, year: int):
    # součet GDP pro daný kalendářní rok (Year.value)
    stmt = (
        select(func.coalesce(func.sum(Fact_table.gdp), 0))
        .join(Year, Fact_table.fk_year == Year.id)
        .where(Year.value == year)
    )
    return session.execute(stmt).scalar_one()

def aggregateGDP(session):
    # agregace GDP napříč všemi roky všech zemí v datasetu
    stmt = (
        select(
            Year.value.label("year"),
            func.coalesce(func.sum(Fact_table.gdp), 0).label("gdp_sum"),
        )
        .join(Year, Fact_table.fk_year == Year.id)
        .group_by(Year.value)
        .order_by(Year.value)
    )
    result = session.execute(stmt).all()
    data = [{"year": year, "gdp_sum": float(gdp)} for year, gdp in result]
    return data

def aggregatePOP(session):
    # agregace POP napříč všemi roky všech zemí v datasetu
    stmt = (
        select(
            Year.value.label("year"),
            func.coalesce(func.sum(Fact_table.pop), 0).label("pop_sum"),
        )
        .join(Year, Fact_table.fk_year == Year.id)
        .group_by(Year.value)
        .order_by(Year.value)
    )
    result = session.execute(stmt).all()
    data = [{"year": year, "pop_sum": int(pop)} for year, pop in result]
    return data

if __name__ == '__main__':
    engine = create_engine('postgresql://admin:admin@localhost:5332/postgres')
    Session = sessionmaker(bind=engine)

    with Session() as s:
        gdp1970 = gdpInYear(s, 1970)
        print("GDP 1970:", gdp1970)
        aggGDP = aggregateGDP(s)
        aggPOP = aggregatePOP(s)


    # Plot GDP
    x = [d["year"] for d in aggGDP]
    y = [d["gdp_sum"] for d in aggGDP]
    plt.plot(x, y)
    plt.xlabel("Year")
    plt.ylabel("Sum GDP")
    plt.title("GDP sum by year")
    plt.tight_layout()
    plt.show()

    # Plot POP
    x = [d["year"] for d in aggPOP]
    y = [d["pop_sum"] for d in aggPOP]
    plt.plot(x, y)
    plt.xlabel("Year")
    plt.ylabel("Sum POP")
    plt.title("POP sum by year")
    plt.tight_layout()
    plt.show()