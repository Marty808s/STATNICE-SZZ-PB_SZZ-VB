if [[ $1 = "?" ]]
then

echo "Tvoje mama"


else
count=0
fail=0

for ((i=0;i<=3;i++))
do

ping -c 1 192.168.5."$i" &>/dev/null

if [[ $? -eq 0 ]]

 then
  ((count++))

 else
  ((fail++))

 fi

done

echo "Connected: $count"
echo "Fail $fail"

fi

