if [[ $1 = "?" ]]
then
	echo "Tento skript testuje konektivitu se serverem www.seznam.cz v intervalu 60s."
	echo "Výsledky najdete v test.txt."
fi

echo "Spouštím testování konektivity na pozadí"

(
	while true; do
		cas=$(date +"%Y-%m-%d %H:%M:%S")
		if ping -c 1 -w 1 www.seznam.cz >> /dev/null
		then
			echo "$cas - OK" >> test.txt
		else
			echo "$cas - Chyba" >> test.txt
		fi
		sleep 60
	done
) &
