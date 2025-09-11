if [[ $1 = "?" ]]
then
echo "Návod"
fi

read -p "Pocet uzivatelu k pridani" pocet
echo "$pocet"

for ((i=1;i<=pocet;i++))
do

echo "K přidání: $pocet"

read -p "Jmeno: " jmeno
read -p "Skupina: " group
read -p "Heslo: " passwd

sudo useradd -m -g $group -p $passwd $jmeno

done


