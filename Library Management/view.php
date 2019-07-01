<?php include "templates/header.php"; ?>
<?php
		require "config.php";
		require "common.php";
		
		if(count($_GET) > 0){
			$styleQueryIn = "style='display:none;'";
		    parse_str($_SERVER["QUERY_STRING"], $query_array);
	}
	else{
		$styleQueryIn = "style='display:block;'";
	}
        if(count($_GET) > 0 && $query_array['page']){
try {

    $tablename = $query_array['table'];
	$connection = new PDO($dsn, $username, $password, $options);

    $sql_column_names = "DESCRIBE $tablename";
    $q = $connection->prepare($sql_column_names);
    $q->execute();
    $table_fields = $q->fetchAll(PDO::FETCH_COLUMN);

	$sql = "SELECT count(*) from $tablename";

  	$statement = $connection->prepare($sql);
	$statement->execute();
    $result = $statement->fetchAll();

	$table_fields_length = $q->columnCount();
	$cols = $statement->columnCount(); 
	$total = $result[0][0];
	
	// How many items to list per page
    $limit = 200;

    // How many pages will there be
    $pages = ceil($total / $limit);


    // What page are we currently on?
    $page = $query_array['page'];
    // Calculate the offset for the query
    $offset = ($page - 1)  * $limit;
    // Some information to display to the user
    $start = $offset + 1;
    $end = min(($offset + $limit), $total);

    // The "back" link
    $prevlink = ($page > 1) ? '<a href="?table=' . ($tablename) . '&page=1" title="First page">&laquo;</a> <a href="?table=' . ($tablename) . '&page=' . ($page - 1) . '" title="Previous page">&lsaquo;</a>' : '<span class="disabled">&laquo;</span> <span class="disabled">&lsaquo;</span>';

    // The "forward" link
    $nextlink = ($page < $pages) ? '<a href="?table=' . ($tablename) . '&page=' . ($page + 1) . '" title="Next page">&rsaquo;</a> <a href="?table=' . ($tablename) . '&page=' . $pages . '" title="Last page">&raquo;</a>' : '<span class="disabled">&rsaquo;</span> <span class="disabled">&raquo;</span>';

	// Display the paging information
    echo '<div class="container" style="padding: 25px 0;" ><div id="paging"><p>', $prevlink, ' Page ', $page, ' of ', $pages, ' pages, displaying ', $start, '-', $end, ' of ', $total, ' results ', $nextlink, ' </p></div>';

    // Prepare the paged query
    $stmt = "SELECT *
        	FROM $tablename
        	LIMIT $offset,$limit";
	// Bind the query params
	$statement1 = $connection->prepare($stmt);
	$statement1->execute();
    $result1 = $statement1->fetchAll();
	$connection = null;
	$cols1 = $statement1->columnCount();

	?>
	<table style="width:100%" class ="users">
		<thead>
			<tr>
				<?php for($i =0; $i < $cols1; $i++){  ?>
				<th><?php echo $table_fields[$i];?></th>
			<?php } ?> 
			</tr>	
		</thead>
		<tbody>
		<?php foreach ($result1 as $row) { ?>
			<tr>
				<?php for($i =0; $i < $cols1; $i++){  ?>
				<td><?php echo $row[$i]; ?></td>
				<?php } ?>
			</tr>
			<?php } ?>
		</tbody>
		
	</table>

	<?php   
}
catch(PDOException $e)
    {
	echo $sql . "<br>" . $e->getMessage();
    echo "Connection failed: " . $e->getMessage();
    }
}
        else{
		    $connection = new PDO($dsn, $username, $password, $options);
         
	       $sql = "SELECT TABLE_NAME
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='project'";
            $statement = $connection->prepare($sql);
	        $statement->execute();

	        $result = $statement->fetchAll();
	        $connection = null;
        }
  		
	
?>
<?php
if(isset($_POST['submit'])){
	$tablename = $_POST['ChooseTable'];
	header('Location: view.php?table=' . $tablename . '&page=1');
	}
?>

<form method="post"  <?php echo $styleQueryIn; ?>>
<div class="userList">
	<?php
		if ($result && $statement->rowCount() > 0) { ?>
		<table class="users">
				<thead>
					<tr>
						<th>Table Name</th>
					</tr>
				</thead>
				<tbody>
		<?php foreach ($result as $row) { ?>
				<tr>
					<td><?php echo escape($row["TABLE_NAME"]); ?></td>
				</tr>

				<div class="tilesView">
					<p class="tiles">TABLE NAME: <?php echo escape($row["TABLE_NAME"]); ?></p>
				</div>
		<?php } ?>
				<b class="chooseTable">Choose from tables below:</b> <input type="text" name="ChooseTable" class="view_input"><br>
  				<input type="submit" value="Submit" name="submit" class="view_button">
				</tbody>
		</table>

		<?php } else { ?>
			<blockquote>No results found.</blockquote>
		<?php }
	 ?>
</div>
</form>
</div>
<?php include "templates/footer.php"; ?>
