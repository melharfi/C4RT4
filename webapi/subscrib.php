<html>
<head>
<title>Document sans titre</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<?php
//$baned= array(".","/",";",",","?","%","$","=",")","(","'","\"","&","}","]","\\","`","|","[","{","<",">");
/*$allowed_num=array(0,1,2,3,4,5,6,7,8,9);
$allowed_alpha=array("a","z","e","r","t","y","u","i","o","p","q","s","d","f","g","h","j","k","l","m","w","x","c","v","b","n");
$allowed_all=array("a","z","e","r","t","y","u","i","o","p","q","s","d","f","g","h","j","k","l","m","w","x","c","v","b","n",0,1,2,3,4,5,6,7,8,9);*/
include("system/bddcordon.php");

extract($_POST);

//mysql_query("insert into users(nom) values('mohssine')");
$pwd=md5($_POST['pwd']);
mysql_query("INSERT INTO users(nom,user,password,ville,subscribed) VALUES('".$_POST['nom']."','".$_POST['login']."','".$pwd."','".$_POST['ville']."','".gmdate("d/m/y h:i:s")."')") or die (mysql_error());

/*
if(isset($_POST["nom"]) && !empty($_POST["nom"]))
{
	$block=false;
	
	for(int cnt=0;cnt<strlen($_POST["nom"]);cnt++)
	{
		if(strpos($_POST["nom"],$allowed_alpha,cnt)
	}
	
}
else
	echo "Error name";*/
?>
</body>
</html>
