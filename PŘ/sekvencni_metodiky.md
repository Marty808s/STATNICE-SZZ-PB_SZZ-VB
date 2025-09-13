# Sekvenční metodiky projektového řízení

Sekvenční (tradiční, prediktivní) metodiky jsou charakteristické **lineárním průběhem** – jednotlivé fáze projektu na sebe navazují a obvykle se nevrací zpět.  
Jsou vhodné pro projekty s **jasně definovanými cíli a stabilními požadavky**.

---

## 1. Waterfall (vodopádový model)
Vodopádový model je **nejklasičtější sekvenční metodika**.

### Fáze
1. **Požadavky** – analýza potřeb zákazníka.  
2. **Návrh** – architektura a design řešení.  
3. **Implementace** – samotný vývoj.  
4. **Testování** – kontrola kvality.  
5. **Nasazení** – předání zákazníkovi.  
6. **Údržba** – provoz a opravy.  

![Waterfall Model](https://upload.wikimedia.org/wikipedia/commons/e/e2/Waterfall_model.svg)

### Výhody
- Jednoduchost a přehlednost.  
- Jasná dokumentace.  
- Vhodné pro projekty s pevnými požadavky.  

### Nevýhody
- Nízká flexibilita vůči změnám.  
- Riziko odhalení chyb až na konci projektu.  

---

## 2. V-Model (Verification and Validation Model)
V-Model je rozšíření vodopádového modelu. Klade důraz na **paralelní testování** k jednotlivým fázím vývoje.

### Struktura
- Levá část „V“ – fáze návrhu (analýza, design).  
- Pravá část „V“ – fáze testování (jednotkové, integrační, systémové, akceptační testy).  
- Spojovací bod – implementace.  

![V-Model](https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fapi.reliasoftware.com%2Fuploads%2Fwhat_is_v_model_5679b57a14.webp&f=1&nofb=1&ipt=3dc4c4a4da4102c33512033237692a0c0f6ca6f681b52886f9c9484624bc8c74)

### Výhody
- Důraz na kvalitu a testování.  
- Chyby odhaleny dříve než ve vodopádovém modelu.  

### Nevýhody
- Stále málo flexibilní vůči změnám.  
- Vyžaduje detailní specifikaci požadavků předem.  

---

## 3. Spiral Model (Spirálový model)
Spirálový model kombinuje prvky **vodopádového modelu** a **prototypování**.  
Vývoj probíhá ve **spirálových cyklech**, kde se opakují fáze: plánování, analýza rizik, vývoj, testování.

![Spiral Model](https://upload.wikimedia.org/wikipedia/commons/e/ec/Spiral_model_%28Boehm%2C_1988%29.svg)

### Charakteristika
- Každá smyčka spirály odpovídá jedné fázi projektu.  
- Velký důraz na **řízení rizik**.  
- Kombinuje sekvenční a iterativní přístup.  

### Výhody
- Flexibilnější než vodopád.  
- Vhodné pro velké a rizikové projekty.  

### Nevýhody
- Složitější řízení.  
- Vyšší náklady.  

---

## 4. Iterativní a inkrementální model
Tento model kombinuje **postupný vývoj po částech (inkrementy)** s **opakovanými iteracemi**.  
Každá verze systému je použitelná a rozšiřuje předchozí.  

![Iterative Incremental](https://upload.wikimedia.org/wikipedia/commons/5/5f/Iterative_development_model_-_en.png)

### Výhody
- Průběžné výsledky.  
- Možnost časté kontroly.  

### Nevýhody
- Vyžaduje dobrou koordinaci.  
- Nehodí se pro velmi krátké projekty.  

---

## Shrnutí
- **Waterfall** – lineární, jednoduchý, ale málo flexibilní.  
- **V-Model** – přidává systematické testování ke každé fázi.  
- **Spirálový model** – vhodný pro projekty s velkým rizikem.  
- **Iterativní/inkrementální model** – postupné doručování, kompromis mezi sekvenčním a agilním přístupem.  
