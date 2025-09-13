# Softwarové inženýrství – teoretická opora

Softwarové inženýrství je disciplína, která se zabývá **systematickým vývojem, provozem a údržbou softwarových systémů**.  
Jeho cílem je vytvářet kvalitní, spolehlivý a udržitelný software.

---

## 1. Životní cyklus softwaru (SDLC – Software Development Life Cycle)

Životní cyklus softwaru popisuje jednotlivé fáze vývoje softwarového produktu.  
Existuje více modelů (vodopádový, spirálový, agilní), ale obecné fáze jsou:

1. **Analýza požadavků** – zjišťování potřeb uživatele a specifikace.  
2. **Návrh systému** – architektura, datové modely, design.  
3. **Implementace** – programování.  
4. **Testování** – ověřování správnosti a kvality.  
5. **Nasazení (Deployment)** – uvedení do provozu.  
6. **Údržba (Maintenance)** – opravy, aktualizace, nové verze.  

![SDLC](https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fblog.gitguardian.com%2Fcontent%2Fimages%2F2022%2F05%2FSDLC2.png&f=1&nofb=1&ipt=e61dc3cc4b37ca9b0a9f8cfa3abc353e0cd46bf558ce8848fd4d34aad020b099)

---

## 2. Správa požadavků (Requirements Engineering)

### Hlavní činnosti
- **Elicitation (získávání požadavků)** – rozhovory, workshopy, brainstorming, dotazníky.  
- **Specifikace požadavků** – zápis formou dokumentace, uživatelských příběhů nebo UML.  
- **Validace** – ověření, že požadavky odpovídají potřebám uživatelů.  
- **Management požadavků** – sledování změn a verzí během celého projektu.  

### Typy požadavků
- **Funkční** – popisují funkce systému („systém umožní uživateli registrovat se“).  
- **Nefunkční** – výkonnost, spolehlivost, bezpečnost, použitelnost.  

---

## 3. Modelování softwaru

Modelování slouží k vizualizaci, specifikaci a dokumentaci softwarového systému.  

### Účely modelování
- **Komunikace** mezi členy týmu.  
- **Abstrakce** – zjednodušené zobrazení složité reality.  
- **Analýza a návrh** – podpora rozhodování.  
- **Dokumentace** – dlouhodobá udržitelnost projektu.  

Modely se vytvářejí často pomocí **UML (Unified Modeling Language)**.

---

## 4. UML (Unified Modeling Language)

UML je **standardizovaný jazyk pro modelování softwaru**.  
Slouží k vizualizaci a dokumentaci systémů pomocí diagramů.  

Rozděluje se na dva základní typy diagramů:
- **Strukturální diagramy** – statické prvky systému (třídy, komponenty, objekty).  
- **Behaviorální diagramy** – dynamické chování systému (scénáře, interakce, procesy).  

---

## 5. Typy UML diagramů

### Strukturální diagramy
- **Diagram tříd (Class Diagram)** – popisuje třídy, jejich atributy, metody a vztahy.  
- **Diagram objektů (Object Diagram)** – konkrétní instance tříd.  
- **Diagram komponent (Component Diagram)** – fyzické komponenty systému.  
- **Diagram nasazení (Deployment Diagram)** – architektura nasazení na hardware.  
- **Package Diagram** – balíčky a jejich vztahy.  

### Behaviorální diagramy
- **Use Case Diagram (Diagram případů užití)** – funkce systému z pohledu uživatele.  
- **Sekvenční diagram (Sequence Diagram)** – časová posloupnost interakcí mezi objekty.  
- **Komunikační diagram (Communication Diagram)** – interakce mezi objekty formou sítě.  
- **Stavový diagram (State Machine Diagram)** – změny stavů objektu na základě událostí.  
- **Aktivitní diagram (Activity Diagram)** – procesy, pracovní postupy.  
- **Časový diagram (Timing Diagram)** – časové závislosti mezi objekty.  

---

## 6. Význam UML v praxi
- **Analytici** používají UML pro modelování požadavků.  
- **Návrháři** tvoří diagramy tříd a komponent.  
- **Vývojáři** využívají sekvenční a aktivitní diagramy pro implementaci.  
- **Testeři** využívají use case diagramy jako podklad pro testovací scénáře.  

![UML overview](https://upload.wikimedia.org/wikipedia/commons/d/d5/UML_diagrams_overview.svg)

---

## Shrnutí
- **Životní cyklus softwaru** definuje fáze od požadavků po údržbu.  
- **Správa požadavků** zajišťuje, že systém odpovídá potřebám uživatele.  
- **Modelování softwaru** a **UML** pomáhají vizualizovat systém.  
- UML nabízí **strukturální i behaviorální diagramy**, které slouží různým rolím v projektu.  

---
