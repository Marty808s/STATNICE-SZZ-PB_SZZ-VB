from db import *
import tkinter as tk
from tkinter import ttk

def create_experiment(name, description):
    insert_experiment(name, description)
