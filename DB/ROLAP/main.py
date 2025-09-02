import pandas as pd
import numpy as np

# gdp data
gdp = pd.read_csv('./gdp.csv')

#populační data
pop = pd.read_csv('./pop_data.csv')

print(pop.head(5))
print(gdp.head(5))

#očistění null hodnot gdp
for c in pop.columns:
    mask = pop[c].isnull()
    #print(pop[mask])

# - nemá null hodnoty

#očistění null hodnot gdp
for c in gdp.columns:
    mask = gdp[c].isnull()
    null_col = gdp[mask]
    #print(f"Sloupec: {c}, počet nullů: {len(null_col)} z počtu rows: {len(gdp[c])}")
# jelikžo dataset má do roků 1979 velký počet chybějících hodnot, tak jej zkrátím od roku 1980 - tím navážu na druhý dataset

for idx, r in gdp.iterrows():
    pocet_null = r.isna().sum()
    #print(f"Země: {r.Code}, {pocet_null} / {len(r)}")

prunik_codes = gdp.set_index("Code").join(pop.set_index("cca3"))
print(prunik_codes)


to_remove = []
for idx, r in prunik_codes.iterrows():
    pocet_null = r.isna().sum()
    celkem = len(r)
    print(f"Země: {idx}, {pocet_null} / {celkem}")
    
    if pocet_null > 0.3 * celkem:
        to_remove.append(idx)
        print(f"K odstranění: {idx}")

print("Seznam k odstranění:", to_remove, "\nCelkem:", len(to_remove))

mask = prunik_codes.isna().sum(axis=1) <= 0.3 * prunik_codes.shape[1] # vyčistím i podle řádku ty, kterým chybí max 30 procent
prunik_clean = prunik_codes[mask]
print(prunik_clean)

print(60*"*")
print(prunik_clean.columns)

# vytáhnu jenom numerické sloupce
prunik_clean_num = prunik_clean.select_dtypes(include="number")

prunik_clean_num = (prunik_clean_num
                    .interpolate(method="linear", axis=1)
                    .ffill(axis=0) # po řádkách
                    .bfill(axis=0)) # po řádkách

# dosadit zpět do původního df
for col in prunik_clean_num.columns:
    prunik_clean.loc[:, col] = prunik_clean_num[col]

prunik_clean = prunik_clean.dropna(axis=1)

print(prunik_clean.isna().sum())  # kontrola – mělo by být vše 0
print(prunik_clean.head())

country_data = prunik_clean.reset_index()[["Code", "Country Name", "area (km²)", "density (km²)"]]



# převedu na fk - country code
prunik_clean.index = [(enum+1) for enum, idx in enumerate(prunik_clean.index)]

# vybertu číselné sloupce - to co hodím do csv a nahraju do db
new_prunik = prunik_clean.select_dtypes(include="number")
new_prunik.insert(0, "fk_country", prunik_clean.index)

del new_prunik['area (km²)']
del new_prunik['density (km²)']
del new_prunik['Unnamed: 65']
del new_prunik['rank']

country_data.rename(columns={"Code": "code", "Country Name": "name", "area (km²)": "area", "density (km²)": "density"}, inplace=True)

print(country_data.head())

print(new_prunik)

#year dimenze
sloupce = new_prunik.columns
years = []

for y in sloupce[1:]:
    parts = str(y).split()
    if parts[0].isdigit():
        if int(parts[0]) not in years:
            years.append(int(parts[0]))
        print(parts[0], parts[1:])
years = set(years)
print(years)
years = pd.DataFrame(years, columns=["value"])

# MELT - na long fomrát
# 1) rozděl sloupce na GDP a POP
gdp_cols = [c for c in new_prunik.columns if str(c).isdigit()]
pop_cols = [c for c in new_prunik.columns if "population" in str(c)]

# 2) melt → long formát
gdp_long = new_prunik.melt(
    id_vars="fk_country",
    value_vars=gdp_cols,
    var_name="year",
    value_name="gdp"
)
gdp_long["year"] = gdp_long["year"].astype(int)

print(10*"*")
print(gdp_long)
print(10*"*")

pop_long = new_prunik.melt(
    id_vars="fk_country",
    value_vars=pop_cols,
    var_name="year_pop",
    value_name="pop"
)
pop_long["year"] = pop_long["year_pop"].str.split().str[0].astype(int)
pop_long = pop_long.drop(columns=["year_pop"])

print(10*"*")
print(pop_long)
print(10*"*")


# spoj GDP a POP podle fk_country + year
fact_table = pd.merge(gdp_long, pop_long, on=["fk_country", "year"], how="inner")

# přidej id (autoincrement)
#fact_table = fact_table.reset_index(drop=True)
#fact_table.insert(0, "id", fact_table.index + 1)

# 5) přejmenuj sloupce podle tvého schématu
fact_table.rename(columns={"year": "fk_year"}, inplace=True)

#fk year
years = years.reset_index(drop=True)
years.insert(0, "id", years.index + 1)

# mapa rok a id
year_map = dict(zip(years["value"], years["id"]))

# nahradím přes map
fact_table["fk_year"] = fact_table["fk_year"].map(year_map)

# zaokrouhluji pop na 0 desetinných míst
fact_table["pop"] = fact_table["pop"].round(0).astype("int64") # zaokoruhlení pop na int

# smažu id - autoinkrement
del years["id"]

print(fact_table.head(500))

#export
fact_table.to_csv('./fact_table.csv', index=False)
country_data.to_csv('./country_data.csv', index=False)
years.to_csv('./years.csv', index=False)

