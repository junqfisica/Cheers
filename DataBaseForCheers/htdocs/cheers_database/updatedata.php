<?php
	$serverName = "localhost";
	$serverUserName = "root";
	$serverPassword = "iag2733";
	$dbName = "cheers_devphase";


	// Make the connection

	$conn = new mysqli($serverName, $serverUserName, $serverPassword, $dbName);

	//Check Connection

	if(!$conn){

		die("Connection Fail. ". mysqli_connect_error());

	}
	//else echo("Connection Succes."."<br>");

	function UpdateToUsersDatabase(){

		global $conn;

		$id = $_POST["idPost"];
		$name = $_POST["namePost"];
		$age = $_POST["agePost"];
		$gender = $_POST["genderPost"];

		// ------------ Prevent injections --------------------------
		$id = mysql_real_escape_string($id);
		$name = mysql_real_escape_string($name);
		$age = mysql_real_escape_string($age);
		$gender = mysql_real_escape_string($gender);

		$query = "UPDATE users SET Name='".$name."', Age = '".$age."', Gender = '".$gender."'  WHERE ID='".$id."' ";
		$result = mysqli_query($conn, $query);

		if(!$result) die ("There was an error". mysqli_error($conn));
		else echo("Success");
	}

	function UpdateToUsersPrefsDatabase(){

		global $conn;

		$id = $_POST["idPost"];
		$smoke = $_POST["smokePost"];
		$minAge = $_POST["minAgePost"];
		$maxAge = $_POST["maxAgePost"];
		$habit = $_POST["habitPost"];
		$wantMan = $_POST["wantManPost"];
		$wantWoman = $_POST["wantWomanPost"];
		// ------------ Prevent injections --------------------------
		$id = mysql_real_escape_string($id);
		$smoke = mysql_real_escape_string($smoke);
		$minAge = mysql_real_escape_string($minAge);
		$maxAge = mysql_real_escape_string($maxAge);
		$habit = mysql_real_escape_string($habit);
		$wantMan = mysql_real_escape_string($wantMan);
		$wantWoman = mysql_real_escape_string($wantWoman);


		// -------------- Convet true or false into 1 or 0 -------------------------------
		if(filter_var($smoke, FILTER_VALIDATE_BOOLEAN) ){
			$smoke = 1;
		} else {
			$smoke = 0;
		}
		if(filter_var($wantMan,FILTER_VALIDATE_BOOLEAN)){
			$wantMan = 1;
		} else {
			$wantMan = 0;
		}
		if(filter_var($wantWoman,FILTER_VALIDATE_BOOLEAN)){
			$wantWoman = 1;
		}else {
			$wantWoman = 0;
		}
		//--------------------------------------------------------------------------------

		$query = "UPDATE users_prefs SET Smoke='".$smoke."', MinAge='".$minAge."', MaxAge='".$maxAge."', Habit='".$habit."', WantToMeetMan='".$wantMan."', WantToMeetWoman='".$wantWoman."'
		WHERE ID='".$id."'";

		$result = mysqli_query($conn, $query);

		if(!$result) die ("There was an error". mysqli_error($conn));
		else echo("Success");

	}



	// Call the function base on a Post from Unity
	$functionName = $_POST["functionName"];
	switch ($functionName){

		default:
			echo("No function Found");
			break;

		case "UpdateToUsersDatabase":
			UpdateToUsersDatabase();
			break;

		case "UpdateToUsersPrefsDatabase":
			UpdateToUsersPrefsDatabase();
			break;
	}

	mysqli_close($conn);
?>