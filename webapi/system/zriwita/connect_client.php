<?php
// page system de connexion
if($_SERVER['HTTP_USER_AGENT']=="NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk" or 1==1)
{
include("../bddcordon.php");

	if(isset($_GET['connect']) && isset($_GET['id_app']) && isset($_GET['partie']) && !empty($_GET['connect']) && !empty($_GET['id_app']) && !empty($_GET['partie']))
	{
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
		$query1=mysql_fetch_array($req1) or die (mysql_error());
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select count(id) from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
			$query2=mysql_fetch_array($req2) or die(mysql_error());
			
			if($query2[0]>0)
			{
				$req3=mysql_query("select * from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
				$query3=mysql_fetch_array($req3) or die(mysql_error());
				
				////// controle si la table est verouillé
				if($query3['locked']==0)
				{
					if(!empty($query3['clients']))
					{
						////// controle et conteur de joueurs max 20;
						$nbr_player = explode("/",$query3['clients']);
						if(count($nbr_player)<20)
						{
							$req4=mysql_query("select ip,user from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
							$query4=mysql_fetch_array($req4) or die(mysql_error());
							
							$req_pts=mysql_query("select points from users where user='".$query4['user']."'") or die(mysql_error());
							$que_pts=mysql_fetch_array($req_pts) or die (mysql_error());
							
							$data=$query4['user'].":".$_GET['id_app'].":".$que_pts['points'];
							mysql_query("update parties_app_zriwita set clients='".$query3['clients']."/".$data."' where nom_table='".$_GET['partie']."'") or die(mysql_error());
							echo "C4RT4#client#added#".$query3['extra']."#".$query3['evalue']."#".$_GET['partie']."#".$query3['nbr_players'];
						}
						else
							echo "C4RT4#client party#full";
					}
					else
					{
						$req4=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
						$query4=mysql_fetch_array($req4) or die(mysql_error());
						
						$req_pts=mysql_query("select points from users where user='".$query4['user']."'") or die(mysql_error());
						$que_pts=mysql_fetch_array($req_pts) or die (mysql_error());
						
						$data=$query4['user']."/".$_GET['id_app'];
						mysql_query("update parties_app_zriwita set clients='".$data."' where nom_table='".$_GET['partie']."'") or die(mysql_error());
						echo "C4RT4#client#added#".$query3['extra']."#".$query3['evalue']."#".$_GET['partie']."#".$query3['nbr_players'];
					}
				}
				else
					echo "C4RT4#client party#locked";
			}
			else
				echo "C4RT4#client party#not found";
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else if(isset($_GET['get_players']) && isset($_GET['id_app']) && isset($_GET['partie']) && is_numeric($_GET['get_players']) && !empty($_GET['id_app']) && !empty($_GET['partie']))
	{
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
		$query1=mysql_fetch_array($req1) or die (mysql_error());
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select count(id) from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die (mysql_error());
			$query2=mysql_fetch_array($req2) or die (mysql_error());
			
			if($query2[0]>0)
			{
				$req3=mysql_query("select clients from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
				$query3=mysql_fetch_array($req3) or die(mysql_error());
				
				if(!empty($query3['clients']))
				{
					$hash_client=explode("/",$query3['clients']);
					$found=false;
					
						for($cnt=0;$cnt<count($hash_client);$cnt++)
						{
							$data_client=explode(":",$hash_client[$cnt]);
							if($_GET['id_app']==$data_client[1])
							{
								$found=true;
								break;
							}
						}
						
						if($found==true)
						{
						$data_to_send="C4RT4#client#list users#";
						
							for($cnt=0;$cnt<count($hash_client);$cnt++)
							{
							
								$data_client=explode(":",$hash_client[$cnt]);
								$data_to_send.=$data_client[0].":";
								
								$req4=mysql_query("select * from users where user='".$data_client[0]."'") or die(mysql_error());
								$query4=mysql_fetch_array($req4) or die(mysql_error());
								
								$data_to_send.=$query4['points'];
								
									if($cnt<count($hash_client)-1)
										$data_to_send.="/";
							}
						echo $data_to_send;
						}
						else
							echo "nothing 1";
				}
				else
					echo "nothing 2";
			}
			else
				echo "C4RT4#client party#destroyed";
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else if(isset($_GET['disconnect']) && is_numeric($_GET['disconnect']) && isset($_GET['id_app']) && !empty($_GET['id_app']) && isset($_GET['partie']) && !empty($_GET['partie']))
	{
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
		$query1=mysql_fetch_array($req1) or die (mysql_error());
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die (mysql_error());
			$query2=mysql_fetch_array($req2) or die (mysql_error());
			
			$req3=mysql_query("select clients from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
			$query3=mysql_fetch_array($req3) or die(mysql_error());
			
			if(!empty($query3['clients']))
			{
				$hash_client=explode("/",$query3['clients']);
				$new_data="";
				
					for($cnt=0;$cnt<count($hash_client);$cnt++)
					{
						$data_client=explode(":",$hash_client[$cnt]);
						if($_GET['id_app']==$data_client[1])
						{
							for($cnt2=0;$cnt2<count($hash_client);$cnt2++)
							{
								if($cnt2!=$cnt)
									$new_data.=$hash_client[$cnt2]."/";
							}
							break;
						}
					}
				
				mysql_query("update parties_app_zriwita set clients='".$new_data."' where nom_table='".$_GET['partie']."'") or die (mysql_error());
				echo "C4RT4#client#disconnected";
			}
			else
				echo "nothing 2";
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else
		echo "missed 1";
}
else
	echo "missed 2";
?>