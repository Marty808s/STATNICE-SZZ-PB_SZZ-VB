args=($@)
adresar=$1
nazev=$2
shift 2
pripony=$@

echo $pripony

counter=0

sum=0

function calcSize() {
	sum=(sum+$@)
}

# nový soubor
> "$nazev.csv"

for i in $@
do
((counter++))
echo "Hledám příponu: $i | iterace: $counter | do adresáře $adresar"
find "$adresar" -type f -name "*.$i" -printf "%s | %p | %i\n"  >> "$nazev.csv"
done

cat "$nazev.csv" | sort -n -r -k1 > "nazev2.csv"
find "$adresar" -printf "%s\n" | awk '{sum+=$1} END {print sum}'

