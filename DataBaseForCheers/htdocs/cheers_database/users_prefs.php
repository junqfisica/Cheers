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

	$sql = "SELECT ID, Smoke, MinAge, MaxAge, Habit, WantToMeetMan, WantToMeetWoman FROM users_prefs";
	$result = mysqli_query($conn, $sql);


	if(mysqli_num_rows($result) > 0){

		//show data from each row
		while($row = mysqli_fetch_assoc($result)){
			// -------------- Convet 0 or 1 into false or true -------------------------------
			$row['Smoke'] = json_encode($row['Smoke'] ? true : false);
			$row['WantToMeetWoman'] = json_encode($row['WantToMeetWoman'] ? true : false);
			$row['WantToMeetMan'] = json_encode($row['WantToMeetMan'] ? true : false);
			//--------------------------------------------------------------------------------

			echo("ID:".$row['ID'] . "|Smoke:".$row['Smoke']. "|MinAge:".$row['MinAge'] ."|MaxAge:".$row['MaxAge']
."|Habit:".$row['Habit']."|WantToMeetMan:".$row['WantToMeetMan'] . "|WantToMeetWoman:".$row['WantToMeetWoman']. ";");

		}

	} else {

		echo("No data avaible.");

	}

	mysqli_close($conn);

?>
