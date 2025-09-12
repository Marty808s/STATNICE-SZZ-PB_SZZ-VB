if [[ $1 = '?' ]]
then
	echo "Tento skript slouží k ověření konektivity na stránky uvedené v souboru servers.txt"
else
	file="servers.txt"

	while read -r line; do
		ping $line -c 4 >> result.html
	done<$file
fi
