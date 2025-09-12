# 📍 Jak odhadnout β (beta) a γ (gamma) ze skutečných dat

## 1. Připomenutí rovnic SIR
\[
\frac{dI}{dt} = \beta \cdot \frac{S \cdot I}{N} - \gamma I
\]  
\[
\frac{dR}{dt} = \gamma I
\]

- **β (beta)** řídí, jak rychle přibývají noví nakažení.  
- **γ (gamma)** řídí, jak rychle lidé přecházejí z infekčních do R (uzdravení + úmrtí).  

---

## 2. Odhad γ (gamma)
Z rovnice pro R:  
\[
\frac{dR}{dt} = \gamma I
\]

Tedy:  
\[
\gamma \approx \frac{\Delta R}{I}
\]

👉 V praxi:  
- spočítej **denní přírůstky R** (nově vyléčení + úmrtí),  
- vyděl **aktuálním počtem infekčních (I)**.  

---

## 3. Odhad β (beta)
Z rovnice pro I:  
\[
\frac{dI}{dt} = \beta \cdot \frac{S \cdot I}{N} - \gamma I
\]

Převedeno:  
\[
\beta \approx \frac{\frac{\Delta I}{I} + \gamma}{S/N}
\]

👉 V praxi:  
- spočítej **relativní růst infekčních** (\(\Delta I / I\)),  
- přičti odhadnutou γ,  
- poděl poměrem **S/N** (kolik procent populace je ještě vnímavých).  

---

## 4. Reprodukční číslo R₀
Jakmile máš β a γ:  
\[
R_0 = \frac{\beta}{\gamma}
\]

- Pokud **R₀ > 1**, epidemie roste.  
- Pokud **R₀ < 1**, epidemie slábne.  

---

## 📊 Praktické kroky v Pythonu

```python
pop_total = 10.9e6

# 1. Aktuálně nakažení (I)
I = data["kumulativni_pocet_nakazenych"] - (
    data["kumulativni_pocet_vylecenych"] + data["kumulativni_pocet_umrti"]
)

# 2. Denní přírůstky R
dR = data["prirustkovy_pocet_vylecenych"] + data["prirustkovy_pocet_umrti"]

# 3. Odhad γ
gamma = dR / I

# 4. Denní přírůstky I
dI = I.diff()

# 5. Počet vnímavých (S)
S = pop_total - data["kumulativni_pocet_nakazenych"]

# 6. Odhad β
beta = ((dI / I) + gamma) / (S / pop_total)

# 7. Reprodukční číslo
R0 = beta / gamma
```

---

# ✅ Shrnutí
- **γ (gamma)** = kolik lidí odchází z I do R, relativně k počtu I.  
- **β (beta)** = jak rychle přibývají nakažení, relativně k S/N.  
- **R₀ = β/γ** určuje, zda epidemie roste (>1) nebo klesá (<1).  
