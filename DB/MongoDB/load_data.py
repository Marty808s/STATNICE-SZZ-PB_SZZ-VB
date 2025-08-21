import pandas as pd
from pymongo import MongoClient
import os, urllib.parse

if __name__ == "__main__":
    uri = f"mongodb://{"root"}:{"root"}@{"mongo"}:{"27017"}"
    client = MongoClient(uri, serverSelectionTimeoutMS=5000)
    db = client["mydb"] #reference na databázi
