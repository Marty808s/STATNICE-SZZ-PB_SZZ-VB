#!/bin/bash
#vnejsi skript, ktery spousti dalsi skript
echo "Spoustim vnitrni skript"
koho="Vesmir"

export koho #exportovani promenne do vnitrnich skriptu

./subscript.sh #spusteni na novem shelu

. ./subscript.sh #spusteni na puvodnim shelu

unset koho 

. ./subscript.sh #spusteni na puvodnim shelu


