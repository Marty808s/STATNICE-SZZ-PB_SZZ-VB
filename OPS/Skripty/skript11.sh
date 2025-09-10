if [[ "$1" = "?" ]]; then
  echo "Tvoje mama"
else
  path="$1"
  output=""

  rm -r filtered.txt 2>/dev/null

while read -r -N 1 char #N - kvuli zachovani mezer v textu
  do

    case "$char" in "y")

     char=" "
     #echo "$char"
     output+="$char"
       ;;

     "i")
     char=" "
     #echo "$char"
     output+="$char"
       ;;

     [!yt])
     #echo "$char"
     output+="$char"

   esac

  done < "$path"

echo "$output" > filtered.txt

fi

