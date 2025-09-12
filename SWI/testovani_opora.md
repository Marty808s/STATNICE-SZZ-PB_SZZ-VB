# Testování softwaru

Testování softwaru je proces ověřování, zda systém funguje podle požadavků, je spolehlivý a splňuje očekávání uživatele.  
Cílem není jen najít chyby, ale také **předcházet jejich vzniku a zajistit kvalitu**.

---

## 1. Úrovně testování

### 1.1 Jednotkové testování (Unit Testing)
- Testování jednotlivých částí kódu (funkce, třídy, metody).  
- Automatizovatelné (např. JUnit, NUnit, pytest).  
- Cíl: Ověřit, že komponenta funguje izolovaně správně.  

### 1.2 Integrační testování (Integration Testing)
- Ověření spolupráce více komponent dohromady.  
- Testují se rozhraní mezi moduly.  
- Přístupy: shora-dolů, zdola-nahoru, sandwich.  

### 1.3 Systémové testování (System Testing)
- Testování celého systému jako celku.  
- Kontrola, zda software odpovídá specifikaci.  
- Provádí se často black-box přístupem.  

### 1.4 Akceptační testování (Acceptance Testing)
- Poslední fáze testování před nasazením.  
- Cíl: Ověřit, že systém splňuje očekávání zákazníka.  
- Uživatel nebo zákazník provádí tzv. **UAT (User Acceptance Testing)**.  

---

## 2. Typy testů podle účelu

- **Funkční testy** – ověřují, zda systém dělá, co má.  
- **Nefunkční testy** – zkoumají kvalitu systému:  
  - Výkonnostní (Performance, Load, Stress)  
  - Bezpečnostní (Security)  
  - Použitelnosti (Usability)  
  - Kompatibility (Compatibility)  
  - Spolehlivosti (Reliability)  
- **Regresní testování** – ověřuje, že nové změny nerozbily staré funkce.  
- **Smoke testování** – rychlé ověření, zda systém „nezahoří“ při spuštění.  
- **Exploratory testing** – tester aktivně hledá chyby bez předem daného scénáře.  

---

## 3. Přístupy k testování

### 3.1 Black-box testing
- Tester nezná vnitřní strukturu kódu.  
- Testuje se pouze vstup–výstup.  
- Vhodné pro systémové a akceptační testy.  

### 3.2 White-box testing
- Tester má přístup ke kódu.  
- Testuje se logika, větve, cesty.  
- Vhodné pro unit testy.  

### 3.3 Grey-box testing
- Kombinace obou přístupů.  
- Tester má částečné znalosti o vnitřní struktuře.  

---

## 4. Automatizované vs. manuální testování

- **Manuální testování** – provádí tester ručně, vhodné pro explorativní testy.  
- **Automatizované testování** – nástroje spouští testy automaticky, vhodné pro regresní a unit testy.  

---

## 5. TDD – Test-Driven Development

**Test-Driven Development** je vývojový přístup, kdy se nejdříve píše test a teprve poté implementace.  

### Postup (cyklus „Red-Green-Refactor“)
1. **Red** – napsání testu, který zatím selže.  
2. **Green** – implementace minimálního kódu, aby test prošel.  
3. **Refactor** – úprava kódu pro lepší kvalitu, testy zůstávají zelené.  

### Výhody
- Zajištění testovatelnosti kódu.  
- Minimalizace chyb.  
- Lepší návrh architektury.  

![TDD Cycle](https://upload.wikimedia.org/wikipedia/commons/0/0b/Tdd_cycle.png)

---

## 6. BDD – Behavior-Driven Development
- Rozšíření TDD.  
- Zaměřuje se na **chování systému z pohledu uživatele**.  
- Testy jsou psané v srozumitelné formě (např. Gherkin syntax: Given–When–Then).  

---

## 7. Continuous Testing
- Součást **CI/CD pipeline** (Continuous Integration / Continuous Delivery).  
- Automatické spouštění testů při každé změně kódu.  
- Zajišťuje rychlou zpětnou vazbu a vyšší kvalitu.  

---

## Shrnutí
- Testování se provádí na více úrovních: **unit → integration → system → acceptance**.  
- Existují různé typy testů: funkční, nefunkční, regresní, explorativní atd.  
- Přístupy: **black-box, white-box, grey-box**.  
- Moderní praktiky: **TDD, BDD, Continuous Testing**.  