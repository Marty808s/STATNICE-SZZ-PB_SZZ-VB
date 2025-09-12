# Základní třídy v Pythonu (int, float, bool, str)

## 🔢 `int` (celá čísla)
```python
a = 10
b = 3

# Aritmetické operace
a + b   # 13
a - b   # 7
a * b   # 30
a / b   # 3.333... (float)
a // b  # 3 (celé dělení)
a % b   # 1 (zbytek po dělení)
a ** b  # 1000 (mocnina)

# Vestavěné funkce
abs(-10)    # 10
pow(2, 5)   # 32
divmod(10, 3) # (3, 1)

# Přetypování
int("42")   # 42
int(3.7)    # 3
```

---

## 🔢 `float` (desetinná čísla)
```python
x = 3.14
y = -2.5

# Aritmetika (stejné jako u int)
x + y   # 0.64
x * y   # -7.85
x / y   # -1.256

# Zaokrouhlování
round(x)     # 3
round(x, 1)  # 3.1
abs(y)       # 2.5

# Porovnávání
x > y   # True
x == 3.14  # True

# Přetypování
float("2.5")  # 2.5
float(10)     # 10.0
```

---

## ✅ `bool` (logické hodnoty)
```python
t = True
f = False

# Logické operace
t and f   # False
t or f    # True
not t     # False

# Porovnávání
5 > 3     # True
10 == 2   # False
bool(0)   # False
bool("")  # False
bool("ahoj")  # True
```

---

## 🔤 `str` (řetězce)
```python
s = "Python"
t = "rocks"

# Spojování a násobení
s + " " + t   # "Python rocks"
s * 3         # "PythonPythonPython"

# Indexování a slicing
s[0]    # 'P'
s[-1]   # 'n'
s[0:3]  # "Pyt"

# Základní metody
s.lower()      # "python"
s.upper()      # "PYTHON"
s.startswith("Py")  # True
s.endswith("on")    # True
s.replace("Py", "My")  # "Mython"
s.split("t")   # ["Py", "hon"]
" ".join(["Ahoj", "světe"])  # "Ahoj světe"
len(s)         # 6

# Přetypování
str(123)   # "123"
str(True)  # "True"
```
