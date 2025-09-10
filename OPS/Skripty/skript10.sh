if [[ $1 = "?" ]]
then
  echo "Tvoje mama"

else

path="$1"

while read line
do

ping -c 2 $line >>sk10_result.html

done <"$path"

fi
