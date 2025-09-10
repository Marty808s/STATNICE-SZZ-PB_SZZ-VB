if [[ $1 = "?" ]]
then
echo "Tvoje mama"

else

for i in $(<$1)
do

ping -c 4 $i >> results.html

done

fi
