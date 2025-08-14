from db import get_connection
from ui import *
from fun import *
import tkinter as tk
from tkinter import ttk

def landing():
    root = tk.Tk()
    root.title("Správa výsledků ML experimentů")
    root.geometry("800x800")

    form_frame = ttk.LabelFrame(root, text="Nový experiment", padding=10)
    form_frame.pack(fill="both", expand=True, padx=10, pady=10)

    # nastav, že sloupec 1 (s inputy) se má roztahovat
    form_frame.grid_columnconfigure(1, weight=1)
    # nastav, že řádek 1 (textarea) se má taky roztahovat
    form_frame.grid_rowconfigure(1, weight=1)

    # Název
    ttk.Label(form_frame, text="Název experimentu:").grid(row=0, column=0, sticky="w", padx=5, pady=5)
    entry_name = ttk.Entry(form_frame)
    entry_name.grid(row=0, column=1, sticky="ew", padx=5, pady=5)

    # Popis
    ttk.Label(form_frame, text="Stručný popis:").grid(row=1, column=0, sticky="nw", padx=5, pady=5)
    text_desc = tk.Text(form_frame, wrap="word")
    text_desc.grid(row=1, column=1, sticky="nsew", padx=5, pady=5)

    # Výběr modelu
    model = tk.StringVar()
    ttk.Label(form_frame, text="Parametry:").grid(row=2, column=0, sticky="nw", padx=5, pady=5)
    comboModel = ttk.Combobox(form_frame, textvariable=model, values=['AB', 'BC', 'MB', 'NB', 'NL', 'NS'])
    comboModel.grid(row=2, column=1, padx=5, pady=5)

    #Button na create
    create_exp = make_button(form_frame, text="Přidej experiment", command=createExperiment, row=3, col=0)

    #Výpis experimentů
    form_frame = ttk.LabelFrame(root, text="Existující experimenty", padding=10)
    form_frame.pack(fill="both", expand=True, padx=10, pady=40)


    root.mainloop()


if __name__ == "__main__":
    print("--APP--")
    con = get_connection()
    print(con)
    #demo()
    landing()




