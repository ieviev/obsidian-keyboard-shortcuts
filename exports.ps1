# export commonjs module for obsidian
param ($param1)
$export = "module.exports = Plugin2;"
write-output $export | out-file -append -encoding utf8 $param1

