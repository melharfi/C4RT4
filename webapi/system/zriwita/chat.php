<?php
include("../bddcordon.php");
if($_SERVER['HTTP_USER_AGENT']=="NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk" || 1==1)
{
	if(isset($_GET['chat']) && $_GET['chat']==1 && isset($_GET['msg']) && !empty($_GET['msg']) && isset($_GET['partie']) && !empty($_GET['partie']) && isset($_GET['id_app']) && !empty($_GET['id_app']))
	{
	// recherche de la session
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die("Error 103-0");
		$query1=mysql_fetch_array($req1) or die ("Error 103-1");
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select count(id) from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
			$query2=mysql_fetch_array($req2) or die(mysql_error());
			
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
							$req4=mysql_query("select chat,chat_cnt from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
							$query4=mysql_fetch_array($req4) or die(mysql_error());
							
							$req5=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
							$query5=mysql_fetch_array($req5) or die(mysql_error());
							
							$char_not_allowed=array("#","/",":");
							$msg=str_replace($char_not_allowed,"!",$_GET['msg']);
							
							$chat_cnt=($query4['chat_cnt']*1)+1;
							$chat=$chat_cnt.":".$query5['user'].":".$msg;
							//echo $chat;
	
							if($query4['chat_cnt']==0)
							{
								mysql_query("update parties_app_zriwita set chat='".$chat."', chat_cnt='".$chat_cnt."' where nom_table='".$_GET['partie']."'") or die(mysql_error());
							}
							else
							{
								mysql_query("update parties_app_zriwita set chat='".$query4['chat']."/".$chat."', chat_cnt='".$chat_cnt."' where nom_table='".$_GET['partie']."'") or die(mysql_error());
							}
							
							echo "C4RT4#chat#msg sent";
						}
						else
							echo "C4RT4#fatal error#not allowed";
				}
				else
					echo "aucun client trouvé";
			}
			else
				echo "C4RT4#error#action not allowed";
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else if(isset($_GET['get_msg']) && $_GET['get_msg']==1 && isset($_GET['id_app']) && !empty($_GET['id_app']) && isset($_GET['partie']) && !empty($_GET['partie']))
	{
	// recherche de la session
		$req1=mysql_query("select count(id) from signed_app where id_app='".$_GET['id_app']."'") or die("Error 103-0");
		$query1=mysql_fetch_array($req1) or die ("Error 103-1");
		
		if($query1[0]>0)
		{
			$req2=mysql_query("select count(id) from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
			$query2=mysql_fetch_array($req2) or die(mysql_error());
			
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
							$req4=mysql_query("select chat,chat_cnt from parties_app_zriwita where nom_table='".$_GET['partie']."'") or die(mysql_error());
							$query4=mysql_fetch_array($req4) or die(mysql_error());
							
							//$req5=mysql_query("select user from signed_app where id_app='".$_GET['id_app']."'") or die(mysql_error());
							//$query5=mysql_fetch_array($req5) or die(mysql_error());
							
							//$char_not_allowed=array("#","/",":");
							//$msg=str_replace($char_not_allowed,"!",$_GET['msg']);
							
							//$chat_cnt=($query4['chat_cnt']*1)+1;
							//$chat=$chat_cnt.":".$query5['user'].":".$msg;
							//echo $chat;
	
							if($query4['chat_cnt']==0)
								echo "C4RT4#chat#nothing";
							else
								echo "C4RT4#chat#msg#".$query4['chat']."/".$query4['chat_cnt'];
						}
						else
							echo "C4RT4#fatal error#not allowed";
				}
				else
					echo "aucun client trouvé";
			}
			else
				echo "C4RT4#error#action not allowed";
		}
		else
			echo "C4RT4#fatal error#not connected";
	}
	else
		echo "missed";
}
?>