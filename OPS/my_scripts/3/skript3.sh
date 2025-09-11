if [[ $1 = "?" ]]
then
	echo "Tento skript smaže veškeré .tmp soubory ze systému. Po smazání napíše počet smazaných souborů";
else

	find / -type f -name "*.tmp" > tempresult.txt 2>/dev/null;

	ctr=0
	while read i; do
		rm -f $i
		ctr=$((ctr+1))
	done<"tempresult.txt"
	echo "Smazáno "${ctr}"  souborů";
	#rm "tempresult.txt";
fi
