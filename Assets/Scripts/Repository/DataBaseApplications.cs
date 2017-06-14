using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Repository.Data;

namespace Repository.Application{

	#region Obsolet
	/*
	public interface IUsers{// It securey that the class must use it.

		string id{ get; set;}
		string name{ get; set;}
		int age{ get; set;}
		string email{ get; set;}
		string gender{ get; set;}
		bool smoke{ get; set;}
		int minAge{ get; set;}
		int maxAge{ get; set;}
		string habit{ get; set;}
		bool wantToMeetMan{ get; set;}
		bool wantToMeetWoman{ get; set;}

	}


	/// <summary>
	/// Get user using their ids numbers.
	/// </summary>
	public class GetUserFromId: IUsers{
		//======== From UsersData ===================
		public string id{get;set;}
		public string name{ get; set;}
		public int age{ get; set;}
		public string email{ get; set;}
		public string gender{ get; set;}
		//======= From UsersPrefsData ===============
		public bool smoke{ get; set;}
		public int minAge{ get; set;}
		public int maxAge{ get; set;}
		public string habit{ get; set;}
		public bool wantToMeetMan{ get; set;}
		public bool wantToMeetWoman{ get; set;}

		public static bool isFinish{ get{

				if(UsersData.usersData.isConstrucDic || UsersPrefsData.usersPrefsData.isConstrucDic){
					return false;
				} else {
					return true;
				} 
			
			}
		}
		/// <summary>
		/// Gets the number of users.
		/// </summary>
		/// <value>The number of users.</value>
		public static int numberOfUsers{get{ 

				return UsersData.usersData.dUsers.Count;
			}
		}
			
		delegate bool IsFinish();
		//======================================================
		/// <summary>
		/// Initializes a new instance of the <see cref="Repository.Users.Ids.Users"/> class.
		/// </summary>
		/// <param name="id">Identifier.</param>
		//======================================================
		public GetUserFromId(string id){

			IsFinish isFinishDelegate;
			isFinishDelegate = CheckIfDataIsReady;
			bool result = isFinishDelegate ();

			if (result) {
				Dictionary<string,object> userId = Id (id,UsersData.usersData.dUsers);
				name = userId ["Name"].ToString();
				age =  Convert.ToInt32(userId ["Age"]);
				email = userId ["Email"].ToString ();
				gender = userId["Gender"].ToString();

				Dictionary<string,object> userPrefsId = Id (id, UsersPrefsData.usersPrefsData.dUsers);
				smoke = Convert.ToBoolean(userPrefsId["Smoke"]);
				minAge = Convert.ToInt32(userPrefsId["MinAge"]);
				maxAge = Convert.ToInt32(userPrefsId["MaxAge"]);
				habit = userPrefsId ["Habit"].ToString ();
				wantToMeetMan = Convert.ToBoolean(userPrefsId["WantToMeetMan"]);
				wantToMeetWoman = Convert.ToBoolean (userPrefsId ["WantToMeetWoman"]);

			} 
		}

		bool CheckIfDataIsReady(){

			if(UsersData.usersData.isConstrucDic || UsersPrefsData.usersPrefsData.isConstrucDic){
				return false;
			} else{
				return true;
			}
		}

		//============================================================================
		/// <summary>
		/// Identifier the specified id and listOfdic.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="listOfdic">List ofdic.</param>
		//============================================================================
		Dictionary<string,object> Id(string id,List<Dictionary<string,object>> listOfdic){


			foreach (Dictionary<string,object> dic in listOfdic) {

				if (dic.ContainsValue (id)) {
					return dic;
				}
			}

			Debug.Log ("The id " + id + " don't exist in the database");
			return null;
		}
	}*/
	#endregion

	public static class GetMethods{


