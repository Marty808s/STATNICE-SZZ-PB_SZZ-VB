# Příručka regulárních výrazů v Bashi

Tato příručka slouží jako praktický úvod do používání regulárních výrazů
v Bashi. Obsahuje přehled základní syntaxe, tipy a praktické příklady.

---

## 1. Základy regulárních výrazů

- `.` – libovolný znak  
- `*` – opakuje předchozí znak 0 a vícekrát  
- `+` – opakuje předchozí znak 1 a vícekrát  
- `?` – předchozí znak je nepovinný  
- `^` – začátek řádku  
- `$` – konec řádku  
- `[abc]` – jeden ze znaků a, b nebo c  
- `[^abc]` – jiný znak než a, b, c  
- `(abc)` – skupina znaků  

---

## 2. Použití v Bashi

### 2.1 grep

```bash
grep "pattern" soubor.txt
grep -E "regulární_výraz" soubor.txt   # Rozšířené regexy
```

#### Časté parametry `grep`
- `-i` – ignoruje velikost písmen  
- `-v` – invertuje shodu (zobrazí řádky, které NEodpovídají)  
- `-n` – zobrazí čísla řádků s výskytem shody  
- `-c` – spočítá počet shodných řádků  
- `-o` – vypíše pouze část řádku, která odpovídá regexu  
- `-A NUM` – vypíše N řádků **za** shodou  
- `-B NUM` – vypíše N řádků **před** shodou  
- `-C NUM` – vypíše N řádků **kolem** shody  
- `-r` nebo `-R` – rekurzivní hledání v adresáři  
- `--color=auto` – zvýrazní nalezený text  

#### Praktické příklady
```bash
# Najít všechny řádky obsahující "error"
grep "error" log.txt

# Najít bez ohledu na velikost písmen
grep -i "chyba" log.txt

# Spočítat počet výskytů "404" v logu
grep -c "404" access.log

# Zobrazit pouze IP adresy z logu
grep -oE "([0-9]{1,3}\.){3}[0-9]{1,3}" access.log

# Najít slovo na začátku řádku
grep "^START" data.txt

# Najít slovo na konci řádku
grep "END$" data.txt

# Rekurzivní hledání slova v celém adresáři
grep -r "TODO" ./projekt

# Zobrazit 3 řádky před a 2 po nalezeném vzoru
grep -B 3 -A 2 "Critical" system.log
```

### 2.2 sed

```bash
sed -n '/pattern/p' soubor.txt    # vypíše řádky obsahující pattern
sed 's/foo/bar/g' soubor.txt      # nahradí foo za bar
```

### 2.3 awk

```bash
awk '/pattern/ {print $0}' soubor.txt
```

### 2.4 Podmínky v Bashi

```bash
if [[ "ahoj" =~ ^a.*j$ ]]; then
  echo "Regex odpovídá"
fi
```

---

## 3. Praktické příklady

### 3.1 Najít e-maily v textu

```bash
grep -E "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}" soubor.txt
```

### 3.2 Validace IP adresy

```bash
if [[ "192.168.1.1" =~ ^([0-9]{1,3}\.){3}[0-9]{1,3}$ ]]; then
  echo "Validní IP"
fi
```

### 3.3 Najít telefonní čísla

```bash
grep -E "\+420[0-9]{9}" soubor.txt
```

### 3.4 Nahradit více mezer jednou

```bash
sed -E 's/ +/ /g' soubor.txt
```

### 3.5 Extrahovat čísla z textu

```bash
grep -oE "[0-9]+" soubor.txt
```

---

## 4. Tipy a triky

- Používej `grep -E` pro rozšířené regexy (lepší syntaxe).  
- V Bashi se doporučuje `[[ ... =~ regex ]]` místo `[`.  
- Testuj regexy online, např. na [regex101.com](https://regex101.com).  
- Pokud regex nefunguje, otestuj nejprve v `grep`.  

---

## 5. Shrnutí

Regulární výrazy jsou silný nástroj pro vyhledávání, validaci a
transformaci textu. V Bashi je lze použít v kombinaci s **grep**, **sed**, **awk** a přímo v podmínkách skriptů.
