import tkinter as tk
from tkinter import ttk, messagebox
from db import get_experiment_details, insert_model_for_experiment, insert_result_for_experiment_model
from ui import *
import json

def detail_selected_page(exp_id, title="Detail experimentu", size="1000x700"):
    # exp_id může být StringVar nebo číslo
    exp_id_val = int(exp_id.get())

    # === okno ===
    win = tk.Toplevel()
    win.title(title)
    win.geometry(size)

    main = ttk.Frame(win, padding=10)
    main.pack(fill="both", expand=True)

    # grid chování
    main.grid_columnconfigure(0, weight=1)
    main.grid_rowconfigure(2, weight=1)
    main.grid_rowconfigure(3, weight=1)

    # hlavička
    ttk.Label(main, text=f"Experiment ID: {exp_id_val}", font=("Arial", 12)).grid(
        row=0, column=0, sticky="w", pady=(0, 8)
    )

    # ---------- FORM: nový model ----------
    form = ttk.LabelFrame(main, text="Nový model pro experiment", padding=10)
    form.grid(row=1, column=0, sticky="nsew")
    form.grid_columnconfigure(1, weight=1)
    form.grid_rowconfigure(2, weight=1)

    # typ modelu
    ttk.Label(form, text="Typ modelu:").grid(row=0, column=0, sticky="w", padx=5, pady=4)
    model_type_var = tk.StringVar(value="RandomForestClassifier")
    model_type = ttk.Combobox(
        form, textvariable=model_type_var,
        values=["RandomForestClassifier", "SVC"], state="readonly"
    )
    model_type.grid(row=0, column=1, sticky="ew", padx=5, pady=4)

    # název
    ttk.Label(form, text="Název modelu:").grid(row=1, column=0, sticky="w", padx=5, pady=4)
    name_var = tk.StringVar()
    ttk.Entry(form, textvariable=name_var).grid(row=1, column=1, sticky="ew", padx=5, pady=4)

    # popis
    ttk.Label(form, text="Popis:").grid(row=2, column=0, sticky="nw", padx=5, pady=4)
    desc_text = tk.Text(form, height=4, wrap="word")
    desc_text.grid(row=2, column=1, sticky="nsew", padx=5, pady=4)

    # parametry
    params_frame = ttk.LabelFrame(form, text="Parametry", padding=8)
    params_frame.grid(row=3, column=0, columnspan=2, sticky="nsew", pady=(8, 0))
    params_frame.grid_columnconfigure(1, weight=1)

    # RF
    rf_frame = ttk.Frame(params_frame)
    rf_frame.grid(row=0, column=0, columnspan=2, sticky="nsew")
    rf_frame.grid_columnconfigure(1, weight=1)

    ttk.Label(rf_frame, text="n_estimators:").grid(row=0, column=0, sticky="w", padx=5, pady=2)
    n_estimators = tk.IntVar(value=100)
    ttk.Spinbox(rf_frame, from_=10, to=1000, increment=10, textvariable=n_estimators)\
        .grid(row=0, column=1, sticky="ew", padx=5, pady=2)

    ttk.Label(rf_frame, text="max_depth:").grid(row=1, column=0, sticky="w", padx=5, pady=2)
    max_depth = tk.StringVar(value="None")
    ttk.Entry(rf_frame, textvariable=max_depth).grid(row=1, column=1, sticky="ew", padx=5, pady=2)

    ttk.Label(rf_frame, text="max_features:").grid(row=2, column=0, sticky="w", padx=5, pady=2)
    max_features = tk.StringVar(value="auto")
    ttk.Combobox(rf_frame, textvariable=max_features,
                 values=["auto", "sqrt", "log2"], state="readonly")\
        .grid(row=2, column=1, sticky="ew", padx=5, pady=2)

    # SVC
    svc_frame = ttk.Frame(params_frame)
    svc_frame.grid(row=0, column=0, columnspan=2, sticky="nsew")
    svc_frame.grid_columnconfigure(1, weight=1)

    ttk.Label(svc_frame, text="C:").grid(row=0, column=0, sticky="w", padx=5, pady=2)
    svc_C = tk.DoubleVar(value=1.0)
    ttk.Spinbox(svc_frame, from_=0.01, to=1000.0, increment=0.01, textvariable=svc_C)\
        .grid(row=0, column=1, sticky="ew", padx=5, pady=2)

    ttk.Label(svc_frame, text="kernel:").grid(row=1, column=0, sticky="w", padx=5, pady=2)
    svc_kernel = tk.StringVar(value="rbf")
    ttk.Combobox(svc_frame, textvariable=svc_kernel,
                 values=["linear", "poly", "rbf", "sigmoid"], state="readonly")\
        .grid(row=1, column=1, sticky="ew", padx=5, pady=2)

    ttk.Label(svc_frame, text="gamma:").grid(row=2, column=0, sticky="w", padx=5, pady=2)
    svc_gamma = tk.StringVar(value="scale")
    ttk.Entry(svc_frame, textvariable=svc_gamma).grid(row=2, column=1, sticky="ew", padx=5, pady=2)

    # handle hodnot dle zvoleného typu modelu
    def update_params(*_):
        if model_type_var.get() == "SVC":
            rf_frame.grid_remove()
            svc_frame.grid()
        else:
            svc_frame.grid_remove()
            rf_frame.grid()
    model_type.bind("<<ComboboxSelected>>", update_params)
    update_params()

    # tlačítka formuláře
    btns = ttk.Frame(form)
    btns.grid(row=4, column=0, columnspan=2, sticky="e", pady=(10, 0))

    def on_submit():
        md = max_depth.get()
        params = (
            {"C": svc_C.get(), "kernel": svc_kernel.get(), "gamma": svc_gamma.get()}
            if model_type_var.get() == "SVC"
            else {
                "n_estimators": n_estimators.get(),
                "max_depth": None if str(md).strip().lower() == "none" else md,
                "max_features": max_features.get(),
            }
        )
        payload = {
            "experiment_id": exp_id_val,
            "model_type": model_type_var.get(),
            "name": name_var.get().strip(),
            "description": desc_text.get("1.0", "end-1c").strip(),
            "params": params,
            "artifact_uri": None,
            "artifact_blob": None,
        }
        if not payload["name"]:
            messagebox.showwarning("Chybí název", "Zadej název modelu.")
            return
        res = insert_model_for_experiment(**payload)
        if res:
            messagebox.showinfo("Hotovo", f"Model uložen (id={res['model_id']}).")
            refresh_tables()  # po uložení načti tabulky znovu
        else:
            messagebox.showerror("Chyba", "Nepodařilo se uložit model.")

    ttk.Button(btns, text="Vytvořit model", command=on_submit).grid(row=0, column=0, padx=5)
    ttk.Button(btns, text="Zavřít", command=win.destroy).grid(row=0, column=1, padx=5)

    # ---------- MODEL & RESULTS TABULKY ----------
    # Modely
    models_box = ttk.LabelFrame(main, text="Modely", padding=8)
    models_box.grid(row=2, column=0, sticky="nsew", pady=(12, 6))
    models_box.grid_columnconfigure(0, weight=1)
    models_box.grid_rowconfigure(0, weight=1)

    model_cols = ["model_id", "model_name", "model_type", "model_desc", "model_created", "params"]
    tree_models = make_treeview(models_box, model_cols, row=0, col=0)

    model_id = tk.StringVar(value="-")
    model_name = tk.StringVar(value="-")
    model_params = tk.StringVar(value="-")

    # on click modelu
    def on_click(event):
        item_id = tree_models.identify_row(event.y)
        if not item_id:
            return
        values = tree_models.item(item_id, "values")
        model_id.set(values[0])
        model_name.set(values[1])
        model_params.set(values[4])
        print("Kliknutý model", values)

        dlg = tk.Toplevel(win)
        dlg.title(f"Nový výsledek · exp#{exp_id_val} · model#{values[0]}")
        dlg.geometry("420x200")
        dlg.transient(win)
        dlg.grab_set()

        frm = ttk.Frame(dlg, padding=10)
        frm.pack(fill="both", expand=True)
        frm.grid_columnconfigure(1, weight=1)

        ttk.Label(frm, text="Experiment ID:").grid(row=0, column=0, sticky="w", padx=5, pady=5)
        ttk.Label(frm, text=str(exp_id_val)).grid(row=0, column=1, sticky="w", padx=5, pady=5)

        ttk.Label(frm, text="Model ID:").grid(row=1, column=0, sticky="w", padx=5, pady=5)
        ttk.Label(frm, text=str(values[0])).grid(row=1, column=1, sticky="w", padx=5, pady=5)

        ttk.Label(frm, text="Název metriky:").grid(row=2, column=0, sticky="w", padx=5, pady=5)
        metric_name_var = tk.StringVar(value="score")
        ttk.Entry(frm, textvariable=metric_name_var).grid(row=2, column=1, sticky="ew", padx=5, pady=5)

        ttk.Label(frm, text="Hodnota:").grid(row=3, column=0, sticky="w", padx=5, pady=5)
        metric_value_var = tk.StringVar(value="")
        ttk.Entry(frm, textvariable=metric_value_var).grid(row=3, column=1, sticky="ew", padx=5, pady=5)

        btns = ttk.Frame(frm)
        btns.grid(row=4, column=0, columnspan=2, sticky="e", pady=(10, 0))

        def save_result():
            name = metric_name_var.get().strip() or "score"
            try:
                val = float(metric_value_var.get().strip())
            except ValueError:
                messagebox.showwarning("Neplatná hodnota", "Zadej číselnou hodnotu metriky.")
                return
            try:
                # vlož výsledek pro dané (experiment_id, model_id)
                insert_result_for_experiment_model(exp_id_val, int(values[0]), name, val)
                messagebox.showinfo("Uloženo", f"Výsledek '{name}={val}' uložen.")
                dlg.destroy()
                refresh_tables()  # obnov tabulky dole
            except Exception as e:
                messagebox.showerror("Chyba", f"Nepodařilo se uložit výsledek:\n{e}")

        ttk.Button(btns, text="Uložit", command=save_result).grid(row=0, column=0, padx=6)
        ttk.Button(btns, text="Zavřít", command=dlg.destroy).grid(row=0, column=1)

    tree_models.bind("<Button-1>", on_click)

    # Výsledky
    results_box = ttk.LabelFrame(main, text="Výsledky", padding=8)
    results_box.grid(row=3, column=0, sticky="nsew", pady=(6, 0))
    results_box.grid_columnconfigure(0, weight=1)
    results_box.grid_rowconfigure(0, weight=1)

    result_cols = ["model_id", "model_name", "model_type", "result_id", "result_metric_name", "result_metric_value", "result_created"]
    tree_results = make_treeview(results_box, result_cols, row=0, col=0)

    # načtení dat a naplnění tabulek
    def load_data():
        return get_experiment_details(exp_id_val)

    def refresh_tables():
        rows = load_data()
        # modely: vezmeme unikátní podle model_id
        seen = set()
        model_rows = []
        for r in rows:
            mid = r.get("model_id")
            if mid not in seen and mid is not None:
                seen.add(mid)
                model_rows.append(r)
        populate_tree(tree_models, model_rows, model_cols)
        # výsledky: všechny řádky s result_id
        result_rows = [r for r in rows if r.get("result_id") is not None]
        populate_tree(tree_results, result_rows, result_cols)

    refresh_tables()

    # filtr výsledků podle vybraného modelu (double-click)
    def on_model_dblclick(event):
        item_id = tree_models.identify_row(event.y)
        if not item_id:
            return
        vals = tree_models.item(item_id, "values")
        # pořadí columns = model_cols
        try:
            selected_model_id = int(vals[0])
        except Exception:
            return
        # znovu načti a vyfiltruj jen výsledky pro daný model
        all_rows = load_data()
        filtered = [r for r in all_rows if r.get("model_id") == selected_model_id and r.get("result_id") is not None]
        populate_tree(tree_results, filtered, result_cols)

    tree_models.bind("<Double-1>", on_model_dblclick)

    return win