		/// <summary>
		/// Gets the user's id and return it. Also, set all the user's data into 
		/// AppManeger, if, the user exist in the database. Otherwise, only returns id = 0.
		/// </summary>
		/// <param name="email">Email.</param>
		/// <param name="callback">Callback.</param>
		public static void GetUserId(string email, System.Action<string> callback){

			AppManeger.isLoadingData = true; // processing data
			WWWForm form = new WWWForm ();
			form.AddField ("emailPost", email);
			form.AddField("functionName", "GetUserId");
			WWW www = new WWW(Urls.getMethodsUrl, form);
			UtilMethods.util.GetData (www, (myvalue) => {

				string[] dicIndex = DataStructure.userDataStructure;
				List<Dictionary<string,string>> dic = new List<Dictionary<string, string>>();
				dic = ConstructListOfDictionary(myvalue,dicIndex); // This list contains only one row

				//===== Find and get the id if exist =====
				string id = "0"; //default valeu 

				if(dic[0].ContainsKey("ID")){
					dic[0].TryGetValue("ID", out id);
					AppManeger.SetUserVariables(dic[0]); // Send the dic to set on the User variables.

				}

				//=========================================
				AppManeger.isLoadingData = false; //finish processing data 
				callback(id); // Return only the id

			});
		}

		/// <summary>
		/// Gets the user preferences. Returns a callback that contains a dictionary with the 
		/// keys {ID,Smoke,MinAge,MaxAge,Habit,WantToMeetMan,WantToMeetWoman}.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void GetUserPrefs(string id, System.Action<Dictionary<string,string>> callback){

			AppManeger.isLoadingData = true; // processing data
			WWWForm form = new WWWForm ();
			form.AddField ("idPost", id);
			form.AddField("functionName", "GetUserPrefs");
			WWW www = new WWW(Urls.getMethodsUrl, form);
			UtilMethods.util.GetData (www, (myvalue) => {

				string[] dicIndex = DataStructure.prefsDataStructure;
				List<Dictionary<string,string>> dic = new List<Dictionary<string, string>>(); 
				dic = ConstructListOfDictionary(myvalue,dicIndex); // This list contains only one row
				AppManeger.isLoadingData = false; //finish processing data
				callback(dic[0]);

			});
		}

		/// <summary>
		/// Get all the users from user and userPrefs Database who match the filters. 
		/// Returns a callback that contains a list of dictionary with the 
		/// keys {ID,Name,Age,Smoke,Habit}.
		/// </summary>
		/// <param name="wantToMeet">Want to meet {Man, Woman or Both}.</param>
		/// <param name="agesBetween">Ages between {min, max}.</param>
		/// <param name="excludeUserId">Exclude this user id.</param>
		/// <param name="callback">Callback.</param>
		public static void GetUsersByFilter(List<AppManeger.Gender> wantToMeet,  int[] agesBetween, string excludeUserId, 
			System.Action<List<Dictionary<string,string>>> callback){

			AppManeger.isLoadingData = true; // processing data
			bool wantBothGender = false;
			if (wantToMeet.Count > 1)
				wantBothGender = true;

			WWWForm form = new WWWForm ();
			form.AddField ("minAgePost", System.Convert.ToString(agesBetween[0]));
			form.AddField ("maxAgePost", System.Convert.ToString(agesBetween[1]));
			form.AddField ("genderPost", System.Convert.ToString( wantToMeet[0]));
			form.AddField ("excludeIdPost", excludeUserId);
			form.AddField ("bothGenderPost", wantBothGender.ToString ());
			form.AddField ("functionName", "SelectUsersByFilter");

			WWW www = new WWW (Urls.getMethodsUrl, form);
			UtilMethods.util.DisplayWWWDataResult (www);

			UtilMethods.util.GetData (www, (result) => {

				string[] dicIndex = {"ID","Name","Age","Smoke","Habit"};
				AppManeger.isLoadingData = false; //finish processing data 
				callback(ConstructListOfDictionary(result,dicIndex));

			});

		}

