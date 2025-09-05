import pandas as pd
from db import getConnection

# LOAD DAT
hdp = pd.read_csv("hdp_coded.csv")
cizinci = pd.read_csv("cizinci.CSV")

# HDP DATASET
# Header fix hdp data - iterace pres radky
for idx, h in hdp.iterrows():
    #print(h.Stat)
    splited = h.Stat.split()
    if len(splited) > 1:
        h.Stat = splited[0]
    else:
        continue

# replace "-" -> NA
hdp.replace('-', pd.NA, inplace=True)

empty_tags = []
for c in hdp.columns:
    print(f"Sloupcec: {c}, počet: {hdp[c].isna().sum()}")
    print(f"Sloupcec: {c}, počet: {hdp[c].isnull().sum()}")


# fill na
hdp = hdp.ffill(axis=0)

for c in hdp.columns:
    print(f"Sloupcec: {c}, počet: {hdp[c].isna().sum()}")
    print(f"Sloupcec: {c}, počet: {hdp[c].isnull().sum()}")


for c in hdp.columns:
    print(f"Sloupcec: {c}, počet: {hdp[c].isna().sum()}")
    print(f"Sloupcec: {c}, počet: {hdp[c].isnull().sum()}")

print("Počet před:", hdp.shape[0])
hdp = hdp.dropna(axis=0, how="any")
print("Počet před:", hdp.shape[0])

#-------------------------------------------------------------------------------------------------------------------
# Cizinci DATASET
for c in cizinci.columns:
    print(f"Sloupcec: {c}, počet: {cizinci[c].isna().sum()}")
    #print(f"Sloupcec: {c}, počet: {cizinci[c].isnull().sum()}")

print(f"Před redukcí: {cizinci.columns.tolist()} | {len(cizinci.columns.tolist())}")

cizinci = cizinci.drop(columns=["pohlavi_cis", "pohlavi_kod", "pohlavi_txt", "vek_txt"])

print(f"Po redukci: {cizinci.columns.tolist()} | {len(cizinci.columns.tolist())}")

treshold = 0.1
to_drop = cizinci.isna().mean()
cols_over = to_drop[to_drop >= treshold].index.tolist()
print(cols_over) # sloupce kde chybi 10%

print(cizinci[cols_over])
cizinci = cizinci.drop(columns=["vek_cis", "vek_kod"])

for c in cizinci.columns:
    print(f"Sloupcec: {c}, počet: {cizinci[c].isna().sum()}")

#smažu chybějící řádky - mám hodně řádků, špatné smažu
print("Před", cizinci.shape[0])
cizinci = cizinci.dropna(axis=0, how="any")
print("Po", cizinci.shape[0])

for c in cizinci.columns:
    print(f"Sloupcec: {c}, počet: {cizinci[c].isna().sum()}")

years_hdp = hdp.columns
years_hdp = years_hdp[1:]
print(years_hdp)

years_dim = pd.DataFrame({
    "id": range(1,len(years_hdp)+1),
    "nazev": years_hdp
})
print(years_dim)

# kraje a uzemi dim
kraje_uzemi = cizinci[["kraj_kod","vuzemi_txt","kraj_txt"]].drop_duplicates() #dropne duplicitní hodnoty na víe sloupcích
print(kraje_uzemi)

region_dim = pd.DataFrame({
    "id": range(1,len(kraje_uzemi)+1),
    "source_key": kraje_uzemi["kraj_kod"],
    "nazev" : kraje_uzemi["kraj_txt"],
    "uzemi" : kraje_uzemi["vuzemi_txt"]
})

print(region_dim)

country = hdp["Stat"].unique()

country_dim = pd.DataFrame({
    "id": range(1,len(country)+1),
    "nazev": country
})

print(country_dim)

# Tabulky faktů - pomocí merge
"""
fact_foreigners = foreigners.merge(
    dim_country,
    left_on="country_name",   # business klíč ve faktech
    right_on="name_cz",       # business klíč v dimenzi
    how="left"
)
"""

