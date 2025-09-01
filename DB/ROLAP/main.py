import pandas as pd

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

prunik_clean_num = prunik_clean.select_dtypes(include="number")

prunik_clean_num = (prunik_clean_num
                    .interpolate(method="linear", axis=1)
                    .ffill(axis=0)
                    .bfill(axis=0))

# dosadit zpět do původního df
for col in prunik_clean_num.columns:
    prunik_clean.loc[:, col] = prunik_clean_num[col]

prunik_clean = prunik_clean.dropna(axis=1)

print(prunik_clean.isna().sum())  # kontrola – mělo by být vše 0
print(prunik_clean.head())

codes = prunik_clean.reset_index()[["Code", "area (km²)", "density (km²)"]]
print(codes.head())