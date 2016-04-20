<?
session_start();
echo "hello ".$_SESSION['userid'];
?>
<html>
<head>
<title>e-post</title>
</head>
<body>
<a href="index.php" align="center"onclick=session_destroy()>LOGOUT</a>
<form id="form1" name="form1" method="POST" action="insert.php">
      <p align="center"><strong>To:</strong></p>
      <p align="center">
        Address line1                		
        <input name="adline1" type="text" id="adline1" size="50" />
     </p>
      <p align="center">
    <label>Address line2</label>
    <input name="adline2" type="text" id="adline2" size="40" />
  </p>
  <p align="center">
    <label>City
    <input name="city" type="text" id="city" />
    </label>
</p>
  <p align="center">
    <label>State
    <input name="state" type="text" id="state" />
    </label>
</p>
    <p align="center">
    <label>Pincode
    <input name="pin" type="text" id="pin" size="10" maxlength="6" />
    </label>
  </p>
      <p align="left"> <strong>Message&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong></p>
    </blockquote>
    </blockquote>
  <p align="center">
    <label>
    <textarea name="message" cols="100" rows="10" id="message"></textarea>
    </label>
  </p>
  <p align="center">
    <label>
    <input type="submit" name="Submit" value="Submit" />
    </label>
  </p>
  <p align="center">&nbsp;</p>
  <p align="center">&nbsp;</p>
  <p align="center">&nbsp;</p>
  <p align="center">&nbsp;</p>
  <p align="center">&nbsp;</p>
</div>
</form>
<p>&nbsp;</p>

</body>
</html>