"""
country_map = dict(zip(dim_country["name_cz"], dim_country["country_id"]))
foreigners["country_id"] = foreigners["country_name"].map(country_map)
"""
#fact_hdp
fact_country_hdp = hdp.merge(
    country_dim,
    left_on="Stat",
    right_on="nazev"
).rename(columns={"id": "fk_country"})

#převod z čirokéfo formátu na dlouhý
fact_country_hdp_long = fact_country_hdp.melt(
    id_vars=["fk_country", "Stat"],    # sloupce, které mají zůstat
    var_name="year",                   # nový název pro původní názvy sloupců (1990,1995,…)
    value_name="hdp_inhabitant"        # nový název pro hodnoty
)

fact_country_hdp_long = fact_country_hdp_long.merge(
    years_dim,
    left_on="year",
    right_on="nazev"
).rename(columns={"id":"fk_year"})

#print(fact_country_hdp_long.head(50))
fact_country_hdp = fact_country_hdp_long[["fk_country", "hdp_inhabitant", "fk_year"]]
#print(fact_country_hdp)

#fact_cizinci
fact_cizinci = cizinci.merge(
    country_dim,
    left_on="stobcan_txt",
    right_on="nazev"
).rename(columns={"id": "fk_country",
                  "hodnota" : "pocet_cizinci",
                  "idhod" : "source_key"
              })

fact_cizinci["rok"] = fact_cizinci["rok"].astype(int)
years_dim["nazev"] = years_dim["nazev"].astype(int)

fact_cizinci = fact_cizinci.merge(
    years_dim,
    left_on="rok",
    right_on="nazev"
).rename(columns={"id": "fk_year"})

fact_cizinci = fact_cizinci.merge(
    region_dim[["id","source_key"]],
    left_on="kraj_kod",
    right_on="source_key",
    how="left"
).rename(columns={"id": "fk_region"})

fact_cizinci = fact_cizinci.rename(columns={"source_key_x" : "source_key"})
#print(fact_cizinci)

fact_cizinci = fact_cizinci[["fk_country","fk_region","pocet_cizinci", "fk_year", "source_key"]]
#print(fact_cizinci)

#fact_cizinci = fact_cizinci[["fk_country", "fk_country"]]
#Export
#cizinci.to_csv('./output_cizinci.csv', index=False)
#hdp.to_csv('./output_hdp.csv', index=False)

#DATA INSERT
print(50*"_")
print("Insertování dat")

conn = getConnection()
print(conn)

#insert do dim_years
print(50*"_")
print("Years insert")
dim_year = years_dim[["nazev"]]

dim_year.to_sql(
    'dim_year',
    con=conn,
    if_exists='append',  # append = přidá data, fail = chyba, replace = dropne tabulku
    index=False
)

print(50*"_")
#insert do dim_region
print(50*"_")
print("Regiony")
dim_region = region_dim[["nazev","uzemi","source_key"]]

dim_region.to_sql(
    'dim_region',
    con=conn,
    if_exists="append",
    index=False
)

print(50*"_")

# insert dim_country
print(50*"_")
print("Země")
dim_country = country_dim[["nazev"]]

dim_country.to_sql(
    'dim_country',
    con=conn,
    if_exists="append",
    index=False
)

print(50*"_")

print(50*"_")
print("Cizinci")
#insert fact_cizinci

fact_cizinci.to_sql(
    'fact_cizinci',
    con=conn,
    if_exists="append",
    index=False
)

print(50*"_")

#insert fact_country_hdp
print(50*"_")
print("country hdp")

fact_country_hdp = fact_country_hdp.copy()   # doporučené, když víš, že chceš samostatný DF

fact_country_hdp.loc[:, "hdp_inhabitant"] = (
    fact_country_hdp["hdp_inhabitant"]
    .astype(str)
    .str.replace(" ", "", regex=False)
    .str.replace(",", ".", regex=False)
    .astype(int)
)

fact_country_hdp.to_sql(
    'fact_country_hdp',
    con=conn,
    if_exists="append",
    index=False
)
print(50*"_")
