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

	}//else echo("Connection Succes."."<br>");

	function CheckMatchs(){

		global $conn;

		$id = $_POST["idPost"];
		$id = mysql_real_escape_string($id); // Prevent Injections


		$query = "SELECT Chooser_ID, Chosen_ID, Cheers, Result FROM matchs WHERE Chooser_ID='".$id."' AND Result = 'None'"; // Select all the users that this id gave Cheers
		$queryResult = mysqli_query($conn, $query);

		$chossenIds = array();
		if(mysqli_num_rows($queryResult) > 0){// Check if the user exist in matchs table.

			//show data from each row
			while($row = mysqli_fetch_assoc($queryResult)){

			    // -------------- Convet 0 or 1 into false or true -------------------------------
				$isCheers = json_encode($row['Cheers'] ? true : false);

				//echo(" Chooser_ID:".$row['Chooser_ID']."|Chosen_ID:".$row['Chosen_ID']."|Cheers:".$isCheers."|Result:".$row['Result']."<br>"); // Print for test only
				$chossenIds[] = $row['Chosen_ID']; // Store the chosen id's data for this user
			}

			foreach ($chossenIds as &$idValue) {// For each id chosen check the results

				// Get the Result from chosen ids for this user
				$query2 = "SELECT Chooser_ID, Chosen_ID, Cheers, Result FROM matchs WHERE Chooser_ID='".$idValue."' AND Chosen_ID = '".$id."'";
				$query2Result = mysqli_query($conn, $query2);

				if(mysqli_num_rows($query2Result) > 0){

					$row2 = mysqli_fetch_assoc($query2Result);

					// -------------- Convet 0 or 1 into false or true -------------------------------
					$isCheers = json_encode($row2['Cheers'] ? true : false);

					if($isCheers){

						echo("User " .$id. " has a mutual matchs with user ". $idValue."<br>");
						//Update the table match
						$updateQuery = "UPDATE matchs SET Result='Success' WHERE (Chooser_ID='".$id."' AND Chosen_ID = '".$idValue."') OR (Chooser_ID='".$idValue."' AND Chosen_ID = '".$id."')" ;
						// Insert users into mutal matchs
						$sql = "INSERT INTO mutual_matchs (ID_1, ID_2) VALUES ('".$id."', '".$idValue."')";

						$updateResult = mysqli_query($conn, $updateQuery);
						$sqlResult = mysqli_query($conn,$sql);
						// Check for errors
						if(!$updateResult) die ("There was an error". mysqli_error($conn));
						if(!$sqlResult) die ("There was an error". mysqli_error($conn));


					} else {

						echo("User " .$id. " has no mutual matchs with user ". $idValue."<br>");
					}

				} else {

					echo("User ".$idValue. " has not chosen user ".$id." yet"."<br>");
				}
			}

			echo("Success"); // Matchs checked succesfuly.

		} else {

			echo("The user ".$id. " has no match"."<br>");

		}
	}

    function CheckMutualMatchs(){

        global $conn;

        $id = $_POST["idPost"];
		$id = mysql_real_escape_string($id); // Prevent Injections

        $query = "SELECT ID_1, ID_2 FROM mutual_matchs WHERE ID_1='".$id."' OR ID_2='".$id."'"; // Select the user id from mutal matchs.
		$queryResult = mysqli_query($conn, $query);
        
        if(mysqli_num_rows($queryResult) > 0){// Check if the user exist in mutual matchs table.

            while($row = mysqli_fetch_assoc($queryResult)){// print results

			    if($row['ID_1'] != $id){// Remove user own id
                    echo("ID:".$row['ID_1'].";");

                } else {
                    echo("ID:".$row['ID_2'].";");
                }

			}

        } else {

            echo("No mutal match found for this user");
        }

    }

	// Call the function base on a Post from Unity
	$functionName = $_POST["functionName"];
	switch ($functionName){

		default:
			echo("No function Found");
			break;

		case "CheckMatchs":
			CheckMatchs();
			break;

        case "CheckMutualMatchs":
            CheckMutualMatchs();
            break;
	}

	mysqli_close($conn);
?>