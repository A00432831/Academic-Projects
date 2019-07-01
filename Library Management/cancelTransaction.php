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
if (isset($_POST['cancel'])) {
    $transactionnumber = $_POST['transactionnumber'];
	//if (!empty($lname) && !empty($fname) && !empty($mobileno) && !empty($mailingAddress) && !empty($discountCode)) {
	//	$emptyStyle=  "style='display:none;'";
  try {
		$connection = new PDO($dsn, $username, $password, $options);
		$new_user = array(
			"transaction_no" => $transactionnumber,
		);
		$sql = sprintf(
				"SELECT *
				FROM transaction
				WHERE transaction_no = :transaction_no");
		$statement = $connection->prepare($sql);
		$statement->bindParam(':transaction_no', $new_user["transaction_no"], PDO::PARAM_STR);
		$statement->execute();	
		$result = $statement->fetchAll();
    if( $statement->rowCount() > 0){
		$now = time(); // or your date as well
        $your_date = strtotime($result[0]["transaction_date"]);
        $datediff = $now - $your_date;

		$datediff = round($datediff / (60 * 60 * 24));
		if($datediff <= 30){
		$style = "style='display:none;'";
		$sql1 = sprintf(
				"DELETE FROM transaction
				WHERE transaction_no = :transaction_no"
			);
		$statement1 = $connection->prepare($sql);
		$statement1->bindParam(':transaction_no', $new_user["transaction_no"], PDO::PARAM_STR);
		$statement1 = $connection->prepare($sql1);
		$statement1->execute($new_user);
		$connection = null;
		//if ($statement->rowCount() == 0) {
            //$_SESSION["mobileno"]=$mobileno;
         //   header('Location: index.php');
		//	exit();
		//}
		}
		else{
			$style = "style='display:block;";
		}
	}
	else{
        //$_SESSION["mobileno"]=$mobileno;
       // header('Location: index.php');
		// $style = "style='display:block;'";
	}
	} catch(PDOException $error) {
	       echo $error->getMessage();
				 die();
	}
}
// else{
// $emptyStyle=  "style='display:block;'";
// }
//}
?>
<?php include "templates/header.php";?>
<div id="error" <?php echo $style;?>>Transaction was done before 30 days!</div>
<div id="error" <?php echo $emptyStyle;?>>No field can be left empty!</div>

<div class="signup-wrap"     style="min-height: 288px;">
	<div class="login-html">
    <input id="tab-2" type="radio" name="tab" class="sign-up" checked><label for="tab-2" class="tab">Cancel Transaction</label>
    <div class="login-form">
    <div class="sign-up-htm" style="transform:rotate(0)">
        <form method="post">
            <div class="group">
                <label for="lname" class="label">Transaction Number:</label>
                <input id="lname" name="transactionnumber" type="text" class="input">
            </div>   
            
            <div class="group">
                <input type="submit" name="cancel"  class="button" value="CANCEL">
            </div>
        </form>
    </div>
    </div>  
    </div>
</div> 
</body>

</html>

<?php include "templates/footer.php"; ?>
