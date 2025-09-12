# 🐍 Python a OOP – co umí

## 1. Třídy a objekty
- Všechno v Pythonu je objekt.
- Umíš definovat vlastní třídy s konstruktorem (`__init__`), atributy a metodami.

```python
class Osoba:
    def __init__(self, jmeno, vek):
        self.jmeno = jmeno
        self.vek = vek

    def pozdrav(self):
        return f"Ahoj, já jsem {self.jmeno}."
```

---

## 2. Dědičnost
- Python podporuje **jednoduchou** i **vícenásobnou dědičnost**.
- Pořadí hledání metod určuje **MRO (Method Resolution Order)**.

```python
class A: pass
class B(A): pass
class C(A): pass
class D(B, C): pass

print(D.__mro__)
```

---

## 3. Polymorfismus
- **Overriding** – přepisování metod.
- **Operátorový polymorfismus** – přes dunder metody (`__add__`, `__len__`).
- **Duck typing** – rozhoduje chování, ne typ.

```python
class Zvire:
    def zvuk(self): return "??"

class Pes(Zvire):
    def zvuk(self): return "haf"

def udelej_zvuk(zvire):
    print(zvire.zvuk())

udelej_zvuk(Pes())  # haf
```

---

## 4. Abstraktní třídy (ABC)
- Python nemá klíčové slovo `abstract`.
- Používá modul **`abc`**.

```python
from abc import ABC, abstractmethod

class Zvire(ABC):
    @abstractmethod
    def zvuk(self): pass

class Pes(Zvire):
    def zvuk(self): return "haf"
```

---

## 5. Rozhraní (interfaces)
- Python nemá formální `interface` jako Java.
- Alternativy:
  - abstraktní třídy bez implementace,
  - duck typing.

---

## 6. Zapouzdření (encapsulation)
- Python nemá skutečné private/protected, jen konvence:
  - `_atribut` → „protected“,
  - `__atribut` → pseudo-private (name mangling).

---

## 7. Speciální (dunder) metody
- `__init__`, `__str__`, `__repr__`, `__len__`, `__call__`, …
- Umožňují, aby se objekty chovaly jako vestavěné typy.

```python
class Kolekce:
    def __init__(self, data):
        self.data = data
    def __len__(self):
        return len(self.data)

k = Kolekce([1, 2, 3])
print(len(k))  # 3
```

---

## 8. Více paradigmat
- Python není čistě OOP – kombinuje OOP, procedurální a funkcionální styl.
- OOP nástroje nejsou povinné, ale k dispozici.

---

# ✅ Shrnutí
- Python umí: třídy, objekty, dědičnost, polymorfismus, zapouzdření.
- Podporuje abstraktní třídy (přes `abc`) → suplují rozhraní.
- Nemá formální `interface` jako Java, spoléhá na duck typing.
- OOP v Pythonu je **flexibilní**, ne přísně vynucené.
