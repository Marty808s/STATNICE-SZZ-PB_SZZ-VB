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
def populate_tree(tree, rows, columns):
    # clear
    for item in tree.get_children():
        tree.delete(item)
    for r in rows:
        if hasattr(r, "_mapping"):          # SQLAlchemy Row
            m = r._mapping
            values = [m.get(c) for c in columns]
        else:                                # dict
            values = [r.get(c) for c in columns]
        tree.insert("", "end", values=values)