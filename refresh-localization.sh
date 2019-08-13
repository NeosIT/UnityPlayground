languages=("de")

# setup
mkdir -p Localization

# get files
find Assets/_INTERNAL_/Scripts/ -name "*.cs" > files.txt

for element in ${languages[*]}
do
  path="Assets/_INTERNAL_/Resources/Localization/$element.po"
  touch $path
  xgettext -o $path --from-code=UTF-8 -f files.txt -k_ -j
done

# cleanup
rm -f files.txt