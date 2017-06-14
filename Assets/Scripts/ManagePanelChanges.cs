using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Repository.Application;

public class ManagePanelChanges : MonoBehaviour {

	public GameObject enterPanel;
	public GameObject setSexAndAgePanel;
	public GameObject setSmokeAndHabitPanel;
	public GameObject cheersPanel;
	public GameObject settingsPanel;

	private static ManagePanelChanges managePanelChanges;

	public static ManagePanelChanges Instance(){

		if (!managePanelChanges) {

			managePanelChanges = FindObjectOfType (typeof(ManagePanelChanges)) as ManagePanelChanges;
			if (!managePanelChanges) // Make sure that Modalpanel existes.
				Debug.Log ("There needs to be one active ManagePanelChanges Script on a GameObject in your scene");
		}

		return managePanelChanges;
	}

	//============ For Test only. This must come from FaceBook =================
	string userName = "Thiago Junqueira";
	string email = "junqfisica@hotmail.com";
	string age = "31";
	string gender = "Male"; // used only on the first time. The user will choose it after.
	//============================================

	void Start(){

		enterPanel.SetActive (true);
		setSexAndAgePanel.SetActive (false);
		setSmokeAndHabitPanel.SetActive(false);
		cheersPanel.SetActive (false);
		settingsPanel.SetActive (false);
	}

	public void FaceBookLogin(){


		GetMethods.GetUserId (email ,(id) => {

			if (id != "0"){// The user exits in the data base

				// Verify if an Update is necessary.
				UpdateMethods.UpdateUsersDatabase(id,userName,age,AppManeger.instance.userGender.ToString(), (result)=>{
					Debug.Log(string.Format("Update {0}",result));
				});

				string fileName = string.Format("Prefs_{0}",AppManeger.instance.userID);

				//GetMethods.GetUserPrefs(id, (prefsResult) => {PrefsResult(prefsResult);}); // Check if the user are in the Prefs Database

				if (SaveLoadData.FileExits(fileName)){// If exit 

					SaveLoadData.Load(fileName,SaveLoadData.DataType.UserPrefs); // load from device
					GoToCheersPanel (); // Means the user has everyting setted;
					
				} else {

					GetMethods.GetUserPrefs(id, (prefsResult) => {PrefsResult(prefsResult);}); // Check if the user are in the Prefs Database
					
				}


			} else {// User don't exit in the data base
				
				PostMethods.InsertUserIntoUsersDatabase(userName,age,email,gender, (result)=>{
					InsertUserResult(result);
				});

			}

		});
	
	}

	void InsertUserResult(string result){

		if (result == "Success" || result == "success") {// Finish to insert user into data base

			Debug.Log (string.Format("Users was {0} insert into users database", result));
			GetMethods.GetUserId (email, (id) => {

				if (id != "0") {// The user exits in the data base

					AppManeger.instance.userID = id;
					GetMethods.GetUserPrefs (id, (prefsResult) => {
						PrefsResult (prefsResult);
					}); // Check if the user are in the Prefs Database

				}
			});

		} else {

			Debug.Log (result);
		}

	}

	void PrefsResult(Dictionary<string,string> result){

		if (result.ContainsKey("Error")) { // means that this id is not within the userprefs dataBase.

			Debug.Log ("User need to be insert into UsersPrefs " + result["Error"]);
			SetSexAndAgePanelOn ();
		
		} else if (result.ContainsKey("ID")) {
			
			AppManeger.SetUsersPrefsVariables (result); // set the results into the global variables.

			GoToCheersPanel (); // Means the user has everyting setted;

		} else {// For safety in the case of conection failure;

			Debug.Log ("Conection error trying to get UserPrefsData");
		}
			
	}

	public void SetSexAndAgePanelOn(){

		enterPanel.SetActive (false);
		cheersPanel.SetActive (false);
		setSmokeAndHabitPanel.SetActive(false);
		settingsPanel.SetActive (false);
		setSexAndAgePanel.SetActive (true);
	}

	public void SetSmokeAndHabitPanelOn(){

		enterPanel.SetActive (false);
		setSexAndAgePanel.SetActive (false);
		cheersPanel.SetActive (false);
		settingsPanel.SetActive (false);
		setSmokeAndHabitPanel.SetActive(true);

	}

	public void GoToCheersPanel(){

		enterPanel.SetActive (false);
		setSexAndAgePanel.SetActive (false);
		setSmokeAndHabitPanel.SetActive(false);
		cheersPanel.SetActive (true);
		settingsPanel.SetActive (true);

	}

}
