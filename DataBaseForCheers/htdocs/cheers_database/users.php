<?php
	$serverName = "localhost";
	$serverUserName = "root";
	$serverPassword = "iag2733";
	$dbName = "cheers_devphase";

	// Make the connection

	$conn = new mysqli($serverName, $serverUserName, $serverPassword, $dbName);

	//Check Connection

	if(!$conn){

		die("Connection Fail. ". mysqli_connect_error()."<br>");

	}
	//else echo("Connection Succes."."<br>");

	$sql = "SELECT ID, Name, Age, Email, Gender FROM users";
	$result = mysqli_query($conn, $sql);

	if(mysqli_num_rows($result) > 0){

		//show data from each row
		while($row = mysqli_fetch_assoc($result)){

			echo("ID:".$row['ID'] . "|Name:".$row['Name'] . "|Age:".$row['Age'] ."|Email:".$row['Email'] ."|Gender:".$row['Gender']. ";");

		}

	} else {

		echo("No data avaible.");

	}

	mysqli_close($conn);

?>
