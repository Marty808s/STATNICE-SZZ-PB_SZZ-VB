find / -name  "*.png" > temp.txt
find / -name "*.jpg" >> temp.txt
find / -name "*.gif" >> temp.txt

zip -r -e vystup20.zip temp.txt
