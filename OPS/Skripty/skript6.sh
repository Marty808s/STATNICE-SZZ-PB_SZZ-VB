if [[ $1 = "?" ]]
then

echo "Tvoje máma"

else

start=1
end=$1

for i in $(eval echo "{$start..$end}") #working
#for ((i=start;i<=end;i++)) #working
do

operator=$((RANDOM % 2))

num1=$((RANDOM))
num2=$((RANDOM))

case $operator in 
0)
 operator="+"
 ;;

1)
 operator="-"
 ;;

esac

echo "$num1 $operator $num2" >> test_matika.txt

done

fi
