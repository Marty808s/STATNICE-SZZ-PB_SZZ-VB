### Výpisky z předmětu **Operační systémy (UJEP, 2016)**

---

## 1. Operační systémy a jejich struktura
- **Definice**: OS = programové vybavení pro správu prostředků výpočetního systému a jejich efektivní přidělování uživatelským programům.
- **Hlavní prostředky**: CPU, paměť, souborový systém, V/V zařízení, procesy.
- **Jádro OS (kernel)**: zajišťuje přidělování CPU a paměti, přístup k V/V zařízením, meziprocesovou komunikaci (IPC).
- **Unix a Linux**:
  - Unix (1970s, Bell Labs) – hierarchický souborový systém, multitasking, více uživatelů.
  - Přepsán do jazyka C → přenositelnost.
  - Dvě větve: BSD (Berkeley) a komerční (AT&T – System V).
  - Linux (1991, Linus Torvalds) – jádro inspirované Unixem, součást GNU ekosystému.
  - GNU/Linux = jádro Linux + GNU nástroje (bash, gcc).
- **Standardizace**: POSIX, X/Open.
- **Dokumentace v Unixu**:
  - Man pages (sekce 1–8: příkazy, systémová volání, knihovny, zařízení, formáty, hry, různé, administrace).
  - Info dokumentace.
  - /usr/share/doc – dokumentace aplikací.

---

## 2. Shell
- **Shell = interpret příkazů** (základní rozhraní OS).
- Typy shellů:
  - Bourne shell (sh) – základní, skriptování.
  - C-shell (csh) – syntaxe podobná C, historie, editace.
  - Bash (Bourne Again Shell) – GNU, nejrozšířenější.
- **Základní práce se shellem**:
  - Prompt → uživatel zadá příkaz → shell jej provede.
  - Syntaxe: `program [volby] parametry`.
  - Volby: krátké (-a), dlouhé (--all), kombinace.
- **Nahrazování (expanze)**:
  - Žolíky: `*`, `?`, `[a-z]`, `[^0-9]`.
  - Složené závorky: `{a,b}` → rozšíření.
  - Tilda `~` → domovský adresář.
  - Citování (ochrana před expanzí): apostrofy `'...'`, uvozovky `"..."`, zpětné lomítko `\`.
- **Historie**:
  - Ukládá se do `~/.bash_history`.
  - Navigace: šipky, Ctrl+R (vyhledávání).
  - Nahrazování: `!!` (poslední příkaz), `!číslo`, `!řetězec`.
- **Terminál**:
  - Virtuální (TTY), fyzické již téměř nepoužívány.
  - Zkratky: Ctrl+C (SIGINT), Ctrl+Z (suspend), Ctrl+D (EOF).

---

## 3. Souborový systém
- **Hierarchie**: strom s kořenem `/`.
- **Typy souborů**: regulární, adresáře, speciální (bloková, znaková zařízení), FIFO, sockety, symbolické odkazy.
- **Cesty**:
  - Absolutní: od `/`.
  - Relativní: od aktuálního adresáře.
- **Příkazy**: `ls`, `cd`, `pwd`, `cp`, `mv`, `rm`, `mkdir`, `rmdir`.
- **Odkazy**:
  - Pevné (hard link) – stejné inode.
  - Symbolické (symlink) – ukazatel na cestu.
- **Práva**:
  - Tři skupiny: uživatel, skupina, ostatní.
  - Typická notace: `rwxr-x---`.
  - Správa: `chmod`, `chown`, `chgrp`.
- **Hledání**: `find`, `locate`, `which`.

---

## 4. Uživatelé a skupiny
- **Root** – superuživatel (UID 0).
- **Běžní uživatelé** – omezená práva.
- **Domovský adresář** – `~`.
- **Uživatelé a procesy** – každý proces má UID vlastníka.
- **Skupiny** – organizace práv, více uživatelů může být ve skupině.
- **Příkazy**: `id`, `who`, `groups`, `passwd`.

---

## 5. Procesy a jejich řízení
- **Proces** = běžící program.
- **Příkaz `ps`** – výpis procesů.
- **Sledování procesů** – `top`, `htop`.
- **Řízení procesů**:
  - Na popředí / pozadí: `&`, `fg`, `bg`.
  - Signály: `kill -9 PID` (SIGKILL), `kill -15 PID` (SIGTERM).
  - `jobs` – úlohy.

---

## 6. Zpracování textů
- **Přesměrování**:
  - `>` – přesměrování výstupu.
  - `<` – přesměrování vstupu.
  - `>>` – append.
- **Roury (pipes)**: `|` – propojení příkazů.
- **Filtry**: `cat`, `more`, `less`, `head`, `tail`, `sort`, `uniq`, `wc`, `grep`, `cut`, `tr`.
- **Regulární výrazy**:
  - `.` libovolný znak, `*` opakování, `^` začátek, `$` konec.
  - Použití v `grep`, `sed`, `awk`.
- **Sed** – streamový editor.
- **Awk** – textový procesor pro sloupce.

---

## 7. Skripty
- **Proměnné**: `VAR=hodnota`, přístup `$VAR`.
- **Spouštění skriptů**: `bash skript.sh`, `./skript.sh` (s právem spustitelnosti).
- **Parametry**: `$1`, `$2` …, `$#` (počet), `$@` (všechny).
- **Podmínky**: `if [ podmínka ]; then ... fi`.
- **Cykly**: `for`, `while`, `until`.
- **Ukázky**: kopírování s výpisem, hledání slov, hlídání procesů.

---

## 8. Služby OS
- **Jobs** – správa procesů na pozadí.
- **Démoni** – běžící procesy poskytující služby (např. cron, sshd).

---

## 9. Síťové služby
- **SSH** – vzdálený přístup.
- **Web server** – Apache, Nginx.

---

## 10. Sdílené souborové systémy
- NFS, SMB (Windows sdílení), CODA.

---

## 11. Instalace software
- Správci balíčků:
  - Debian/Ubuntu: `apt-get`, `dpkg`.
  - RedHat/Fedora: `yum`, `rpm`.

---

## 12. Zálohování
- Archivace: `tar`.
- Komprese: `gzip`, `bzip2`, `xz`.
- Kopie: `rsync`, `scp`.

---

👉 Tyto výpisky pokrývají hlavní obsah skript a slouží jako **dlouhá verze** pro pochopení i dohledání detailů.

