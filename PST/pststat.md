# Náhodná veličina a její charakteristiky – srozumitelně

---

## 1. Co je náhodná veličina?
- Číslo, které popisuje výsledek náhodného jevu.  
- **Příklad:**
  - hod kostkou → `X = padlé číslo`  
  - doba čekání na autobus → `T = čas v minutách`  

### Typy
- **Diskrétní** – jen některé hodnoty (často celá čísla).  
  - příklad: počet dětí v rodině  
- **Spojitá** – libovolné hodnoty v intervalu.  
  - příklad: výška člověka  

---

## 2. Distribuční funkce
- `F(x) = P(X ≤ x)`  
- Říká: jaká je pravděpodobnost, že náhodná veličina nabude hodnoty menší nebo rovné `x`.  
- Vlastnosti:
  - Roste od 0 do 1  
  - Nikdy neklesá  
  - Úplně popisuje rozdělení veličiny  

---

## 3. Pravděpodobnostní funkce vs. hustota

### Diskrétní veličiny
- **Pravděpodobnostní funkce (p(x))**  
  - `p(x) = P(X = x)`  
  - součet všech `p(x)` = 1  
- **Příklad:** hod mincí:  
  - `p(1) = 0.5`, `p(0) = 0.5`

### Spojité veličiny
- **Hustota pravděpodobnosti (f(x))**  
  - `f(x) ≥ 0`, `∫ f(x) dx = 1`  
  - pravděpodobnost intervalu: `P(a ≤ X ≤ b) = ∫ f(x) dx`  
- Pravděpodobnost jediné hodnoty = 0  

---

## 4. Číselné charakteristiky

### Střední hodnota
- „Průměrná hodnota, kdybych pokus opakoval hodněkrát“.  
- Diskrétní: `E[X] = Σ x·p(x)`  
- Spojitá: `E[X] = ∫ x·f(x) dx`  
- **Příklad:**  
  - hod kostkou → `E[X] = 3.5`

### Rozptyl a směrodatná odchylka
- „Jak moc se hodnoty liší od průměru“.  
- `Var(X) = E[(X - E[X])²]`  
- `σ = √Var(X)`  

### Kvantily
- Hodnota, která dělí data na určité části.  
- Medián = prostřední hodnota.  
- Kvartily = čtvrtiny, percentily = setiny.  

### Modus
- Hodnota, která je nejčastější / nejpravděpodobnější.  

---

## 5. Typická rozdělení

### Diskrétní
- **Bernoulli(p):** jeden pokus (úspěch/neúspěch).  
- **Binomické(n,p):** počet úspěchů v n pokusech.  
- **Poissonovo(λ):** počet vzácných událostí (nehody, hovory).  
- **Hypergeometrické(N,A,n):** výběr bez vracení.  

### Spojitá
- **Rovnoměrné U(a,b):** všechny hodnoty stejně pravděpodobné.  
- **Exponenciální Exp(λ):** doba do příští události.  
- **Normální N(µ,σ²):** Gaussova křivka, nejdůležitější rozdělení.  
- **χ² rozdělení:** testy rozptylu, nezávislosti.  
- **t-rozdělení:** malé výběry, odhady průměru.  
- **Beta(α,β):** interval [0,1], často v Bayesově statistice.  

---

## 6. Intervaly spolehlivosti

### Co to je
- Rozsah hodnot, kde očekáváme skutečný parametr (např. průměr).  
- Typicky 95% nebo 99% spolehlivost.  

### Jak to funguje
- Spočítáme průměr z dat → vytvoříme interval kolem něj.  
- Interval říká: pokud bychom měření opakovali, ve většině případů (např. 95 %) by tento interval obsahoval skutečnou hodnotu.  

### Vzorce
- **Pro průměr µ:**  
  - známý σ: `X̄ ± z·σ/√n`  
  - neznámý σ: `X̄ ± t·s/√n`  

- **Pro rozptyl σ²:**  
  `((n-1)·s² / χ²_max , (n-1)·s² / χ²_min)`  

- **Pro podíl p:**  
  `p̂ ± z·√(p̂(1-p̂)/n)`  

### Vliv vzorku
- Větší n → užší interval.  
- Větší rozptyl → širší interval.  
- Vyšší spolehlivost (99 %) → širší interval.  

### Když to nejde
- Nenormální data, malé n → použiju **neparametrické nebo bootstrapové metody**.  

---

## 7. Shrnutí
- Náhodná veličina je číselný model náhodného jevu.  
- Distribuční funkce určuje rozdělení.  
- Diskrétní veličiny mají pravděpodobnostní funkci, spojité hustotu.  
- Číselné charakteristiky (průměr, rozptyl, kvantily) popisují chování veličiny.  
- Základní rozdělení: binomické, Poissonovo, normální, exponenciální, atd.  
- Intervaly spolehlivosti jsou základní nástroj pro odhady parametrů v praxi.  
