if [[ $1 = "?" ]]

then
echo "Tvoje mama"

else

read -p "Pocet uzivatelu k pridani: " pocet
echo "$pocet"

for ((i=1;i<=pocet;i++))
do

read -p "Jmeno uzivatelu k pridani: " jmeno
read -p "Skupina uzivatele: " group
read -p "Heslo uzivatele: " passwd

sudo useradd -m -g $group -p $passwd $jmeno #sem sudo pokud nebudu muset byt root

#kdyz nejsem v sudo group, tak su -c "./path"
#group - addgroup/groupadd viz man (-gid..... nazev)

done

fi
