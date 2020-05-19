<?php
// page system de connexion
if($_SERVER['HTTP_USER_AGENT']=="NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk")
{
	include("bddcordon.php");

	if(isset($_GET['signin']) && isset($_GET['login']) && !empty($_GET['login']) && isset($_GET['pwd']) && !empty($_GET['pwd']))
	{
		$req1=mysql_query("select count(id) from users where user='".$_GET['login']."' && password='".$_GET['pwd']."'") or die("Error 100-0");
		$query1=mysql_fetch_array($req1) or die("Error 100-1");
		
	
		if($query1[0]>0)
		{
			// extraction des infos user
			$req2=mysql_query("select id,nom,user,points,nbr_parties from users where user='$_GET[login]'") or die("Error 101-0");
			$query2=mysql_fetch_array($req2) or die ("Error 101-1");
			
			// recherche un doublon de session
			$req4=mysql_query("select count(id) from signed_app where real_id='$query2[id]'") or die("Error 103-0");
			$query4=mysql_fetch_array($req4) or die ("Error 103-1");
			
			if($query4[0]>0)
				echo "C4RT4#error#user already signed";
			else
			{
			// CREATION D'UN IDENTIFIANT ALEATOIRE
			$taille = 40;
			$lettres = "abcdefghijklmnopqrstuvwxyz0123456789";
			$id="";
			
			for ($i=0;$i<$taille;$i++)
				$id.=substr($lettres,(rand()%(strlen($lettres))),1);
			
			$req3=mysql_query("select count(id) from signed_app where id_app='$id'") or die("Error 102-0");
			$query3=mysql_fetch_array($req3) or die ("Error 102-1");
			
			while($query3[0]>0)
			{
				$id="";
				
				for ($i=0;$i<$taille;$i++)
					$id.=substr($lettres,(rand()%(strlen($lettres))),1);
				
				$req3=mysql_query("select count(id) from signed_app where id_app='$id'") or die("Error 102-0");
				$query3=mysql_fetch_array($req3) or die ("Error 102-1");
			}
			
			// Création d'une session APP
			mysql_query("insert into signed_app(id_app,real_id,user,time,ip) values('".$id."','".$query2['id']."','".$query2['user']."','".time()."','".$_SERVER['REMOTE_ADDR']."')") or die(mysql_error());
			// envoie de données à C4RT4
			echo "C4RT4#welcome#".$id."#".$query2['nom']."#".$query2['user']."#".$query2['points']."#".$query2['nbr_parties'];
			}
		}
		else
			echo "C4RT4#error#failed to log";
	}
	elseif(isset($_GET['signout']) && isset($_GET['id_app']) && !empty($_GET['id_app']))
	{
		// recherche de la session de l'user
		$req4=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die("Error 104-0");
		$query4=mysql_fetch_array($req4) or die ("Error 104-1");
		
		$req5=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die("Error 105-0"); 
		$query5=mysql_fetch_array($req5) or die ("Error 105-1");
		
		if($query4[0]>0)
		{
			mysql_query("delete from signed_app where id_app='".$_GET['id_app']."'") or die("Error 105-2");
			mysql_query("delete from parties_app_zriwita where owner='".$query5['user']."' and etat='En attente'") or die("Error 105-3");
			//////// ajouter un champ delet pour chaque jeux
			echo "C4RT4#signed out done";
		}
		else
			echo "C4RT4#error#user already signed out";
	}
	else if(isset($_GET['syn']) && isset($_GET['id_app']) && is_numeric($_GET['syn']) && !empty($_GET['id_app']))
	{
		$req=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die ("Error 106-0");
		$que=mysql_fetch_array($req) or die("Error 106-1");
		
		if($que[0]>0)
		{
			$req=mysql_query("select time from signed_app where id_app='".$_GET['id_app']."'") or die (mysql_error());
			$que=mysql_fetch_array($req) or die("Error 107-1");
			
			$time_left=420;   // 420 = 7 min
			if((time()-$que['time'])>$time_left)
			{
				mysql_query("delete from signed_app where id_app='".$_GET['id_app']."'") or die("Error 105-1");
				echo "C4RT4#error#session lifetimeout";
			}
			else
			{
				mysql_query("update signed_app set time='".time()."' where id_app='".$_GET['id_app']."'") or die("error 106-0");
				echo "C4RT4#session reset done, time:".(time()-$que[0]);
			}
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else
		echo "missed";
}
?>