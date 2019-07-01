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
if (isset($_POST['newCustomer'])) {
    $transactionnumber = $_POST['transactionnumber'];
	//if (!empty($lname) && !empty($fname) && !empty($mobileno) && !empty($mailingAddress) && !empty($discountCode)) {
	//	$emptyStyle=  "style='display:none;'";
  try {
        header('Location: addTransaction.php');
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
    <input id="tab-2" type="radio" name="tab" class="sign-up" checked><label for="tab-2" class="tab">Continue</label>
    <div class="login-form">
    <div class="sign-up-htm" style="transform:rotate(0)">
        <form method="post">
        	<div class="group">
        		<p>User Already Exists!!</p>
        	</div>
            <br>
            <br>
            <div class="group">
                <input type="submit" name="newCustomer"  class="button" value="Contine with Transaction">
            </div>
        </form>
    </div>
    </div>  
    </div>
</div> 
</body>

</html>

<?php include "templates/footer.php"; ?>
