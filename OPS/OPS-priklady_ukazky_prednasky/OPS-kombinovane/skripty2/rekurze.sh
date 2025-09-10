#!/bin/bash
if [ "$kolik" -gt "0" ]; then
 echo "ahoj svete"
 kolik=$(($kolik-1))
 . ./rekurze.sh
fi

