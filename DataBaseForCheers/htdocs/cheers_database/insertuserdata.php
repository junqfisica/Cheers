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

	function PostToUsersDatabase(){

		global $conn;

		$name = $_POST["namePost"];
		$age = $_POST["agePost"];
		$email = $_POST["emailPost"];
		$gender = $_POST["genderPost"];
	
		// ------------ Prevent injections --------------------------
		$name = mysql_real_escape_string($name);
		$age = mysql_real_escape_string($age);
		$email = mysql_real_escape_string($email);
		$gender = mysql_real_escape_string($gender);

		$query = "SELECT Email FROM users WHERE Email='".$email."'";
		$queryResult = mysqli_query($conn, $query);

		if(mysqli_num_rows($queryResult) != 0){// Check if the user exist by vertfing the email.

			echo("User already Exist");

		} else {

			$sql = "INSERT INTO users (Name, Age, Email, Gender)
		VALUES ('".$name."', '".$age."', '".$email."','".$gender."')";

			$result = mysqli_query($conn, $sql);

			if(!$result) die ("There was an error". mysqli_error($conn));
			else echo("Success");

		}
	}

	function PostToUsersPrefsDatabase(){

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

		$query = "SELECT ID FROM users_prefs WHERE ID='".$id."'";
		$queryResult = mysqli_query($conn, $query);

		if(mysqli_num_rows($queryResult) != 0){// Check if the user exist by vertfing the id.

			echo("User already Exist");

		} else {

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

			$sql = "INSERT INTO users_prefs (ID, Smoke, MinAge, MaxAge, Habit, WantToMeetMan, WantToMeetWoman)
		VALUES ('".$id."', '".$smoke."', '".$minAge."','".$maxAge."','".$habit."','".$wantMan."','".$wantWoman."')";

			$result = mysqli_query($conn, $sql);

			if(!$result) die ("There was an error". mysqli_error($conn));
			else echo("Success");

		}
	}

	function PostToMatchsDatabase(){

		global $conn;

		$chooserId = $_POST["chooserPost"];
		$chosenId = $_POST["chosenPost"];
		$cheers = $_POST["cheersPost"];
		// ------------ Prevent injections --------------------------
		$chooserId = mysql_real_escape_string($chooserId);
		$chosenId = mysql_real_escape_string($chosenId);
		$cheers = mysql_real_escape_string($cheers);

		// -------------- Convet true or false into 1 or 0 -------------------------------
		if(filter_var($cheers, FILTER_VALIDATE_BOOLEAN) ){
			$cheers = 1;
			$resultCheers = "None"; // Unknow is cheers is positive.
		} else {
			$cheers = 0;
			$resultCheers = "Fail"; // Fail.
		}

		$sql = "INSERT INTO matchs (Chooser_ID, Chosen_ID, Cheers,Result) VALUES ('".$chooserId."', '".$chosenId."', '".$cheers."','".$resultCheers."')";

		$result = mysqli_query($conn, $sql);

		if(!$result) die ("There was an error". mysqli_error($conn));
		else echo("Success");

	}



	// Call the function base on a Post from Unity
	$functionName = $_POST["functionName"];
	switch ($functionName){

		default:
			echo("No function Found");
			break;

		case "PostToUsersDatabase":
			PostToUsersDatabase();
			break;

		case "PostToUsersPrefsDatabase":
			PostToUsersPrefsDatabase();
			break;

		case "PostToMatchsDatabase":
			PostToMatchsDatabase();
			break;
	}

	mysqli_close($conn);
?>