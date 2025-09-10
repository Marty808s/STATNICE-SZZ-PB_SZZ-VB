if [[ $1 = "?" ]]
then
echo "Tvoje mama"

else

find ./txt_2_connect -type f -name "*.txt" > skript7_connect.txt

for i in $(<skript7_connect.txt)
do

cat $i >> ./txt_2_connect/sk7_vysledek.txt

done

fi

