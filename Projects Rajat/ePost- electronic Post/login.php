<?php
$userid=$_POST['userid2'];
$password=$_POST['pass2'];
$dbc=mysql_connect('www.rajataggarwal.zymichost.com','351871_rajat','thereare');
//$dbc=mysql_connect('localhost','root','');
$sel=mysql_select_db('rajataggarwal_zymichost_masterdb',$dbc);
$query="SELECT * from user where User_id='$userid'";
$result=mysql_query($query);
while($row = mysql_fetch_array($result, MYSQL_ASSOC))
{
	if($row[Password]==$password)
	{	
	ob_start();
	session_start();
	$_SESSION['userid']=$userid;
	header('Refresh:2;url=main2.php');
	ob_flush();
	}
	else
	{
ob_start();
echo "<script>alert(\"Incorrect pasword\");</script>"; 
header('Refresh:2;url=index.php');
ob_flush();
	}
}
?>