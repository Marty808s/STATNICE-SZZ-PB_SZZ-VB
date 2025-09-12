# Python – Tahák základů OOP, funkcí a výjimek

## 🔢 Základní třídy (`int`, `float`, `bool`, `str`)

### int
```python
a = 10
b = 3
a + b   # 13
a // b  # 3
a % b   # 1
a ** b  # 1000
abs(-10)    # 10
int("42")   # 42
```

### float
```python
x = 3.14
y = -2.5
x + y   # 0.64
round(x, 1)  # 3.1
float("2.5") # 2.5
```

### bool
```python
t = True
f = False
t and f   # False
5 > 3     # True
bool("ahoj")  # True
```

### str
```python
s = "Python"
s + " rocks"   # "Python rocks"
s[0:3]         # "Pyt"
s.upper()      # "PYTHON"
" ".join(["Ahoj", "světe"])  # "Ahoj světe"
len(s)         # 6
```

---

## 🛠️ Vlastní funkce

```python
# Poziční parametry
def secti(a, b):
    return a + b

# Pojmenované parametry
def predstav_se(jmeno, vek):
    return f"Jmenuji se {jmeno} a je mi {vek} let."
predstav_se(jmeno="Petr", vek=20)

# Výchozí parametry
def pozdrav(jmeno, jazyk="cs"):
    if jazyk == "cs":
        return f"Ahoj, {jmeno}!"
    else:
        return f"Hello, {jmeno}!"
```

---

## 🧩 Výjimky

### Vyvolání
```python
raise ValueError("Neplatná hodnota!")
```

### Ošetření
```python
try:
    x = int("abc")
except ValueError as e:
    print("Chyba:", e)
```

### Vlastní výjimka
```python
class MyError(Exception):
    pass

raise MyError("Moje vlastní chyba")
```

---

## 🏗️ Vlastní třídy

```python
class Osoba:
    def __init__(self, jmeno, vek):  # konstruktor
        self.jmeno = jmeno           # vlastnost
        self.vek = vek

    def pozdrav(self):               # metoda
        return f"Ahoj, já jsem {self.jmeno}."

    def __str__(self):               # speciální metoda
        return f"Osoba({self.jmeno}, {self.vek})"

    def __repr__(self):
        return f"Osoba(jmeno={self.jmeno!r}, vek={self.vek!r})"

    def __contains__(self, znak):
        return znak in self.jmeno
```

---

## 👨‍👩‍👧‍👦 Dědičnost a abstraktní třídy

```python
from abc import ABC, abstractmethod

class Zvire(ABC):
    @abstractmethod
    def zvuk(self): ...

class Pes(Zvire):
    def zvuk(self): return "haf"
```

---

## 🔄 MRO (Method Resolution Order)

```python
class A: pass
class B(A): pass
class C(A): pass
class D(B, C): pass

print(D.__mro__)
# (<class '__main__.D'>, <class '__main__.B'>, <class '__main__.C'>, <class '__main__.A'>, <class 'object'>)
```

---

## 🦆 Duck typing

```python
def udelej_zvuk(zvire):
    print(zvire.zvuk())

class Pes:
    def zvuk(self): return "haf"

class Auto:
    def zvuk(self): return "brm brm"

udelej_zvuk(Pes())   # haf
udelej_zvuk(Auto())  # brm brm
```

---

## 🎭 Polymorfismus + super()

### Překrývání metod
```python
class Zvire:
    def zvuk(self): return "??"

class Kocka(Zvire):
    def zvuk(self): return "mňau"
```

### super()
```python
class Zvire:
    def __init__(self, jmeno): self.jmeno = jmeno

class Pes(Zvire):
    def __init__(self, jmeno, rasa):
        super().__init__(jmeno)   # volá rodiče
        self.rasa = rasa
```

---

## 🖊️ input()

```python
jmeno = input("Zadej jméno: ")
vek = int(input("Zadej věk: "))
print("Za rok ti bude:", vek + 1)
```

---

## 📦 *args a **kwargs

```python
def secti(*args):
    return sum(args)

def vypis_info(**kwargs):
    for k, v in kwargs.items():
        print(k, ":", v)

def demo(a, b, *args, c=0, **kwargs):
    print(a, b, args, c, kwargs)

# Rozbalení
nums = [1, 2, 3]
print(secti(*nums))   # 6

data = {"a": 1, "b": 2, "c": 3}
print(secti(**data))  # 6
```

---

# ✅ Shrnutí
- `int`, `float`, `bool`, `str` = základní typy
- Funkce: poziční/pojmenované parametry, `return`
- Výjimky: `raise`, `try/except`, vlastní výjimky
- Třídy: konstruktor, metody, speciální metody (`__str__`, `__repr__`, `__contains__`)
- Dědičnost: jednoduchá, abstraktní třídy, MRO
- Polymorfismus: překrývání, operátory, duck typing
- `super()` = volání metody rodiče podle MRO
- `input()` = vždy string → nutný převod
- `*args`, `**kwargs` = libovolný počet argumentů
