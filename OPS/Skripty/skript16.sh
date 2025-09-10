if [[ $1 = "?" ]]
then
echo "Tvoje mama"

else

path="$1"

IFS=';'

while read jmeno heslo skupina
do

sudo useradd -m -g $skupina -p $heslo $jmeno

done <"$path"

fi
