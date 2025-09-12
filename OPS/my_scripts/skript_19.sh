if [[ $1 == "?" ]]

then

echo "Tohle je nápověda.."

else

find / -name "*.mp3" > tmp_list.txt

cil="./home/hudba/"
i=0

while read -r line;
do

i=$((i+1))

echo -e "$line \n počet: $i"
mv $line $cil

done < tmp_list.txt
echo "Hotovo"

fi
