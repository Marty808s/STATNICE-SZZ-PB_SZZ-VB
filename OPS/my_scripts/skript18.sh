if [[ $1 = "?" ]]
then
echo "Tohle je super nápověda"

else

find / -name "*.tmp" > tmp_detect.txt
count=0
for path in $(<tmp_detect.txt)
do
((count++))
rm "$path"
echo "Smazán: $path / $count"
done
echo "Smazáné celkem $count..."
rm tmp_detect.txt
fi
