using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadData{

	public enum DataType{UserPrefs};

	/// <summary>
	/// Files the exits.
	/// </summary>
	/// <returns><c>true</c>, if exits, <c>false</c> otherwise.</returns>
	/// <param name="fileName">File name.</param>
	public static bool FileExits(string fileName){

		if (File.Exists (Application.persistentDataPath + "/" + fileName)) {
			return true;
		} else {
			return false;
		}
	}

	#region Game Save and Load methos
	/// <summary>
	/// Save the specified fileName and dataType.
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="dataType">Data type.</param>
	public static void Save(string fileName, DataType dataType){

		switch (dataType) {

		default:
			Debug.Log (string.Format ("No data of the type {0} was found", dataType));
			break;

		case DataType.UserPrefs:
			SavePrefs (fileName);
			break;

		}
	}

	private static void SavePrefs(string fileName){

		BinaryFormatter bf = new BinaryFormatter (); //create a binary formart
		FileStream file = File.Create (Application.persistentDataPath + "/" + fileName); //create a file called SavaData.dat

		UserPrefsData data = new UserPrefsData(); // Create a data object using a serializable class

		//-------- pass the Prefs values to data -------------
		data.wantToMeet = new List<AppManeger.Gender>();
		data.wantAge = new int[2];
		data.wantToMeet = AppManeger.instance.wantToMeet;
		data.wantAge = AppManeger.instance.wantAge;
		data.isSmoke = AppManeger.instance.isSmoke;
		data.yourHabit = AppManeger.instance.yourHabit;
		//----------------------------------------------------

		bf.Serialize (file, data); //write data on file
		file.Close();
		Debug.Log ("Prefs Saved");
	}
	/// <summary>
	/// Load the specified fileName and dataType.
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="dataType">Data type.</param>
	public static void Load(string fileName, DataType dataType){

		switch (dataType) {

		default:
			Debug.Log (string.Format ("No data of the type {0} was found", dataType));
			break;

		case DataType.UserPrefs:
			LoadPrefs (fileName);
			break;

		}
	}

	private static void LoadPrefs(string fileName){

		if (File.Exists (Application.persistentDataPath + "/" + fileName)) {

			BinaryFormatter bf = new BinaryFormatter (); //create a binary formart
			FileStream file = File.Open (Application.persistentDataPath + "/" + fileName, FileMode.Open); //open the file called SavaData.dat

			UserPrefsData data = (UserPrefsData)bf.Deserialize (file); //Gets the data and put on the data variable
			file.Close ();

			//----------- Store data into Prefs vars --------------
			AppManeger.instance.wantToMeet = data.wantToMeet;
			AppManeger.instance.wantAge = data.wantAge;
			AppManeger.instance.isSmoke = data.isSmoke;
			AppManeger.instance.yourHabit = data.yourHabit;
			//-----------------------------------------------------

			Debug.Log ("Prefs data Load");

		} else {

			Debug.Log ("Prefs save data doesn't exit");
		}

	}
	#endregion
}

// This class will hold the information to be saved. It has to be serializable
[Serializable]
class UserPrefsData{

	public List<AppManeger.Gender> wantToMeet;
	public int[] wantAge;
	public bool isSmoke;
	public AppManeger.Habit yourHabit;
}
