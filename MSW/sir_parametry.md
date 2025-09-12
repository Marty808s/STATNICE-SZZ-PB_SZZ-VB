# 📍 Parametry SIR modelu – β (beta) a γ (gamma)

## 🔹 β (beta) – infekčnost
- Udává **rychlost šíření infekce**.  
- Pravděpodobnost, že dojde k přenosu nemoci při kontaktu mezi jedním **vnímavým (S)** a jedním **nakaženým (I)** během jednotky času.  
- Čím vyšší β, tím rychleji roste počet nakažených.  
- Ovlivňují ji faktory jako:  
  - četnost kontaktů mezi lidmi,  
  - nakažlivost patogenu,  
  - opatření (roušky, lockdown, očkování).  

---

## 🔹 γ (gamma) – zotavení
- Udává **rychlost uzdravení (nebo vyřazení) nakažených**.  
- Je převrácenou hodnotou **průměrné doby infekčnosti**:  

\[
\gamma = \frac{1}{\text{doba infekčnosti}}
\]

Příklady:  
- průměrná infekčnost 10 dní → γ = 0.1  
- průměrná infekčnost 5 dní → γ = 0.2  

---

## 📍 Základní rovnice SIR modelu

\[
\frac{dS}{dt} = -\beta \cdot \frac{S \cdot I}{N}
\]

\[
\frac{dI}{dt} = \beta \cdot \frac{S \cdot I}{N} - \gamma I
\]

\[
\frac{dR}{dt} = \gamma I
\]

- **S** = počet vnímavých  
- **I** = počet nakažených  
- **R** = počet uzdravených/odstraněných  
- **N** = celková populace  

---

## 📍 Odvozený parametr – R₀ (reprodukční číslo)
\[
R_0 = \frac{\beta}{\gamma}
\]

- Pokud **R₀ > 1**, epidemie roste.  
- Pokud **R₀ < 1**, epidemie slábne a zaniká.  

---

# ✅ Shrnutí
- **β (beta)** = míra infekčnosti (jak rychle se nemoc šíří).  
- **γ (gamma)** = míra uzdravování (jak rychle lidé přecházejí do R).  
- Poměr **β/γ** určuje, zda epidemie roste (**R₀ > 1**) nebo klesá (**R₀ < 1**).  
