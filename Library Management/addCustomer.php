<?php session_start();?>
<?php

/**
 * Use an HTML form to create a new entry in the
 * users table.
 *
 */
require "config.php";
require "common.php";
$style = "";
$emptyStyle= ""; ?>
<!-- <script type="text/javascript">
  var mailVal = localStorage.getItem("email");
  if(mailVal){
      window.location.href = 'view.php';
  }
</script> -->
<?php
if (isset($_POST['addCustomer'])) {
    $lname = $_POST['lname'];
    $fname = $_POST['fname'];
    $mobileno = $_POST['mobileno'];
    $mailingAddress = $_POST['mailingAddress'];
    $discountCode = $_POST['discountCode'];
	if (!empty($lname) && !empty($fname) && !empty($mobileno) && !empty($mailingAddress) && !empty($discountCode)) {
		$emptyStyle=  "style='display:none;'";
  try {
		$connection = new PDO($dsn, $username, $password, $options);
		$new_user = array(
			"lname" => $lname,
			"fname"     => $fname,
            "telephone_no" => $mobileno,
            "mailing_address" => $mailingAddress,
			"discount_code"  => $discountCode
		);
		$sql = sprintf(
				"SELECT *
				FROM customer
				WHERE lname = :lname AND fname = :fname");
		$statement = $connection->prepare($sql);
		//$statement->bindParam(':telephone_no', $new_user["telephone_no"], PDO::PARAM_STR);
        $statement->bindParam(':lname', $new_user["lname"], PDO::PARAM_STR);
        $statement->bindParam(':fname', $new_user["fname"], PDO::PARAM_STR);

		$statement->execute();
    if( $statement->rowCount() == 0){
		$style = "style='display:none;'";
		$sql1 = sprintf(
				"INSERT INTO %s (%s) values (%s)",
				"customer",
				implode(", ", array_keys($new_user)),
				":" . implode(", :", array_keys($new_user))
		);

		$statement = $connection->prepare($sql1);
		$statement->execute($new_user);
		//$connection = null;
		if ($statement->rowCount() > 0) {
            $_SESSION["mobileno"]=$mobileno;
            header('Location: addTransaction.php');
			 //exit();
		}
	}
	else{
        ///echo "Are you a new Customer!";
        //if (isset($_POST['newCustomer'])){
        $_SESSION["lname"]=$lname;
        $_SESSION["fname"]=$fname;
        $_SESSION["mobileno"]=$mobileno;
        $_SESSION["mailing_address"]=$mailing_address;
        $_SESSION["discount_code"]=$discount_code;
        header('Location: newCustomer.php');
	   //}
    }
	} catch(PDOException $error) {
	       echo $error->getMessage();
				 die();
	}
}
else{
$emptyStyle=  "style='display:block;'";
}
}
?>
<?php include "templates/header.php";?>
<div id="error" <?php echo $style;?>>Mobile Number already exists!</div>
<div id="error" <?php echo $emptyStyle;?>>No field can be left empty!</div>

<div class="signup-wrap"     style="min-height: 602px;">
	<div class="login-html">
    <input id="tab-2" type="radio" name="tab" class="sign-up" checked><label for="tab-2" class="tab">Add Customer</label>
    <div class="login-form">
    <div class="sign-up-htm" style="transform:rotate(0)">
        <form method="post">
            <div class="group">
                <label for="lname" class="label">Last Name:</label>
                <input id="lname" name="lname" type="text" class="input">
            </div>   
            <div class="group">
                <label for="fname" class="label">First Name:</label>
                <input id="fname" name="fname" type="text" class="input">
            </div>
            <div class="group">
                <label for="mobileno" class="label">Mobile No.:</label>
                <input id="mobileno" name="mobileno"  pattern="^\d{10}$" type="number" class="input">
            </div>
            <div class="group">
                <label for="mailingAddress" class="label">Mailing Address:</label>
                <input id="mailingAddress" name="mailingAddress" type="text" class="input">
            </div>
            <div class="group">
                <label for="discountCode" class="label">Discount Code:</label>
                <input id="discountCode" name="discountCode" type="text" class="input">
            </div>
            
            <div class="group">
                <input type="submit" name="addCustomer"  class="button" value="ADD CUSTOMER">
            </div>

        </form>
    </div>
    </div>  
    </div>
</div> 
<!-- <script type="text/javascript">
   function removeItem(el){
    var parent = $(el).parent();
    $(el).parent().empty();
    parent.remove();
    calculatePrice();
}
  

</script> -->
</body>

</html>

<?php include "templates/footer.php"; ?>
