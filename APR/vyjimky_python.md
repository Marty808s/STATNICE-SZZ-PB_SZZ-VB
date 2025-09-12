# Výjimky v Pythonu

## 🧩 Co jsou výjimky?
- **Výjimka (exception)** je událost, která přeruší běh programu, pokud není správně ošetřena.
- Typicky vznikají při chybách (dělení nulou, špatný vstup, přístup k neexistujícímu indexu).

---

## ⚡ Vyvolání výjimky (`raise`)
Pomocí `raise` můžeme **explicitně vyvolat výjimku**.

```python
# Vyvolání vestavěné výjimky
raise ValueError("Neplatná hodnota!")

# Vlastní příklad: dělení s kontrolou
def divide(a, b):
    if b == 0:
        raise ZeroDivisionError("Dělení nulou není povoleno!")
    return a / b

print(divide(10, 2))  # 5.0
print(divide(5, 0))   # Vyvolá ZeroDivisionError
```

---

## 🛡️ Ošetření výjimky (`try/except`)
Používá se pro zachycení a řešení chyb.

```python
try:
    x = int("abc")  # Chyba při převodu
except ValueError as e:
    print("Došlo k chybě:", e)
```

---

## 🔁 `try/except/else/finally`
- `try` – blok, kde může nastat výjimka
- `except` – co se stane, když chyba nastane
- `else` – provede se, pokud nenastane chyba
- `finally` – provede se vždy (např. uvolnění zdrojů)

```python
try:
    cislo = int("42")
except ValueError:
    print("Špatný vstup!")
else:
    print("Převod úspěšný:", cislo)
finally:
    print("Konec bloku try-except")
```

---

## 🎨 Vlastní výjimky
Lze vytvořit vlastní třídu děděním z `Exception`.

```python
class MyError(Exception):
    pass

def kontrola(x):
    if x < 0:
        raise MyError("Číslo nesmí být záporné!")

kontrola(-5)  # Vyvolá MyError
```
