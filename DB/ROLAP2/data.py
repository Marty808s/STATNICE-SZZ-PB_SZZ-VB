import pandas as pd

# LOAD DAT
hdp = pd.read_csv("hdp_coded.csv")
cizinci = pd.read_csv("cizinci.csv")

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

#Export
cizinci.to_csv('./output_cizinci.csv', index=False)
hdp.to_csv('./output_hdp.csv', index=False)
