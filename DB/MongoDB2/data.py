import pandas as pd
import numpy as np
import pymongo as pm

#dataset cizinci - načtu, zkontroluju nan hodnoty
cizinci = pd.read_csv("./cizinci.csv")
print(cizinci.info())

threshold = 0.3
to_drop = []

#iterace přes sloupce + identifikace sloupců, kde chybí více jak 30% dat
for c in cizinci.columns:
    missing = cizinci[c].isna().sum()
    total = cizinci.shape[0]
    print(f"Sloupec: {c}, {missing}/{total}")
    if missing > total * threshold:
        to_drop.append(c)

print("*" * 50)
print("Sloupce k vymazání:", to_drop)

#smazání těchto sloupců - jedná se o data o pohlaví
cizinci = cizinci.drop(columns=to_drop)

# jelikoz nemám k dispozici řadu, kterou bych byl schopen doplnit, tak pujdu cestou dropnutí řádků
cizinci = cizinci.dropna(axis=0, how="any")

for c in cizinci.columns:
    print(f"Sloupec: {c}, {cizinci[c].isna().sum()}/{cizinci.shape[0]}")

# u této datové sady došlo k redukci o cca 28tis řádků

# HDP data
print(40*"___")
print("HDP")
print(40*"___")
hdp = pd.read_csv("hdp.csv")

# pomocí okometrie -> chybějící údaje budou značeny "-"
print(hdp)
for c in hdp.columns:
    print(f"Sloupec: {c}, {hdp[c].isna().sum()}/{hdp.shape[0]}")

hdp = hdp.replace(to_replace="-", value=np.NaN)

print(40*"___")
print("Kontrola po replace - NaN")
print(40*"___")

for c in hdp.columns:
    print(f"Sloupec: {c}, {hdp[c].isna().sum()}/{hdp.shape[0]}")

# je obvyklé, že chybí data mezi sebou -> přechodně 2 roky - pro použiji nejspíše průměr hodnot
for idx, row in hdp.iterrows():
    if row.isna().any():
        print(f"idx: {idx} | NaN values:\n{row[row.isna()]}")

# počet řádků s aspoň jedním NaN
print(hdp.isna().any(axis=1).sum())
# z celkových 196 řádků bych musel odstranit 35

# musím převést object na int u roků
print(hdp.info())

# projdu sloupce (kromě prvního)
for c in hdp.columns[1:]:
    hdp[c] = hdp[c].fillna("0").astype(str).str.replace(" ","").astype(int)

#kontrola - už to funguje
print(hdp.info())

#ted vrátím 0 - na NaN
cols = hdp.columns[1:]
hdp[cols] = hdp[cols].replace(0, np.nan)
print(cols)

#print(hdp.head(5))

print(40*"___")
print("Před interpolací")
print(40*"___")

for c in hdp.columns:
    print(f"Sloupec: {c}, {hdp[c].isna().sum()}/{hdp.shape[0]}")

print(hdp.isna().any(axis=1).sum())

#interpolace
hdp[cols] = hdp[cols].interpolate(axis=1, limit=3) # po řádcích, max 3 chybějící

print(40*"___")
print("Po interpolaci")
print(40*"___")

for c in hdp.columns:
    print(f"Sloupec: {c}, {hdp[c].isna().sum()}/{hdp.shape[0]}")

print(hdp.isna().any(axis=1).sum()) #nakonec jsem akorát snížil počet k vymazání o 3

print(hdp.head(5))

#zbytek tedy dropuji - podle řádku
hdp = hdp.dropna(axis=0, how="any")
print(hdp)

for c in hdp.columns:
    print(f"Sloupec: {c}, {hdp[c].isna().sum()}/{hdp.shape[0]}")



