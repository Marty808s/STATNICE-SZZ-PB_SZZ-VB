import pandas as pd
from pymongo import MongoClient

if __name__ == "__main__":
    gdp = pd.read_csv("./GDP.CSV")
    pop = pd.read_csv("./population.CSV")

    #GDP
    print("************GDP****************")
    #kontrola nan values
    print(gdp.isna())
    #v jakém sloupci je kolikl nan hodnot
    print(gdp.isnull().sum())

    #získám sloupce
    cols = gdp.columns.tolist()
    print(cols)

    #https://pandas.pydata.org/pandas-docs/stable/reference/api/pandas.DataFrame.fillna.html
    for c in cols:
        if gdp[c].isnull().any():
            gdp[c] = gdp[c].fillna(gdp[c].median()) #dopním mediánem

    #kontrola nan values
    print(gdp.isna())
    #v jakém sloupci je kolikl nan hodnot
    print(gdp.isnull().sum())
    print("*******************************")

    # - to je clean
    #kontrola nan values
    print("************POP****************")
    print(pop.isna())
    #v jakém sloupci je kolikl nan hodnot
    print(pop.isnull().sum())
    print("*******************************")

    print("*************EXPORT*************")

    gdp = gdp.rename(columns=lambda x: f"gdp_{x}")
    pop = pop.rename(columns=lambda x: f"pop_{x}")

    # join
    joined_df = gdp.set_index('gdp_Country Name').join(pop.set_index('pop_Country Name'))
    print(joined_df)

    gdp.to_csv('./gdp_clean.csv')
    pop.to_csv('./pop_clean.csv')
    joined_df.to_csv('./joined.csv')

