if [[ $1 == "?" ]]
then
echo "Tohle je napoveda skriptu.."
fi

start=0
end=255
name=$1
counter=0

for ((i=start;i<=end;i++))
do
echo "$name - $1$i"
if ping -c 1 "$name$i" &>/dev/null
then
((counter++))
echo "$name$i - OK" >> ip_list.txt
else
echo "$name$i - nezapisuji"
fi
echo "Počet: $counter"
done
