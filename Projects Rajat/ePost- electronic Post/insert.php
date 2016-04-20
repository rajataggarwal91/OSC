<?php
$userid=$_SESSION['userid'];
$adline1=$_POST['adline1'];
$adline2=$_POST['adline2'];
$city=$_POST['city'];
$state=$_POST['state'];
$pin=$_POST['pin'];
$message=$_POST['message'];
$dbc=mysqli_connect('rajataggarwal.zymichost.com','351871_rajat','thereare','rajataggarwal_zymichost_masterdb')
or die('Error connecting to MySql server.');
$query="INSERT INTO send_details VALUES". "('','$userid','$adline1','$adline2','$city','$state','$pin','$message')";
ob_start();
echo "<script>alert('Message added successfully');</script>";
header('Refresh:1;url="payment.php"');
ob_flush();
$result= mysqli_query($dbc,$query)
or die ('Error querying db');
mysqli_close($dbc); 
?>
<html>
<head>
</head>
</html>
	
