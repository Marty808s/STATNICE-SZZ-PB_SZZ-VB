import pandas as pd
import pymongo as pm
import os, urllib.parse

if __name__ == "__main__":
    data = pd.read_csv("./joined.csv")
    print(len(data["gdp_Country Name"].unique()))
    uri = "mongodb://root:root@localhost:27017/?authSource=admin"
    client = pm.MongoClient(uri, serverSelectionTimeoutMS=5000)
    db = client["mydb"]
    mycol = db["word_data"]#reference na kolekci

    records = data.to_dict(orient="records")
    print(records)

    x = mycol.insert_many(records)

    # print list of the _id values of the inserted documents:
    print(x.inserted_ids)



