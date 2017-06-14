using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Repository.Application;

public class AppManeger : MonoBehaviour {

	public static AppManeger instance;


	public enum Gender {None,Male,Female};
	public enum Habit{None,Day,Night};
	public static bool isLoadingData{ get; set;} // use to Set the Loading Panel on and off.

	#region UserData
	public string userID;
	public string userName;
	public int userAge;
	public Gender userGender;
	#endregion

	#region UserPrefsData
	public List<Gender> wantToMeet = new List<Gender>();
	public int[] wantAge = new int[2];
	public bool isSmoke;
	public Habit yourHabit;
	#endregion

	private float waitForCheckDatabase = 300f; // 5 mins

	void Awake (){

		if (instance == null) {

			DontDestroyOnLoad (gameObject);
			instance = this;

		} else if (instance != this) {

			Destroy (gameObject);

		}
	}

	void Start(){

		isLoadingData = false;
		StartCoroutine ("CheckForMatchs");

	}

	/// <summary>
	/// Sets the users prefs into the global variables in AppManeger and save it. 
	/// Please, enter with a dictonary with the keys: 
	/// {Smoke, MinAge, MaxAge, Habit, WantToMeetMan, WantToMeetWoman}.
	/// </summary>
	/// <param name="dic">Dictionary</param>
	static public void SetUsersPrefsVariables(Dictionary<string,string> dic){

		AppManeger.instance.wantToMeet = new List<Gender> (); // reset list

		// ====================== Add the results into the global vars ========================================= 
		if (Convert.ToBoolean(dic ["WantToMeetMan"]))
			AppManeger.instance.wantToMeet.Add (Gender.Male);
		

		if (Convert.ToBoolean (dic ["WantToMeetWoman"]))
			AppManeger.instance.wantToMeet.Add (Gender.Female);
		

		AppManeger.instance.wantAge [0] = Convert.ToInt32(dic["MinAge"]);
		AppManeger.instance.wantAge [1] = Convert.ToInt32(dic["MaxAge"]);
		AppManeger.instance.isSmoke = Convert.ToBoolean(dic["Smoke"]);
		AppManeger.instance.yourHabit = (Habit) Enum.Parse((typeof(Habit)), dic["Habit"]);
		// ===================================================================================================

		//-------------- Save user prefs data on the phone ----------------------------------
		string fileName = string.Format("Prefs_{0}",AppManeger.instance.userID);
		SaveLoadData.Save (fileName,SaveLoadData.DataType.UserPrefs);
		//-----------------------------------------------------------------------------

	}

	/// <summary>
	/// Sets the user variables. Enter the dictonary with the keys: {ID, Name, Age, Gender} or {Name, Age, Gender}
	/// </summary>
	/// <param name="dic">Dictionary</param>
	static public void SetUserVariables(Dictionary<string,string> dic){

		// ====================== Add the results into the global vars ========================================= 
		if(dic.ContainsKey("ID"))
			AppManeger.instance.userID = dic["ID"];
		
		AppManeger.instance.userName = dic["Name"];
		AppManeger.instance.userAge = Convert.ToInt32(dic["Age"]);
		AppManeger.instance.userGender = (Gender) Enum.Parse((typeof(Gender)), dic["Gender"]);
		// ===================================================================================================

	}

	IEnumerator CheckForMatchs(){

		while (true) {

			yield return new WaitForSeconds (waitForCheckDatabase);
			FindCheers.CheckForMatchs (userID);
		}

	}
		

}
