import pandas as pd
import pymongo as pm
# Načtu data a vytvořím dokumenty

# cizinci df
cizinci = pd.read_csv("_cizinci.csv")
print(cizinci)

# hdp df 
hdp = pd.read_csv("_hdp.csv")
print(hdp)
# jenom roky
_years = hdp.columns[1:]

# helper na none
def none_if_nan(x):
    return None if (x is None) else x


# definuji dokumenty - cizinci
def cizinciDoc(row):
    return {
            "source_id": none_if_nan(row.get("idhod")),
            "rok": none_if_nan(row.get("rok")),
            "zeme": none_if_nan(row.get("stobcan_txt")),
            "pocet": none_if_nan(row.get("hodnota")),
            "kraj": none_if_nan(row.get("kraj_txt")),
            "uzemi": none_if_nan(row.get("vuzemi_txt")),
            "vekova_skupina": none_if_nan(row.get("vek_txt")),

            "zeme_meta": {
                "kod": none_if_nan(row.get("stobcan_kod")),
                "nazev": none_if_nan(row.get("stobcan_txt")),
            },

            "kraj_meta": {
                "kod": none_if_nan(row.get("kraj_kod")),
                "nazev": none_if_nan(row.get("kraj_txt")),
                "cislo": none_if_nan(row.get("kraj_cis")),
            },

            "uzemi_meta": {
                "kod": none_if_nan(row.get("vuzemi_kod")),
                "nazev": none_if_nan(row.get("vuzemi_txt")),
                "cislo": none_if_nan(row.get("vuzemi_cis")),
            },

            "pohlavi": none_if_nan(row.get("pohlavi_txt"))
        }


# definuji dokumenty - cizinci
def hdpDoc(row, years=_years):
    print(_years)

    yearsDict = {}
    for y in _years:
        print(type(y))
        yearsDict[str(y)] = none_if_nan(row.get(y))

    return {
            #"source_id": none_if_nan(row.get("id")),
            "zeme": none_if_nan(row.get("Stat")),
            "stats" : yearsDict,
            "units": "osoby" 
        }

#vytvořím dokumenty z df - cizinci
print(50*"_")
cizinci_docs = cizinci.apply(cizinciDoc, axis=1).tolist()
print(len(cizinci_docs))

print(50*"_")
print("Cizinci - dokumenty:")
print(50*"_")
print(cizinci_docs[0])
print(50*"_")


print(50*"_")
# vytvořím dokumenty z df - hdp
hdp_docs = hdp.apply(hdpDoc, axis=1).tolist()

print(50*"_")
print("HDP - dokumenty:")
print(50*"_")
print(hdp_docs[0])
print(50*"_")


#DB část
uri = "mongodb://root:root@localhost:27017/?authSource=admin"
client = pm.MongoClient(uri, serverSelectionTimeoutMS=5000)
db = client["mydb"]
cizinci_col = db["cizinci"] #reference na kolekci
hdp_col = db["hdp"] #reference na kolekci

# Samotný insert - cizinci
if cizinci_docs:
  cizinci_col.insert_many(cizinci_docs, ordered=False)

# Samotný insert - cizinci
if hdp_docs:
    hdp_col.insert_many(hdp_docs, ordered=False)
