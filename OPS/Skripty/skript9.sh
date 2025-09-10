if [[ $1 = "?" ]]; then
  echo "Tvoje mama"

else
  rm -rf ../zaloha 2>/dev/null
  rm -rf zaloha.tar.gz

  mkdir ../zaloha
  cp -r /home/"$1"/* ../zaloha 2>/dev/null

  tar -czf zaloha.tar.gz -C ../zaloha . #tecka znamena, ze mi vytvoři archiv, vcetne s adresarem

  #pro nasledne precteni - tar -tzf zaloha.tar.gz

fi

