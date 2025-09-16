# Pravděpodobnost a statistika – tahák ke státnicím

## Náhodná veličina a její charakteristiky

### Definice
- Náhodná veličina: \(X: \Omega \to \mathbb{R}\).  
- Distribuční funkce:  
  \[
  F(x) = P(X \leq x)
  \]

### Diskrétní veličina
- Pravděpodobnostní funkce:  
  \[
  p(x) = P(X = x), \quad \sum_{i} p(x_i) = 1
  \]
- Střední hodnota:  
  \[
  E[X] = \sum_{i} x_i p(x_i)
  \]
- Rozptyl:  
  \[
  \mathrm{Var}(X) = \sum_{i} (x_i - E[X])^2 p(x_i)
  \]

### Spojitá veličina
- Hustota:  
  \[
  f(x) \geq 0, \quad \int_{-\infty}^{\infty} f(x)\,dx = 1
  \]
- Střední hodnota:  
  \[
  E[X] = \int_{-\infty}^{\infty} x f(x)\,dx
  \]
- Rozptyl:  
  \[
  \mathrm{Var}(X) = \int_{-\infty}^{\infty} (x - E[X])^2 f(x)\,dx
  \]

### Vlastnosti
- \(E(a+bX) = a+bE[X]\)  
- \(E(X+Y) = E[X]+E[Y]\)  
- \(\mathrm{Var}(aX+b) = a^2\mathrm{Var}(X)\)  
- \(\mathrm{Var}(X+Y) = \mathrm{Var}(X)+\mathrm{Var}(Y)+2\mathrm{Cov}(X,Y)\)  

---

## Vybraná rozdělení

### Diskrétní
- **Binomické \(Bi(n,p)\):**
  \[
  P(X=k) = \binom{n}{k} p^k (1-p)^{n-k}
  \]
  \[
  E[X]=np,\quad \mathrm{Var}(X)=np(1-p)
  \]

- **Poissonovo \(Po(\lambda)\):**
  \[
  P(X=k) = \frac{\lambda^k e^{-\lambda}}{k!} e^{-\lambda}
  \]
  \[
  E[X]=\lambda,\quad \mathrm{Var}(X)=\lambda
  \]

- **Hypergeometrické \(Hy(N,A,n)\):**
  \[
  P(X=k) = \frac{\binom{A}{k}\binom{N-A}{n-k}}{\binom{N}{n}}
  \]
  \[
  E[X] = \frac{nA}{N},\quad \mathrm{Var}(X) = \frac{nA}{N}\left(1-\frac{A}{N}\right)\left(\frac{N-n}{N-1}\right)
  \]

### Spojitá
- **Normální \(N(\mu,\sigma^2)\):**
  \[
  f(x) = \frac{1}{\sigma \sqrt{2\pi}} \exp\left(-\frac{(x-\mu)^2}{2\sigma^2}\right)
  \]
  \[
  E[X]=\mu,\quad \mathrm{Var}(X)=\sigma^2
  \]

- **Exponenciální \(Exp(\lambda)\):**
  \[
  f(x) = \lambda e^{-\lambda x},\quad x\geq 0
  \]
  \[
  E[X]=\frac{1}{\lambda},\quad \mathrm{Var}(X)=\frac{1}{\lambda^2}
  \]

- **χ² rozdělení (df = k):**
  \[
  X = \sum_{i=1}^k Z_i^2,\quad Z_i \sim N(0,1)
  \]

- **Beta rozdělení \(B(\alpha,\beta)\):**
  \[
  f(x) = \frac{\Gamma(\alpha+\beta)}{\Gamma(\alpha)\Gamma(\beta)} x^{\alpha-1}(1-x)^{\beta-1},\quad 0<x<1
  \]

---

## Intervaly spolehlivosti

### Definice
- Interval, kde se s pravděpodobností \(1-\alpha\) nachází parametr (např. \(\mu, \sigma^2, p\)).  
- **Hladina spolehlivosti:** \(1-\alpha\).  
- **Hladina významnosti:** \(\alpha\).  

---

### Pro střední hodnotu \(\mu\)
- Pokud známe \(\sigma\):  
  \[
  \left(\bar{X} - z_{1-\alpha/2}\frac{\sigma}{\sqrt{n}},\; \bar{X} + z_{1-\alpha/2}\frac{\sigma}{\sqrt{n}}\right)
  \]
- Pokud \(\sigma\) neznáme:  
  \[
  \left(\bar{X} - t_{n-1;1-\alpha/2}\frac{s}{\sqrt{n}},\; \bar{X} + t_{n-1;1-\alpha/2}\frac{s}{\sqrt{n}}\right)
  \]

### Pro rozptyl \(\sigma^2\)
\[
\left(\frac{(n-1)s^2}{\chi^2_{n-1;1-\alpha/2}},\; \frac{(n-1)s^2}{\chi^2_{n-1;\alpha/2}}\right)
\]

### Pro relativní četnost \(p\)
\[
\left(\hat{p} - z_{1-\alpha/2}\sqrt{\frac{\hat{p}(1-\hat{p})}{n}},\; \hat{p} + z_{1-\alpha/2}\sqrt{\frac{\hat{p}(1-\hat{p})}{n}}\right)
\]

---

### Interpretace
- „95% interval spolehlivosti“ neznamená, že µ má 95% šanci být uvnitř.  
- Znamená: kdybych výpočet opakoval mnohokrát, 95 % intervalů by µ obsahovalo.  

### Vliv rozsahu výběru
- **Čím větší n** → užší interval.  
- **Čím větší rozptyl** → širší interval.  
- **Vyšší spolehlivost (nižší α)** → širší interval.  

### Alternativy
- Pokud nejsou splněny předpoklady (nenormální rozdělení, malé n):  
  - **neparametrické intervaly** (percentily),  
  - **bootstrapové intervaly** (simulace).  

---
