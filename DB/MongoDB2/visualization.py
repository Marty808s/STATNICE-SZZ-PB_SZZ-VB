import pymongo as pm
import pandas as pd
import matplotlib.pyplot as plt

uri = "mongodb://root:root@localhost:27017/?authSource=admin"
client = pm.MongoClient(uri, serverSelectionTimeoutMS=5000)
db = client["mydb"]

#https://www.mongodb.com/resources/products/capabilities/aggregation-pipeline

secti_cizinci = [
    {"$match": {"rok": 2020}}, #jiný rok v datech nemám
    {"$group": {"_id": "$zeme", "total": {"$sum": "$pocet"}}},
    {"$sort": {"total": -1}},
    {"$limit": 10} # vemu tom 10
]

df_secti_cizinci = pd.DataFrame(list(db.cizinci.aggregate(secti_cizinci)))
df_secti_cizinci.rename(columns={"_id": "Země", "total": "Počet"}, inplace=True)
print(df_secti_cizinci)


plt.bar(df_secti_cizinci["Země"], df_secti_cizinci["Počet"])
plt.xticks(rotation=90)
plt.ylabel("Počet")
plt.xlabel("Země")
plt.show()

#joined view -> kolekce
# https://www.mongodb.com/docs/manual/core/views/join-collections-with-view/
# pipeline s lookup pro vytvoření view -> to pak bude kolekce na kterou bud moct odkazovat
db.drop_collection("cizinci_hdp") #pokud je tak dropnu

pipeline_hdp_cizinci = [
    {"$lookup": {
        "from": "hdp",
        "localField": "zeme",      # pole v cizinci
        "foreignField": "zeme",    # pole v hdp (podle tvého hdpDoc)
        "as": "hdp"
    }},
    {"$unwind": {"path": "$hdp"}}
]

# vitvořím koleci pro view
db.create_collection(
    "cizinci_hdp",
    viewOn="cizinci",
    pipeline=pipeline_hdp_cizinci
)

# agregace nad view 
agregace_kraje = [
    {"$match": {
        "kraj": "Hlavní město Praha",
        "hdp.stats.2020": {"$ne": None} # not equal - jen ty co jsou opravdu 2020 a ne nan
    }},
    # omezit pole - výkon (to co použiji):
    {"$project": {"zeme": 1, "pocet": 1, "gdp2020": "$hdp.stats.2020"}}, # 1 = True | 0 = False
    {"$group": {
        "_id": "$zeme",
        "total_cizinci": {"$sum": "$pocet"},
        "gdp2020": {"$first": "$gdp2020"}
    }},
    {"$sort": {"total_cizinci": -1}},
    {"$limit": 10}
]

df_agregace_kraje = pd.DataFrame(list(db.cizinci_hdp.aggregate(agregace_kraje))).rename(columns={"_id": "zeme"})

print(df_agregace_kraje)







