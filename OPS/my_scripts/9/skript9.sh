if [[ $1 = "?" ]]
then
	echo "Tento skript udělá zálohu uživatele vloženého jako parametr."
else
	user="$1"
	home="/home/$user"
	archive="$user.tar"

	if [[ ! -d "$home" ]]
	then
		echo "Uživatel '$1' neexistuje, zkontrolujte zadanou hodnotu"
		exit 1
	fi
	sudo tar -cvf "$archive" "$home"
	echo "Uloženo do $archive"
fi
