<?php session_start();?>
<?php
/**
 * Use an HTML form to create a new entry in the
 * users table.
 *
 */
require "config.php";
require "common.php";
$successStyle = "";
$itemStyle = "";
$style = "";
$emptyStyle= ""; ?>
<!-- <script type="text/javascript">
  var mailVal = localStorage.getItem("email");
  if(mailVal){
      window.location.href = 'view.php';
  }
</script> -->
<?php
if (isset($_POST['addTransaction'])) {
    try {
    $successStyle =  "style='display:none;'";
    $itemStyle=  "style='display:none;'";
    $itemIdPresent = 1;
	$connection = new PDO($dsn, $username, $password, $options);
    $transactionDate = $_POST['transactionDate'];
    $purchasePrice = 0;
    $discount = 0;
    $totalPrice = 0;
    $mobileno = $_SESSION["mobileno"];
    for($i =1; $i <= $_POST['numberOfItems']; $i++){
    $new_item = array(
        "_id"     =>  $_POST['itemId'.$i],
        "price"       => $_POST['itemPrice'.$i]
    );
    $purchasePrice += $new_item["price"];
    $sql_item = sprintf(
        "SELECT count(*)
        FROM item
        WHERE _id = :_id AND price = :price");
    $statement = $connection->prepare($sql_item);
    $statement->bindParam(':_id', $new_item["_id"], PDO::PARAM_STR);
    $statement->bindParam(':price', $new_item["price"], PDO::PARAM_STR);
    $statement->execute();
    $result = $statement->fetchAll();
	if ($result[0][0] == 0) {
        $itemIdPresent = 0;
        break;
    }
}
    if($itemIdPresent == 1){
	if (!empty($purchasePrice)) {
		$emptyStyle=  "style='display:none;'";
		
		$sql = sprintf(
				"SELECT cid
				FROM customer
				WHERE telephone_no = :telephone_no");
		$statement = $connection->prepare($sql);
		$statement->bindParam(':telephone_no', $mobileno, PDO::PARAM_STR);
        $statement->execute();
        $result = $statement->fetchAll();
        $sql_discount = sprintf(
            "SELECT SUM(total_price) as total
            FROM transaction
            WHERE transaction_date <= NOW()
            and transaction_date >= Date_add(Now(),interval - 60 month)
            AND cid = :cid;");
       $statement1 = $connection->prepare($sql_discount);
       $statement1->bindParam(':cid', $result[0][0], PDO::PARAM_STR);
       $statement1->execute();
       $result1 = $statement1->fetchAll();
       if($result1[0][0] >= 500){
        $discount = 5;
        $totalPrice = $purchasePrice*(1-(2.5*($discount/100)));
       }
       else{
           $discount = floor($result1[0][0]/100);
           $totalPrice = $purchasePrice*(1-(2.5*($discount/100)));
       }
        $new_user = array(
                "transaction_date" => $transactionDate,
                "total_price"     => $totalPrice,
                "purchase_price"  => $purchasePrice,
                "discount" => $discount,
                "cid"  => $result[0][0]
            );
    
    if( $statement->rowCount() != 0){
		$style = "style='display:none;'";
		$sql1 = sprintf(
				"INSERT INTO %s (%s) values (%s)",
				"transaction",
				implode(", ", array_keys($new_user)),
				":" . implode(", :", array_keys($new_user))
		);
		$statement = $connection->prepare($sql1);
		$statement->execute($new_user);
		$connection = null;
		if ($statement->rowCount() > 0) {
            $successStyle = "style='display:block;'";
            //header('Location: index.php');
			//exit();
		}
	}
	else{
		$style = "style='display:block;'";
	}
}
else{
$emptyStyle=  "style='display:block;'";
}
    }else{
        echo "Exit From Loop";
        $itemStyle=  "style='display:block;'";
    }
} 
catch(PDOException $error) {
    echo $error->getMessage();
    die();
}
}
?>
<?php $GLOBALS['state'] = 0; include "templates/header.php";?>
<div id="error" <?php echo $successStyle;?>>Transaction Successful for <?php echo $totalPrice;?> at a discount of <?php echo $discount;?>!</div>
<div id="error" <?php echo $itemStyle;?>>Item Id or Price doesn't exist!</div>
<div id="error" <?php echo $style;?>>Customer doesn't exist!</div>
<div id="error" <?php echo $emptyStyle;?>>No field can be left empty!</div>

<div class="signup-wrap"  style="height: 100%">
	<div class="login-html" style="overflow:auto">
    <input id="tab-2" type="radio" name="tab" class="sign-up" checked><label for="tab-2" class="tab">Add Transaction</label>
<div class="login-form">
    <div class="sign-up-htm">
        <form name="transactionForm" method="post">
            <div class="group">
                <label for="transactionDate" class="label">Transaction Date:</label>
                <input id="transactionDate" name="transactionDate" type="text" value="<?php echo date('y/m/d'); ?>" readonly="" class="input"/>
            </div>  
            <div class="group">
                <input type="button" name="addInput" class="button" onclick="addInputElement()" value="ADD ITEM">
                <input type="text" id="numberOfItems" style="visibility:hidden" required="false" name="numberOfItems" value="1">
            </div> 
            <div class="group row">
                <span onclick="removeItem(this)" style="position: absolute;right: 0;color: beige;border: 1px solid;z-index: 1111;cursor: pointer;">X</span>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="itemId1" class="label">Item Id:</label>
                <input id="itemId1" name="itemId1" type="number" class="input"/>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="itemPrice1" class="label">Item Price:</label>
                <input id="itemPrice1" name="itemPrice1" type="number" min="1" step="any" class="input itemPrice"/>
                </div>
            </div> 
            <div class="group" id="submitButton">
                <input type="submit" name="addTransaction" class="button" value="ADD TRANSACTION">
            </div>
        </form>
    </div>
</div>   
</div>
</div>

<script type="text/javascript">   
 function addInputElement(){
     document.getElementById("numberOfItems").value = parseInt(document.getElementById("numberOfItems").value)+1;
     var count = document.getElementById("numberOfItems").value;
     var el = document.getElementById("submitButton");
     el.insertAdjacentHTML('beforebegin', `<div class="group row">
                <span onclick="removeItem(this)" style="position: absolute;right: 0;color: beige;border: 1px solid;z-index: 1111;cursor: pointer;">X</span>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="itemId${count}" class="label">Item Id:</label>
                <input id="itemId${count}" name="itemId${count}" type="number" class="input"/>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="itemPrice${count}" class="label">Item Price:</label>
                <input id="itemPrice${count}" name="itemPrice${count}" type="number" min="1" step="any" onchange="calculatePrice()" class="input itemPrice"/>
                </div>
            </div>`);
} 
function removeItem(el){
    var parent = $(el).parent();
    $(el).parent().empty();
    parent.remove();
    document.getElementById("numberOfItems").value = parseInt(document.getElementById("numberOfItems").value)-1;
}
</script>
</body>

</html>

<?php include "templates/footer.php"; ?>