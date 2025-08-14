from db import get_connection
import tkinter as tk
from tkinter import ttk, messagebox

# tool boxy
def make_button(parent, text, command, row, col):
    btn = ttk.Button(parent, text=text, command=command)
    btn.grid(row=row, column=col, padx=5, pady=5, sticky="e")
    return btn

def make_treeview(parent, columns, row=0, col=0, colspan=1, rowspan=1, expand=True):
    tree = ttk.Treeview(parent, columns=columns, show="headings")
    for colname in columns:
        tree.heading(colname, text=colname.capitalize())
        tree.column(colname, width=150)
    tree.grid(row=row, column=col, columnspan=colspan, rowspan=rowspan,
              sticky="nsew", padx=5, pady=5)
    return tree