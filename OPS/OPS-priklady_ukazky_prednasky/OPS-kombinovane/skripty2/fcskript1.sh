#!/bin/bash

kdo="VESMIR" #globalni

pozdrav() {
local koho
koho=$1
echo "$kdo rika:"
echo "ahoj $koho"e

}

echo "Zadej jmeno"
read -s -t 10 koho #nacti do promenne nezobrazuj znaky a cekej max 10 sec
pozdrav $koho

