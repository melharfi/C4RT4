<?php
$server="localhost";
$Auser="root";
$Apass="";
$bdd="C4RT4_bdd";
////////// connexcion a la bdd\\\\\\
$db = mysql_connect($server,$Auser,$Apass);
mysql_select_db($bdd,$db);
?>