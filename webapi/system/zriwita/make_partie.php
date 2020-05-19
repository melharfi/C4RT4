<?php
// page system de connexion
if($_SERVER['HTTP_USER_AGENT']=="NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk")
{
	include("../bddcordon.php");
	
	if(isset($_GET['make']) && isset($_GET['id_app']) && isset($_GET['players']) && isset($_GET['evalue']) && isset($_GET['extra1']) && isset($_GET['extra2']) && isset($_GET['extra3']) && isset($_GET['extra4']) && !empty($_GET['make']) && !empty($_GET['id_app'])&& !empty($_GET['players'])&& !empty($_GET['evalue'])&& !empty($_GET['extra1'])&& !empty($_GET['extra2'])&& !empty($_GET['extra3'])&& !empty($_GET['extra4']))
	{
		// recherche de la session
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die("Error 103-0");
		$query1=mysql_fetch_array($req1) or die ("Error 103-1");
		
		if($query1[0]>0)
		{
			// recherche de la session
			$req2=mysql_query("select time,user from signed_app where id_app='".$_GET['id_app']."'") or die ("Error 106-1");
			$query2=mysql_fetch_array($req2) or die("Error 106-2");
			
			// CREATION D'UN NOM aléatoire pour la table
			$taille = 4;
			$lettres = "abcdefghijklmnopqrstuvwxyz0123456789";
			$id="";
			
			for ($i=0;$i<$taille;$i++)
				$id.=substr($lettres,(rand()%(strlen($lettres))),1);
			
			////////// recherche d'un nom de table valide
			//$cnt=1;
			while(true)
			{
				$req4=mysql_query("select count(id) from parties_app_zriwita where nom_table='table_".$id."'") or die(mysql_error());
				$query4=mysql_fetch_array($req4) or die(mysql_error());
				
				if($query4[0]==0)
					break;
				else
				{
					$id="";
					for ($i=0;$i<$taille;$i++)
						$id.=substr($lettres,(rand()%(strlen($lettres))),1);
					//$cnt++;
				}
			}
			
			$req5=mysql_query("select points from users where user='".$query2['user']."'") or die(mysql_error());
			$query5=mysql_fetch_array($req5) or die(mysql_error());
			
			mysql_query("insert into parties_app_zriwita(nom_table,owner,evalue,nbr_players,ip_host,clients,extra) values('table_".$id."','".$query2['user']."','".$_GET['evalue']."','".$_GET['players']."','".$_SERVER['REMOTE_ADDR']."','".$query2['user'].":".$_GET['id_app'].":".$query5['points']."','".$_GET['extra1'].'-'.$_GET['extra2'].'-'.$_GET['extra3'].'-'.$_GET['extra4']."')") or die (mysql_error());
			echo "C4RT4#party#done#table_".$id;
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else if(isset($_GET['destroy']) && is_numeric($_GET['destroy']) && isset($_GET['id_app']) && !empty($_GET['id_app']))
	{
		// recherche de la session
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die("Error 103-0");
		$query1=mysql_fetch_array($req1) or die ("Error 103-1");
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die (mysql_error());
			$query2=mysql_fetch_array($req2) or die(mysql_error());
			
			$req3=mysql_query("select count(id) from parties_app_zriwita where owner='".$query2['user']."' and etat='En attente'") or die(mysql_error());
			$query3=mysql_fetch_array($req3) or die(mysql_error());
			
			if($query3[0]>0)
			{
				mysql_query("delete from parties_app_zriwita where owner='".$query2['user']."' and etat='En attente'") or die(mysql_error());
				echo "C4RT4#party#destroyed";
			}
			else
				echo "C4RT4#error#action not allowed";
		}
		else
			echo "C4RT4#fatal error#not connected";
		
		
	}
	else if(isset($_GET['locked']) && is_numeric($_GET['locked']) && isset($_GET['id_app']) && !empty($_GET['id_app']))
	{
		// recherche de la session
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
		$query1=mysql_fetch_array($req1) or die (mysql_error());
		
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die (mysql_error());
			$query2=mysql_fetch_array($req2) or die(mysql_error());
			
			$req3=mysql_query("select count(id) from parties_app_zriwita where owner='".$query2['user']."' and etat='En attente'") or die (mysql_error());
			$query3=mysql_fetch_array($req3) or die (mysql_error());
			
			if($query3[0]>0)
			{
				if($_GET['locked']==1)
				{
					mysql_query("update parties_app_zriwita set locked=1 where owner='".$query2['user']."'") or die (mysql_error());
					echo "C4RT4#host party#locked";
				}
				else
				{
					mysql_query("update parties_app_zriwita set locked=0 where owner='".$query2['user']."'") or die (mysql_error());
					echo "C4RT4#host party#unlocked";
				}
			}
			else
				echo "C4RT4#error#action not allowed";
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else
		echo "missed 2";
}
?>