pocet=$1
jmeno=$2

for ((i=1;i<=pocet;i++))
do
operator=$((RANDOM % 2))
n1=$((RANDOM))
n2=$((RANDOM))

case $operator in
0) operator="+";;
1) operator="-";;
esac

echo "$n1 $operator $n2 = " >> $jmeno

done
