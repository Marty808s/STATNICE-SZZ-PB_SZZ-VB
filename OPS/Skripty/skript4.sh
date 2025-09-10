if [[ $1 = "?" ]]
then
echo "Tvoje mama"

else

mkdir ../hudba

find / -type f -name "*.mp3" > mp3_route.txt 2>/dev/null

for i in $(<mp3_route.txt)
do

mv -f $i ../hudba

done

fi
