# Tahák: Epidemické a ekologické modely (SIR, SIS, SIRD, SIRVD, Lotka–Volterra)

Tento tahák shrnuje základní **vzorce** a pojmy z klasických modelů šíření nákazy a z ekologického modelu **Lotka–Volterra (predátor–kořist)**. Vše je v kompaktní podobě pro rychlé použití.

---

## Notace (společná)
- \(S(t)\): vnímaví (susceptible)
- \(I(t)\): nakažení (infected)
- \(R(t)\): zotavení/odstranění (recovered/removed)
- \(D(t)\): zemřelí (deceased)
- \(V(t)\): očkovaní (vaccinated)
- \(N(t)\): velikost populace, obvykle \(N = S + I + R\) (+ případně \(+V\) a/nebo \(-D\) dle konvence)
- Parametry:
  - \(\beta\): míra přenosu (kontakty × pravděpodobnost infekce na kontakt)
  - \(\gamma\): míra zotavení (\(\gamma = 1/D\) pro průměrnou infekční dobu \(D\))
  - \(\mu\): míra úmrtí na infekci (fatalita za jednotku času)
  - \(\nu\): míra očkování (odchod ze \(S\) do \(V\))

Základní reprodukční číslo:  
\[\; R_0 = \frac{\beta}{\gamma} \quad\text{(pro SIR/SIS bez dalších toků, při \(S \approx N\))}\]

Efektivní reprodukční číslo:  
\[\; R_t = R_0 \cdot \frac{S(t)}{N(t)}\]

---

## SIR
\[\begin{aligned}
\frac{dS}{dt} &= -\beta \frac{S I}{N}, \\
\frac{dI}{dt} &= \beta \frac{S I}{N} - \gamma I, \\
\frac{dR}{dt} &= \gamma I.
\end{aligned}\]

Poznámky:
- \(N = S + I + R\) (konstantní, pokud neuvažujeme demografii).
- Vrchol epidemie nastává, když \(dI/dt = 0 \Rightarrow S/N = \gamma/\beta = 1/R_0\).

---

## SIS
\[\begin{aligned}
\frac{dS}{dt} &= -\beta \frac{S I}{N} + \gamma I, \\
\frac{dI}{dt} &= \beta \frac{S I}{N} - \gamma I.
\end{aligned}\]

Poznámky:
- Zotavení vrací jedince ze \(I\) zpět do \(S\) (nevzniká \(R\)).
- Endemická rovnováha (pro \(R_0>1\)): \(I^*/N = 1 - \frac{1}{R_0}\).

---

v

Poznámky:
- Infekční smrtelnost (instantní): \(\mu\).  
- Průměrný „odchod“ z \(I\): \(\gamma + \mu\).  
- „Efektivní“ reprodukční číslo lze psát \(R_0 = \beta / (\gamma+\mu)\).

---

## SIRVD (očkování + úmrtnost)
\[\begin{aligned}
\frac{dS}{dt} &= -\beta \frac{S I}{N} - \nu S, \\
\frac{dI}{dt} &= \beta \frac{S I}{N} - \gamma I - \mu I, \\
\frac{dR}{dt} &= \gamma I, \\
\frac{dV}{dt} &= \nu S, \\
\frac{dD}{dt} &= \mu I.
\end{aligned}\]

Poznámky:
- Očkování snižuje \(S\) rychlostí \(\nu S\) a tím i \(R_t\).  
- Pokud je vakcína sterilizující, očkovaní již nepřispívají k přenosu (jinak lze doplnit účinnost \(\epsilon\) přes redukci \(\beta\) či toku \(S\to V\)).

---

## Lotka–Volterra (predátor–kořist)
Proměnné:
- \(x(t)\): kořist (např. zajíci)
- \(y(t)\): predátor (např. rysi)

Parametry (standardní značení):
- \(\alpha\): intrinsická míra růstu kořisti bez predátorů
- \(\beta\): míra predace (interakční koeficient)
- \(\delta\): míra „konverze“ kořisti na nové predátory
- \(\gamma\): úmrtnost predátora bez kořisti

Rovnice:
\[\begin{aligned}
\frac{dx}{dt} &= \alpha x - \beta x y, \\
\frac{dy}{dt} &= \delta x y - \gamma y.
\end{aligned}\]

Rovnovážné body:
- \(E_0 = (x^*, y^*) = (0, 0)\) (triviální),  
- \(E_1 = \left(\frac{\gamma}{\delta}, \frac{\alpha}{\beta}\right)\) (ne‑triviální cykly okolo).

Integrál pohybu (konstantní „energie“ systému — užitečné pro kontrolu simulací):
\[\; H(x,y) = \delta x - \gamma \ln x + \beta y - \alpha \ln y = \text{konst.}\]

Diskretizace (explicitní Euler pro rychlou simulaci, krok \(h\)):
\[\begin{aligned}
x_{t+h} &= x_t + h\,(\alpha x_t - \beta x_t y_t), \\
y_{t+h} &= y_t + h\,(\delta x_t y_t - \gamma y_t).
\end{aligned}\]

---

## Užitečné doplňky

**Odhad \(\beta, \gamma\) z dat (SIR/SIS):**
- Z časových řad \(I(t), S(t)\) (nebo incidence) lze při známém \(N\) a krátkých krocích aproximovat:
  \[ \gamma \approx -\frac{\Delta R}{\Delta t}\Big/\bar{I}, \qquad 
     \beta \approx \left(\frac{\Delta I}{\Delta t} + \gamma \bar{I}\right)\Big/\left(\bar{S}\,\bar{I}/N\right) \]
  (s \(\bar{\cdot}\) jako průměry přes krátké okno).

**Prahové podmínky pro šíření:**
- Růst na začátku epidemie, pokud \(R_0 > 1\) resp. \(S(0)/N > 1/R_0\).

**Tip pro numeriku:**
- Stabilnější integrace: Runge–Kutta 4. řádu (RK4).  
- Kladné řešení hlídat projekcí na \([0,\infty)\) po kroku.

---

## Mini‑recepty (rychlé „plug‑in“)
- **Vrchol infekce v SIR:** nastane, když \(S/N = 1/R_0\).  
- **Konečný podíl nakažených v SIR (implicitně):**
  \[\; 1 - \frac{S_\infty}{S_0} = 1 - \exp\!\left(-R_0 \left(1 - \frac{S_\infty}{N}\right)\right)\]
  (řeší se numericky pro \(S_\infty\)).
- **Endemie v SIS:** \(I^*/N = 1 - 1/R_0\) pro \(R_0>1\).

---

### Rychlé jednotky
- Doba infekčnosti \(D\) (dny) ⇒ \(\gamma = 1/D\ \text{[den}^{-1}\text{]}\).
- Kontaktní míra \(\beta\) v \(\text{[den}^{-1}\text{]}\) (při normalizaci \(\tfrac{SI}{N}\)).

---

> Tento tahák je koncipován tak, aby seděl k buňkám ve tvém notebooku: SIR, SIS, SIRD, SIRVD a Lotka–Volterra. Pokud máš v kódu jiné značení parametrů, dopiš si je do „Notace“.
