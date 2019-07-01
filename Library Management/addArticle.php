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
if (isset($_POST['addArticle'])) {
    $id = $_POST['id'];
    $title = $_POST['title'];
    $pages = $_POST['pages'];
    $volume = $_POST['volume'];
    $publicationYear = $_POST['publicationYear'];
    $magazineId = $_POST['magazineId'];
    if (!empty($id) && !empty($title) && !empty($pages) && !empty($volume) && !empty($publicationYear) && !empty($magazineId)) {
		$emptyStyle=  "style='display:none;'";
  try {

    $query = "INSERT INTO articles(unique_id,title,pages,volume_number,publication_year,magazine_id) VALUES('$id','$title','$pages','$volume','$publicationYear',,'$magazineId')";
		$connection = new PDO($dsn, $username, $password, $options);
		$new_user = array(
			"unique_id" => $id,
			"title"     => $title,
            "pages"       => $pages,
            "volume_number" => $volume,
            "publication_year"  => $publicationYear,
            "magazine_id" => $magazineId
		);
		$sql = sprintf(
				"SELECT *
				FROM articles
				WHERE unique_id = :unique_id");
		$statement = $connection->prepare($sql);
		$statement->bindParam(':unique_id', $new_user["unique_id"], PDO::PARAM_STR);
		$statement->execute();
    if( $statement->rowCount() == 0){
        $style = "style='display:none;'";
        $sql1 = sprintf(
            "INSERT INTO %s (%s) values (%s)",
            "articles",
            implode(", ", array_keys($new_user)),
            ":" . implode(", :", array_keys($new_user))
    );

    $statement = $connection->prepare($sql1);
    $statement->execute($new_user);

    for($i =1; $i <= $_POST['numberOfAuthors']; $i++)
{
    $new_author = array(
        "lname"     =>  $_POST['authorLastName'.$i],
        "fname"       => $_POST['authorFirstName'.$i],
        "email" => "",
    );
    $sql_author = sprintf(
				"INSERT INTO %s (%s) values (%s)",
				"author",
				implode(", ", array_keys($new_author)),
				":" . implode(", :", array_keys($new_author))
	);

	$statement = $connection->prepare($sql_author);
    $statement->execute($new_author);
    $connection = null;
	if ($statement->rowCount() > 0) {
            header('Location: index.php');
             exit();
    }
}

	}
	else{
		$style = "style='display:block;";
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
<?php include "templates/header.php"?>
<div id="error" <?php echo $style;?>>Article ID already exists!</div>
<div id="error" <?php echo $emptyStyle;?>>No field can be left empty!</div>

<div class="signup-wrap"     style="height: 100%;">
	<div class="login-html">
    <input id="tab-2" type="radio" name="tab" class="sign-up" checked><label for="tab-2" class="tab">Add Article</label>
<div class="login-form" style="overflow:auto;height:100%;overflow-x: hidden;">
    <div class="sign-up-htm">
        <form method="post">
            <div class="group">
                <label for="id" class="label">Enter the Article ID:</label>
                <input id="id" name="id" type="text" class="input">
            </div>   
            <div class="group">
                <label for="title" class="label">Enter the title:</label>
                <input id="title" name="title" type="text" class="input">
            </div>
            <div class="group">
                <label for="pages" class="label">Enter the pages:</label>
                <input id="pages" name="pages" type="text" class="input">
            </div>
            <div class="group">
                <label for="volume" class="label">Enter Volume No.:</label>
                <input id="volume" name="volume" type="number" class="input">
            </div>
            <div class="group">
                <label for="publicationYear" class="label">Publication Year:</label>
                <input id="publicationYear" name="publicationYear" type="date" class="input">
            </div>
            <div class="group">
                <label for="magazineId" class="label">Magazine ID:</label>
                <input id="magazineId" name="magazineId" type="number" class="input">
            </div>
            <div class="group">
                <input type="button" name="addInput" class="button" onclick="addInputElement()" value="ADD Author">
                <input type="text" id="numberOfAuthors" style="visibility:hidden" name="numberOfAuthors" value="1">
            </div> 
            <div class="group row">
                <span onclick="removeItem(this)" style="position: absolute;right: 0;color: beige;border: 1px solid;z-index: 1111;cursor: pointer;">X</span>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="authorFirstName1" class="label">Author First Name:</label>
                <input id="authorFirstName1" name="authorFirstName1" type="text" class="input"/>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="authorLastName1" class="label">Author Last Name:</label>
                <input id="authorLastName1" name="authorLastName1" type="text" class="input"/>
                </div>
            </div>
            <div class="group" id="articleSubmit">
                <input type="submit" name="addArticle" class="button" value="ADD ARTICLE">
            </div>
        </form>
    </div>
</div>   
</div>
</div>
<script type="text/javascript">   
 function addInputElement(){
     document.getElementById("numberOfAuthors").value = parseInt(document.getElementById("numberOfAuthors").value)+1;
     var count = document.getElementById("numberOfAuthors").value;
     var el = document.getElementById("articleSubmit");
     el.insertAdjacentHTML('beforebegin', `<div class="group row">
                <span onclick="removeItem(this)" style="position: absolute;right: 0;color: beige;border: 1px solid;z-index: 1111;cursor: pointer;">X</span>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="authorFirstName${count}" class="label">Author First Name:</label>
                <input id="authorFirstName${count}" name="authorFirstName${count}" type="text" class="input"/>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <label for="authorLastName${count}" class="label">Author Last Name:</label>
                <input id="authorLastName${count}" name="authorLastName${count}" type="text" class="input"/>
                </div>
            </div>`);

} 

function removeItem(el){
    var parent = $(el).parent();
    $(el).parent().empty();
    parent.remove();
    document.getElementById("numberOfAuthors").value = parseInt(document.getElementById("numberOfAuthors").value)-1;
}
</script>
</body>

</html>

<?php include "templates/footer.php"; ?>
