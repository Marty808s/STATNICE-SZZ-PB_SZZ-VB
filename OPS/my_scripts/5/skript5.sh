if [[ $1 = "?" ]]
then
	echo "Tento skript najde všechny .png .jpg .gif soubory na disku a vytvoří z nich zaheslovaný archiv."
fi
list=("*.jpg" "*.png" "*.gif")
# @ zajišťuje iteraci skrze list
for i in "${list[@]}"; do
	echo "Jsem for $i"
	find / -type f -name "$i" >> "tempfile.txt"
done
# pomocí @ načítáme každý řádek z txt souboru
zip -e obrazky.zip -@ < tempfile.txt