		/// <summary>
		/// Selects users who select me.Returns a callback that contains a list 
		/// of dictionary with the keys {ID,Name,Age,Smoke,Habit}.
		/// </summary>
		/// <param name="wantToMeet">Want to meet.</param>
		/// <param name="agesBetween">Ages between.</param>
		/// <param name="userId">User id.</param>
		/// <param name="callback">Callback.</param>
		public static void SelectWhoSelectMe(List<AppManeger.Gender> wantToMeet,  int[] agesBetween, string userId, 
			System.Action<List<Dictionary<string,string>>> callback){

			AppManeger.isLoadingData = true; // processing data
			bool wantBothGender = false;
			if (wantToMeet.Count > 1)
				wantBothGender = true;

			WWWForm form = new WWWForm ();
			form.AddField ("myIdPost", userId);
			form.AddField ("minAgePost", System.Convert.ToString(agesBetween[0]));
			form.AddField ("maxAgePost", System.Convert.ToString(agesBetween[1]));
			form.AddField ("genderPost", System.Convert.ToString( wantToMeet[0]));
			form.AddField ("bothGenderPost", wantBothGender.ToString ());
			form.AddField ("functionName", "SelectWhoSelectMe");

			WWW www = new WWW (Urls.getMethodsUrl, form);
			UtilMethods.util.DisplayWWWDataResult (www);

			UtilMethods.util.GetData (www, (result) => {

				string[] dicIndex = {"ID","Name","Age","Smoke","Habit"};
				AppManeger.isLoadingData = false; //finish processing data 
				callback(ConstructListOfDictionary(result,dicIndex));

			});

		}

