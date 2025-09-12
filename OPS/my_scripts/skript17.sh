if [[ $1 == "?" ]]
then
echo "Tento skript vytváží uživatele pomocí txt -> name/pass/group_name"
fi

while read -r line
do
echo "Jsem na žádku $line"
IFS="/" read -r name pass group <<< "$line"

echo "$name / $pass / $group"
sudo useradd -m -g "$group" -p "$pass" "$name"

done < "$1"

