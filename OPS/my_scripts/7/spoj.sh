if [[ $1 = "?" ]]
then
	echo "Tento skript spojí všechy txt soubory v zadaném adresáři do jednoho souboru"

else
	adresar="$1"

	if [[ ! -d "$adresar" ]]
	then
		echo "Zadaná cesta není platný adresář"
	fi

	cd "$adresar" || echo "Zadaný adresář nelze otevřít"

	echo "Spojím soubory z '$adresar'."

	> vysledek.txt

	find "$adresar" -type f -name "*txt" -print0 | sort -z | xargs -0 cat >> vysledek.txt
fi
