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
	// ============================== Methods ==========================================
	function GetUserId(){

		global $conn;

		$email = $_POST["emailPost"];
	// ------------ Prevent injections --------------------------
		$email = mysql_real_escape_string($email);

		$query = "SELECT ID, Name, Age, Email, Gender FROM users WHERE Email='".$email."'";
		$queryResult = mysqli_query($conn, $query);

		if(mysqli_num_rows($queryResult) != 0){// Check if the user exist by vertfing the email.

			$row = mysqli_fetch_assoc($queryResult);
			echo("ID:".$row['ID'] . "|Name:".$row['Name'] . "|Age:".$row['Age']."|Gender:".$row['Gender']);

		} else {// If not return 0

			echo("0");
		}

	}

	function GetUserPrefs(){

		global $conn;

		$id = $_POST["idPost"];
	// ------------ Prevent injections --------------------------
		$id = mysql_real_escape_string($id);

		$query = "SELECT ID, Smoke, MinAge, MaxAge, Habit, WantToMeetMan, WantToMeetWoman FROM users_prefs WHERE ID='".$id."'";
		$queryResult = mysqli_query($conn, $query);

		if(mysqli_num_rows($queryResult) != 0){// Check if the user exist by vertfing the id.

			$row = mysqli_fetch_assoc($queryResult);
			// -------------- Convet 0 or 1 into false or true -------------------------------
			$row['Smoke'] = json_encode($row['Smoke'] ? true : false);
			$row['WantToMeetWoman'] = json_encode($row['WantToMeetWoman'] ? true : false);
			$row['WantToMeetMan'] = json_encode($row['WantToMeetMan'] ? true : false);
			//--------------------------------------------------------------------------------

			echo("ID:".$row['ID'] . "|Smoke:".$row['Smoke']. "|MinAge:".$row['MinAge'] ."|MaxAge:".$row['MaxAge']
			."|Habit:".$row['Habit']."|WantToMeetMan:".$row['WantToMeetMan'] . "|WantToMeetWoman:".$row['WantToMeetWoman']);

		} else {// If not return 0

			echo("0");
		}

	}

	function SelectWhoSelectMe(){

		global $conn;

		$myId = $_POST["myIdPost"];
		$minAge = $_POST["minAgePost"];
		$maxAge = $_POST["maxAgePost"];
		$gender = $_POST["genderPost"];
		$bothGender = $_POST["bothGenderPost"]; // string True or False
	// ------------ Prevent injections --------------------------
		$myId = mysql_real_escape_string($myId);
		$minAge = mysql_real_escape_string($minAge);
		$maxAge = mysql_real_escape_string($maxAge);
		$gender = mysql_real_escape_string($gender);
		$bothGender = mysql_real_escape_string($bothGender);

		// ============================ Select the matchs from the table matchs ==============================================
		$query = "SELECT Chooser_ID, Chosen_ID, Result FROM matchs WHERE Chosen_ID = '".$myId."' AND Result = 'None'" ; // Get all the cheers that choose me.

		$matchResult = mysqli_query($conn, $query);
		$idsChooser = array();
		if(mysqli_num_rows($matchResult) > 0){

			while($row = mysqli_fetch_assoc($matchResult)){

				$idsChooser[] = $row['Chooser_ID']; // Store in the array to be used in the sql query.
				//echo($row['Chooser_ID']);
			}

		} else {

			$idsChooser[] = 0; // Add the 0 for be used in the list search.
		}
		//array_walk($idsChosen , 'intval');
		$ids = implode(',', $idsChooser);
		//=======================================================================================================================

		if(filter_var($bothGender,FILTER_VALIDATE_BOOLEAN)){// If true don't use Gender as filter.

			$sql = "SELECT u.ID, u.Name, u.Age, u.Gender, uf.ID, uf.Smoke, uf.Habit FROM users u, users_prefs uf WHERE u.ID IN ({$ids}) AND u.ID = uf.ID AND u.Age BETWEEN '".$minAge."' AND '".$maxAge."'";

		} else {// Otherwise use the gender as filter.

			$sql = "SELECT u.ID, u.Name, u.Age, u.Gender, uf.ID, uf.Smoke, uf.Habit FROM users u, users_prefs uf WHERE u.ID IN ({$ids}) AND u.Gender='".$gender."' AND u.ID = uf.ID AND u.Age BETWEEN '".$minAge."' AND '".$maxAge."'";

		}

		$result = mysqli_query($conn, $sql);


		if(mysqli_num_rows($result) > 0){

			//show data from each row
			while($row = mysqli_fetch_assoc($result)){

			    // -------------- Convet 0 or 1 into false or true -------------------------------
				$row['Smoke'] = json_encode($row['Smoke'] ? true : false);

				echo("ID:".$row['ID']."|Name:".$row['Name']."|Age:".$row['Age']."|Smoke:".$row['Smoke']."|Habit:".$row['Habit'].";");

			}

		} else {

			echo("0"); // no user found

		}

	}

	function SelectUsersByFilter(){

		global $conn;

		$minAge = $_POST["minAgePost"];
		$maxAge = $_POST["maxAgePost"];
		$gender = $_POST["genderPost"];
		$excludeId = $_POST["excludeIdPost"];
		$bothGender = $_POST["bothGenderPost"]; // string True or False
		// ------------ Prevent injections --------------------------
		$minAge = mysql_real_escape_string($minAge);
		$maxAge = mysql_real_escape_string($maxAge);
		$gender = mysql_real_escape_string($gender);
		$excludeId = mysql_real_escape_string($excludeId);
		$bothGender = mysql_real_escape_string($bothGender);

	// ============================ Select the matchs from the table matchs ==============================================
		$query = "SELECT Chooser_ID, Chosen_ID FROM matchs WHERE Chooser_ID = '".$excludeId."'"; // Get all the cheers that this user have.

		$matchResult = mysqli_query($conn, $query);
		$idsChosen = array();
		if(mysqli_num_rows($matchResult) > 0){

			while($row = mysqli_fetch_assoc($matchResult)){

				$idsChosen[] = $row['Chosen_ID']; // Store in the array to be used in the sql query.
				//echo($row['Chosen_ID']);
			}
			$idsChosen[] = $excludeId; // Add the own id to be exclued

		} else {
			$idsChosen[] = $excludeId; // Add the own id to be exclued
		}
		//array_walk($idsChosen , 'intval');
		$ids = implode(',', $idsChosen);
	//=======================================================================================================================

		if(filter_var($bothGender,FILTER_VALIDATE_BOOLEAN)){// If true don't use Gender as filter.

			$sql = "SELECT u.ID, u.Name, u.Age, u.Gender, uf.ID, uf.Smoke, uf.Habit FROM users u, users_prefs uf WHERE u.ID NOT IN ({$ids}) AND u.ID = uf.ID AND u.Age BETWEEN '".$minAge."' AND '".$maxAge."'";

		} else {// Otherwise use the gender as filter.

			$sql = "SELECT u.ID, u.Name, u.Age, u.Gender, uf.ID, uf.Smoke, uf.Habit FROM users u, users_prefs uf WHERE u.ID NOT IN ({$ids}) AND u.Gender='".$gender."' AND u.ID = uf.ID AND u.Age BETWEEN '".$minAge."' AND '".$maxAge."'";

		}

		$result = mysqli_query($conn, $sql);


		if(mysqli_num_rows($result) > 0){

			//show data from each row
			while($row = mysqli_fetch_assoc($result)){

			    // -------------- Convet 0 or 1 into false or true -------------------------------
				$row['Smoke'] = json_encode($row['Smoke'] ? true : false);

				echo("ID:".$row['ID']."|Name:".$row['Name']."|Age:".$row['Age']."|Smoke:".$row['Smoke']."|Habit:".$row['Habit'].";");

			}

		} else {

			echo("0");

		}

	}


	// ========================================================================================


	// Call the function base on a Post from Unity
	$functionName = $_POST["functionName"];
	switch ($functionName){

		default:
			echo("No function Found");
			break;

		case "GetUserId":
			GetUserId();
			break;

		case "GetUserPrefs":
			GetUserPrefs();
			break;

		case "SelectUsersByFilter":
			SelectUsersByFilter();
			break;

		case "SelectWhoSelectMe":
			SelectWhoSelectMe();
			break;
	}

	mysqli_close($conn);
?>