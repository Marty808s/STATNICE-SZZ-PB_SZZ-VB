if [[ $1 = "?" ]]
then
	echo "Tento skript generuje náhodné matematické příklady s operatory + - / *"
	echo "Jako parametr použijte počet příkladů. "
else
pocet_prikladu="$1"
operatory=("+" "-" "*" "/")

> test_matika_zadani.txt

echo "Generuju $pocet_prikladu příkladů."

for i in $(seq 1 $pocet_prikladu); do
	num1=$((RANDOM % 100))
	num2=$((1+RANDOM % 100))

	operator=$((RANDOM%4)) 
	case $operator in
	0)
	operator="+"
	;;
	1)
	operator="-"
	;;
	2)
	operator="*"
	;;
	3)
	operator="/"
	;;
	esac

	echo "$num1 $operator $num2" >> test_matika_zadani.txt
done
fi