		static List<Dictionary<string,string>> ConstructListOfDictionary(string result, string[] index){

			List<Dictionary<string,string>> returnDic = new List<Dictionary<string,string>> ();

			if (result == "0") {  // Means there is no data

				Dictionary<string,string> dic = new Dictionary<string,string> ();
				dic.Add ("Error", "No data Avaible");
				returnDic.Add (dic);
				return returnDic;

			} else { // Otherwise construct the dictionary

				string[] splitLinesResult = result.Split (new char[]{ ';' });

				foreach (string s in splitLinesResult) {

					if (!string.IsNullOrEmpty (s)) {// avoid possible blank lines

						Dictionary<string,string> dic = new Dictionary<string, string> ();
						foreach (string sIndex in index) {

							string value = s.Substring (s.IndexOf (sIndex) + sIndex.Length + 1);
							if (value.Contains ("|"))
								value = value.Remove (value.IndexOf ("|"));

							dic.Add (sIndex, value);

						}

						returnDic.Add (dic);

					}

				}

				return returnDic;
			}
		}

	}

	/// <summary>
	/// Class that contains the Post methods.
	/// </summary>
	public static class PostMethods{

		/// <summary>
		/// Inserts the new user on the user DataBase.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="age">Age.</param>
		/// <param name="email">Email.</param>
		/// <param name="gender">Gender.</param>
		/// <param name="callback">Callback.</param>
		public static void InsertUserIntoUsersDatabase(string name, string age, string email, string gender,
			System.Action<string> callback){

			WWWForm form = new WWWForm ();
			form.AddField ("namePost", name);
			form.AddField ("agePost", age);
			form.AddField ("emailPost", email);
			form.AddField ("genderPost", gender);
			form.AddField ("functionName", "PostToUsersDatabase");

			WWW www = new WWW (Urls.insertUserUrl,form);
			UtilMethods.util.GetData(www, (data) => {callback(data);
			});
		}

		/// <summary>
		/// Inserts the user into users prefs database.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="smoke">Smoke.</param>
		/// <param name="minAge">Minimum age.</param>
		/// <param name="maxAge">Max age.</param>
		/// <param name="habit">Habit.</param>
		/// <param name="wantToMeetMan">Want to meet man.</param>
		/// <param name="wantToMeetWoman">Want to meet woman.</param>
		/// <param name="callback">Callback.</param>
		public static void InsertUserIntoUsersPrefsDatabase(string id, string smoke, string minAge, string maxAge,
			string habit, string wantToMeetMan, string wantToMeetWoman, System.Action<string> callback){

			WWWForm form = new WWWForm ();
			form.AddField("idPost", id);
			form.AddField("smokePost",smoke);
			form.AddField("minAgePost",minAge);
			form.AddField("maxAgePost",maxAge);
			form.AddField("habitPost", habit);
			form.AddField("wantManPost", wantToMeetMan);
			form.AddField("wantWomanPost", wantToMeetWoman);
			form.AddField ("functionName", "PostToUsersPrefsDatabase");

			WWW www = new WWW (Urls.insertUserUrl,form);
			UtilMethods.util.GetData(www, (data) => {callback(data);
			});
		}

		/// <summary>
		/// Inserts the cheers into matchs database.
		/// </summary>
		/// <param name="chooserId">Chooser identifier.</param>
		/// <param name="choosenId">Choosen identifier.</param>
		/// <param name="cheers">Cheers.</param>
		/// <param name="callback">Callback.</param>
		public static void InsertCheersIntoMatchsDatabase(string chooserId, string choosenId, string cheers,
			System.Action<string> callback){

			WWWForm form = new WWWForm ();
			form.AddField("chooserPost", chooserId);
			form.AddField("chosenPost",choosenId);
			form.AddField("cheersPost",cheers);
			form.AddField ("functionName", "PostToMatchsDatabase");

			WWW www = new WWW (Urls.insertUserUrl,form);
			UtilMethods.util.GetData(www, (data) => {callback(data);
			});
		}

	}

	public static class UpdateMethods{

		/// <summary>
		/// Verify and updates the users database and AppManeger data if necessary. Enter with the user id for identification. 
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="updateName">Update name.</param>
		/// <param name="updateAge">Update age.</param>
		/// <param name="updateGender">Update gender.</param>
		/// <param name="callback">Callback.</param>
		public static void UpdateUsersDatabase(string id,string updateName, string updateAge, string updateGender,
			System.Action<string> callback){

			bool update = false;
			bool nameUp = string.Equals (updateName, AppManeger.instance.userName, StringComparison.OrdinalIgnoreCase);
			bool ageUp = string.Equals (updateAge, AppManeger.instance.userAge.ToString(), StringComparison.OrdinalIgnoreCase);
			bool genderUp = string.Equals (updateGender, AppManeger.instance.userGender.ToString(), StringComparison.OrdinalIgnoreCase);

			if (!nameUp|| !ageUp || !genderUp) update = true;

			if (update) {

				WWWForm form = new WWWForm ();
				form.AddField ("idPost", id);
				form.AddField ("namePost", updateName);
				form.AddField ("agePost", updateAge);
				form.AddField ("genderPost", updateGender);
				form.AddField ("functionName", "UpdateToUsersDatabase");

				WWW www = new WWW (Urls.updatedataUrl,form);
				UtilMethods.util.GetData(www, (data) => {
					if (data == "Success"){

						// Converts the first letter to Captal, to avoid errors
						updateGender = char.ToUpper(updateGender[0]) + updateGender.Substring(1); 
						Dictionary<string,string> dic = new Dictionary<string, string>();
						//keys: {Name, Age, Gender}
						dic.Add("Name", updateName);
						dic.Add("Age", updateAge);
						dic.Add("Gender", updateGender);

						AppManeger.SetUserVariables(dic);
					}

					callback(data);
				
				});

			} else {

				callback ("not necessary");
			}
			
		}

		/// <summary>
		///  Verify and updates the users prefs database and AppManeger data if necessary. Enter with the user id for identification.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="smoke">Smoke.</param>
		/// <param name="minAge">Minimum age.</param>
		/// <param name="maxAge">Max age.</param>
		/// <param name="habit">Habit.</param>
		/// <param name="wantToMeetMan">true/false.</param>
		/// <param name="wantToMeetWoman">true/false.</param>
		/// <param name="callback">Callback.</param>
		public static void UpdateUsersPrefsDatabase(string id, string updateSmoke, string updateMinAge, string updateMaxAge,
			string updateHabit, string updateWantToMeetMan, string updateWantToMeetWoman, System.Action<string> callback){

			bool update = false;

			bool smokeNotUp = string.Equals (updateSmoke, AppManeger.instance.isSmoke.ToString(), StringComparison.OrdinalIgnoreCase);
			bool minAgeNotUp = string.Equals (updateMinAge, AppManeger.instance.wantAge[0].ToString(), StringComparison.OrdinalIgnoreCase);
			bool maxAgeNotUp = string.Equals (updateMaxAge, AppManeger.instance.wantAge[1].ToString(), StringComparison.OrdinalIgnoreCase);
			bool habitNotUp = string.Equals (updateHabit, AppManeger.instance.yourHabit.ToString(), StringComparison.OrdinalIgnoreCase);
			bool manNotUp = true;   // Assume that there was no change in this status
			bool womanNotUp = true; // Assume that there was no change in this status
			if (AppManeger.instance.wantToMeet.Contains (AppManeger.Gender.Female)) {//The list contains woman
				if (!Convert.ToBoolean (updateWantToMeetWoman))
					womanNotUp = false;
			} else {
				if (Convert.ToBoolean (updateWantToMeetWoman))
					womanNotUp = false;
			}
				
			if (AppManeger.instance.wantToMeet.Contains (AppManeger.Gender.Male)) {//The list contains man
				if (!Convert.ToBoolean (updateWantToMeetMan))
					manNotUp = false;
			} else {
				if (Convert.ToBoolean (updateWantToMeetMan))
					manNotUp = false;
			}
				
			if (!smokeNotUp || !minAgeNotUp || !maxAgeNotUp || !habitNotUp || !manNotUp || !womanNotUp) update = true;

			if (update) {

				WWWForm form = new WWWForm ();
				form.AddField ("idPost", id);
				form.AddField ("smokePost", updateSmoke);
				form.AddField ("minAgePost", updateMinAge);
				form.AddField ("maxAgePost", updateMaxAge);
				form.AddField ("habitPost", updateHabit);
				form.AddField ("wantManPost", updateWantToMeetMan);
				form.AddField ("wantWomanPost", updateWantToMeetWoman);
				form.AddField ("functionName", "UpdateToUsersPrefsDatabase");

				WWW www = new WWW (Urls.updatedataUrl, form);
				UtilMethods.util.GetData (www, (data) => {

					if (data == "Success"){

						Dictionary<string,string> dic = new Dictionary<string, string>();
						//keys: {Smoke, MinAge, MaxAge, Habit, WantToMeetMan, WantToMeetWoman}
						dic.Add("Smoke",updateSmoke);
						dic.Add("MinAge",updateMinAge);
						dic.Add("MaxAge",updateMaxAge);
						dic.Add("Habit", updateHabit);
						dic.Add("WantToMeetMan", updateWantToMeetMan);
						dic.Add("WantToMeetWoman",updateWantToMeetWoman);

						AppManeger.SetUsersPrefsVariables(dic);

					}

					callback (data);

				});
			
			} else {
				
				callback ("not necessary");
			}
		}
	}

	public static class FindCheers{

		public static void CheckForMatchs(string userId){

			WWWForm form = new WWWForm ();
			form.AddField ("idPost", userId);
			form.AddField ("functionName", "CheckMatchs");

			WWW www = new WWW (Urls.checkmatchsUrl, form);
			UtilMethods.util.DisplayWWWDataResult (www);
		}
	}
}

