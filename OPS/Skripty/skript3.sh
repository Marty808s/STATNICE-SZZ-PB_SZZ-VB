if [[ $1 = "?" ]]
then
echo "Tvoje mama"

else

find / -type f -name "*.tmp" > tmp_route.txt 2>/dev/null

counter=0

for i in $(<tmp_route.txt)
do

rm -r $i 

((counter++))

done

echo $counter

fi


#while IFS= read -r path 
#do 
#done <tmp_route.txt - bez IFS
