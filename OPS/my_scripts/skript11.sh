if [[ $1 == "?" ]]
then
echo "tohle je nápověda"
fi

source_f=$1
output_f=$2
output=""

while read -r -N 1 char
do
case "$char" in
"y") output+=" ";;
"i") output+=" ";;
*) output+="$char";;
esac
done < "$source_f"

echo $output > $output_f
