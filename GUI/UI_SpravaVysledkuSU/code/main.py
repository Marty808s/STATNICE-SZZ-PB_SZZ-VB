from ui import *
from fun import *
import tkinter as tk
from tkinter import ttk

def landing():
    root = tk.Tk()
    root.title("Správa výsledků ML experimentů")
    root.geometry("1280x720")  # kompaktnější plocha

    # --- HLAVNÍ DVOU-SLOUPCOVÝ LAYOUT (vlevo data, vpravo detail) ---
    main = ttk.Frame(root, padding=10)
    main.pack(fill="both", expand=True)
    main.grid_columnconfigure(0, weight=3)  # levý sloupec (form+tabulka)
    main.grid_columnconfigure(1, weight=2)  # pravý sloupec (detail)
    main.grid_rowconfigure(0, weight=1)

    # ----------------- LEVÝ SLOUPEC -----------------
    left = ttk.Frame(main)
    left.grid(row=0, column=0, sticky="nsew", padx=(0,10))
    left.grid_columnconfigure(0, weight=1)

    # Formulář (nahoře vlevo)
    form_frame = ttk.LabelFrame(left, text="Nový experiment", padding=8)
    form_frame.grid(row=0, column=0, sticky="ew")
    form_frame.grid_columnconfigure(1, weight=1)
    form_frame.grid_rowconfigure(1, weight=1)

    ttk.Label(form_frame, text="Název experimentu:").grid(row=0, column=0, sticky="w", padx=5, pady=4)
    entry_name = ttk.Entry(form_frame)
    entry_name.grid(row=0, column=1, sticky="ew", padx=5, pady=4)

    ttk.Label(form_frame, text="Stručný popis:").grid(row=1, column=0, sticky="nw", padx=5, pady=4)
    text_desc = tk.Text(form_frame, wrap="word", height=4)
    text_desc.grid(row=1, column=1, sticky="nsew", padx=5, pady=4)

    create_exp = make_button(
        form_frame,
        text="Přidej experiment",
        command=lambda: create_experiment(entry_name.get(), text_desc.get("1.0","end-1c")),
        row=2,
        col=1
    )

    # Tabulka (dole vlevo)
    experiment_frame = ttk.LabelFrame(left, text="Existující experimenty", padding=8)
    experiment_frame.grid(row=1, column=0, sticky="nsew", pady=(10,0))
    left.grid_rowconfigure(1, weight=1)

    experiment_frame.grid_columnconfigure(0, weight=1)
    experiment_frame.grid_rowconfigure(0, weight=1)

    columns = ("id", "name", "description", "created_at")
    tree = make_treeview(parent=experiment_frame, columns=columns, row=0, col=0, colspan=2)
    data = get_experiments()
    populate_tree(tree, data, columns)

    def refresh_tree():
        populate_tree(tree, get_experiments(), columns)

    make_button(experiment_frame, text="Obnovit", command=refresh_tree, row=1, col=1)

    # ----------------- PRAVÝ SLOUPEC (DETAIL) -----------------
    detail = ttk.LabelFrame(main, text="Zvolený experiment", padding=10)
    detail.grid(row=0, column=1, sticky="nsew")
    detail.grid_columnconfigure(1, weight=1)

    id_var = tk.StringVar(value="-")
    name_var = tk.StringVar(value="-")
    desc_var = tk.StringVar(value="-")

    ttk.Label(detail, text="ID:").grid(row=0, column=0, sticky="w", padx=5, pady=4)
    ttk.Label(detail, textvariable=id_var).grid(row=0, column=1, sticky="w", padx=5, pady=4)

    ttk.Label(detail, text="Název:").grid(row=1, column=0, sticky="w", padx=5, pady=4)
    ttk.Label(detail, textvariable=name_var).grid(row=1, column=1, sticky="w", padx=5, pady=4)

    ttk.Label(detail, text="Popis:").grid(row=2, column=0, sticky="nw", padx=5, pady=4)
    # popis jako Label s wrapem, aby se vešel na pravou stranu
    desc_label = ttk.Label(detail, textvariable=desc_var, anchor="w", justify="left", wraplength=520)
    desc_label.grid(row=2, column=1, sticky="nwe", padx=5, pady=4)

    def detail_selected_page(exp_id, title="Nové okno", size="600x400"):
        # vytvoří nové okno
        new_win = tk.Toplevel()
        new_win.title(title)
        new_win.geometry(size)

        # příklad obsahu
        frame = ttk.Frame(new_win, padding=10)
        frame.pack(fill="both", expand=True)

        ttk.Label(frame, text=f"Tady je {title} | {exp_id.get()}", font=("Arial", 14)).pack(pady=10)
        ttk.Button(frame, text="Zavřít", command=new_win.destroy).pack(pady=5)

        return new_win

    make_button(detail, text="Zobrazit model",
        command=lambda: detail_selected_page(id_var, f"Detail experimentu s id {id_var.get()}","1200x600" ),
        row=1,
        col=1
    )



    # --- Double-click handler (aktualizuje jen proměnné → layout se nemění) ---
    def on_double_click(event):
        item_id = tree.identify_row(event.y)
        if not item_id:
            return
        values = tree.item(item_id, "values")
        id_var.set(values[0])
        name_var.set(values[1])
        desc_var.set(values[2] if len(values) > 2 else "-")

    tree.bind("<Double-1>", on_double_click)

    root.mainloop()


if __name__ == "__main__":
    print("--APP--")
    landing()
