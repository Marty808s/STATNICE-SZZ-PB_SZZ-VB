if [[ $1 = "?" ]]
then
echo "Tvoje mama"

else

while true
do

date=$(date)
ping=$(ping -c 1 www.seznam.cz)

echo "$ping $date" >>sh8_ping.txt

sleep 5

done &

fi
