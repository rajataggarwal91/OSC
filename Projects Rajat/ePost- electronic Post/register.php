<?
$dbc=mysqli_connect('www.rajataggarwal.zymichost.com','351871_rajat','thereare','rajataggarwal_zymichost_masterdb');
$userid=$_POST['userid'];
$pass=$_POST['pass'];
$conpass=$_POST['conpass'];
if($pass!=$conpass)
{
ob_start();
echo "<script>alert(\"Passwords do not match\");</script>";
header('Refresh:2;url=index.php');
ob_flush();
exit;
}
$query="INSERT INTO user VALUES ('$userid','$pass')";
$result=mysqli_query($dbc,$query);
if($result)
{
ob_start();
echo "<script>alert(\"Registered successfully.\");</script>"; 
header('Refresh:0;url=index.php');
ob_flush();
}
else
{
 ob_start();
 echo "<script>alert(\"Try another userID.\");</script>"; 
 header('Refresh:0;url=index.php');
 ob_flush();
}
mysqli_close($dbc);
?>