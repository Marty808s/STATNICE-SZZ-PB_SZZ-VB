if [[ $1 = "?" ]]
then

echo "Tvoje mama"

else

find /home -type f -name "*.jpg" > skript5_route.txt 2>/dev/null
find /home -type f -name "*.png" >> skript5_route.txt 2>/dev/null
find /home -type f -name "*.gif" >> skript5_route.txt 2>/dev/null

counter=0

for i in $(<skript5_route.txt)
do

rm -r $i 2>/dev/null
((counter++))

done
echo $counter

fi
