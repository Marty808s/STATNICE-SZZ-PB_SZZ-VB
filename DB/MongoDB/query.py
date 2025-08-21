import pandas as pd
import pymongo as pm
import os, urllib.parse

if __name__ == "__main__":
    uri = "mongodb://root:root@localhost:27017/?authSource=admin"
    client = pm.MongoClient(uri, serverSelectionTimeoutMS=5000)
    db = client["mydb"]
    mycol = db["word_data"]#reference na kolekci

    #top 10 zemí podle HDP 2021
    pipeline1 = [
        {
            "$project": {
                "country": "$gdp_Country Name",
                "code": "$gdp_Country Code",
                "gdp2021": {
                    "$cond": [
                        {"$in": [{"$type": "$gdp_2021"}, ["double", "int", "long", "decimal"]]},
                        "$gdp_2021",
                        {"$toDouble": {"$replaceAll": {"input": "$gdp_2021", "find": " ", "replacement": ""}}}
                    ]
                }
            }
        },
        {"$match": {"gdp2021": {"$ne": None, "$gt": 0}}},
        {"$sort": {"gdp2021": -1}},
        {"$limit": 10}
    ]

    #for doc in mycol.aggregate(pipeline1):
        #print(doc)


    # Top 10 zemí podle průměrného HDP + avg populace za 2012–2021.
    pipeline2 = [
        {
            "$project": {
                "country": "$gdp_Country Name",
                "code": "$gdp_Country Code",
                "gdp_avg": {
                    "$avg": [
                        {"$toDouble": "$gdp_2012"},
                        {"$toDouble": "$gdp_2013"},
                        {"$toDouble": "$gdp_2014"},
                        {"$toDouble": "$gdp_2015"},
                        {"$toDouble": "$gdp_2016"},
                        {"$toDouble": "$gdp_2017"},
                        {"$toDouble": "$gdp_2018"},
                        {"$toDouble": "$gdp_2019"},
                        {"$toDouble": "$gdp_2020"},
                        {"$toDouble": "$gdp_2021"},
                    ]
                },"pop_avg": {
                    "$avg": [
                        {"$toDouble": "$pop_2012"},
                        {"$toDouble": "$pop_2013"},
                        {"$toDouble": "$pop_2014"},
                        {"$toDouble": "$pop_2015"},
                        {"$toDouble": "$pop_2016"},
                        {"$toDouble": "$pop_2017"},
                        {"$toDouble": "$pop_2018"},
                        {"$toDouble": "$pop_2019"},
                        {"$toDouble": "$pop_2020"},
                        {"$toDouble": "$pop_2021"},
                    ]
                },
            },
        },
        {"$sort": {"gdp_avg": -1}},
        {"$limit": 10}
    ]
    #for doc in mycol.aggregate(pipeline2):
        #print(doc)

    # Spojení HDP a populace (2012–2021) → globální průměr
    pipeline3 = [
        {
            "$project": {
                "gdp_avg": {
                    "$avg": [
                        {"$toDouble": "$gdp_2012"},
                        {"$toDouble": "$gdp_2013"},
                        {"$toDouble": "$gdp_2014"},
                        {"$toDouble": "$gdp_2015"},
                        {"$toDouble": "$gdp_2016"},
                        {"$toDouble": "$gdp_2017"},
                        {"$toDouble": "$gdp_2018"},
                        {"$toDouble": "$gdp_2019"},
                        {"$toDouble": "$gdp_2020"},
                        {"$toDouble": "$gdp_2021"}
                    ]
                },
                "pop_avg": {
                    "$avg": [
                        {"$toDouble": "$pop_2012"},
                        {"$toDouble": "$pop_2013"},
                        {"$toDouble": "$pop_2014"},
                        {"$toDouble": "$pop_2015"},
                        {"$toDouble": "$pop_2016"},
                        {"$toDouble": "$pop_2017"},
                        {"$toDouble": "$pop_2018"},
                        {"$toDouble": "$pop_2019"},
                        {"$toDouble": "$pop_2020"},
                        {"$toDouble": "$pop_2021"}
                    ]
                }
            }
        },
        {
            "$group": {
                "_id": None,  # vše dohromady
                "world_gdp_avg": {"$avg": "$gdp_avg"},
                "world_pop_avg": {"$avg": "$pop_avg"}
            }
        }
    ]

    for doc in mycol.aggregate(pipeline3):
        print(doc)


