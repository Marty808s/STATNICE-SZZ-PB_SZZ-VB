if [[ $1 = "?" ]]
then
	echo "Tento skript prohledá celý disk a přesune všechny MP3 do složky /home/hudba"
else
	find / -type f -name "*.mp3" > tempfile.txt 2> /dev/null
	while read i; do

	mv $i "/home/jv/Hudba/"
	done<tempfile.txt
fi
