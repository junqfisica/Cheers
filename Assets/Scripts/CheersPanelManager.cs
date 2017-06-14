using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Repository.Application;

public class CheersPanelManager : MonoBehaviour {

	public GameObject cheersDrink;
	public GameObject fixGlass;
	public GameObject failDrink;
	public GameObject chooseDrink;

	public Text nameText;
	public Text ageText;
	public Image smokeImage;
	public Image habitImage;

	private string choosenId;
	private bool isUsersNotOver;
	private bool ignoreOnGUI;

	private Sprite smokeSprite;
	private Sprite habitSprite;
	private Button cheersBut;
	private Button giveupBut;
	private Animator moveGlassAnimartor;
	private Animator fixGlassAnimator;

	List<Dictionary<string,string>> usersListOfDic{ get; set;}

	void Awake(){

		cheersBut = this.transform.FindChild ("Buttons Panel").FindChild ("CheersBut").GetComponent<Button> ();
		giveupBut = this.transform.FindChild ("Buttons Panel").FindChild ("GiveupBut").GetComponent<Button> ();
		moveGlassAnimartor = chooseDrink.GetComponent<Animator> ();
		fixGlassAnimator = fixGlass.GetComponent<Animator> ();
		ignoreOnGUI = false;
	}

	void OnEnable(){

		SelectUsers ();
		FindCheers.CheckForMatchs (AppManeger.instance.userID); // Run the dateBase to find mutualMatchs

	}

	void OnGUI(){
		
		if(!ignoreOnGUI){
			
			if(isUsersNotOver != cheersBut.interactable){

				cheersBut.interactable = isUsersNotOver;
				giveupBut.interactable = isUsersNotOver;
			}
		}

		#region Drag
		if (cheersBut.interactable) {// If the button is of don't use drag left and right
			
			if (DragController.OnLeftDrag) {
				Debug.Log ("Drag left");
				MoveGlassToLeft ();
			} else if (DragController.OnRightDrag) {
				Debug.Log ("Drag right");
				MoveGlassToRight ();
			}
		}
		#endregion

		if (DragController.OnUpDrag) {
			Debug.Log ("Drag UP");
		} else if(DragController.OnDownDrag) {
			Debug.Log ("Drag Dow");
		}
	}

	void SelectUsers(){

		usersListOfDic = new List<Dictionary<string,string>>();

		GetMethods.GetUsersByFilter(AppManeger.instance.wantToMeet,AppManeger.instance.wantAge,AppManeger.instance.userID, 
			(listResults)=>{

				if(listResults[0].ContainsKey("Error")){

					Debug.Log(listResults[0]["Error"]);

					isUsersNotOver = false;
					// === Clean info Panel===
					nameText.text = "Nome";
					ageText.text = "";
					//=========================

				} else {

					GetMethods.SelectWhoSelectMe(AppManeger.instance.wantToMeet,AppManeger.instance.wantAge,AppManeger.instance.userID, 
						(listResultsSelectMe)=>{

							if(listResultsSelectMe[0].ContainsKey("Error")){
								Debug.Log(listResultsSelectMe[0]["Error"] + ". No one selected me");

								// Show other users
								usersListOfDic = listResults;
								DisplayUsers();

							} else {

								// Remove users from listResults that are also in listResultsSelectMe
								//ps: ListResultsSelectMe is always <= listResults
								foreach(Dictionary<string,string> dic in listResultsSelectMe){
									int i = 0;
									foreach(Dictionary<string,string> dic2 in listResults){ 

										if (dic["ID"] == dic2["ID"]){// same id remove from he listResults
											listResults.RemoveAt(i);
											break;
										}
										i++;
									}
								}

								listResults.AddRange(listResultsSelectMe); //Add the users back
								usersListOfDic = listResults;
								DisplayUsers();
							}
						}
					);// End of GetMethods.SelectWhoSelectMe

				}

			}
		); // End of GetMethods.GetUsersByFilter
			
	}

	void DisplayUsers(){

		int numberOfUsers = usersListOfDic.Count;

		if (numberOfUsers == 0) {

			SelectUsers ();
		
		} else {

			isUsersNotOver = true;
			int showIndex = Random.Range (0, numberOfUsers);

			bool isSmoke = System.Convert.ToBoolean(usersListOfDic [showIndex] ["Smoke"]);
			string habitToString = usersListOfDic [showIndex]["Habit"].ToString ();
			AppManeger.Habit habit = (AppManeger.Habit) System.Enum.Parse((typeof(AppManeger.Habit)), habitToString);
			choosenId = usersListOfDic [showIndex] ["ID"]; // The id of the user who is been displayed.

			nameText.text = usersListOfDic [showIndex]["Name"];
			ageText.text = usersListOfDic [showIndex] ["Age"];

			if (isSmoke) {
				smokeSprite = Resources.Load<Sprite> ("Sprites/Icons/smoke_color");
				smokeImage.sprite = smokeSprite;

			} else {

				smokeSprite = Resources.Load<Sprite> ("Sprites/Icons/no_smoke_color");
				smokeImage.sprite = smokeSprite;
			}

			if (habit == AppManeger.Habit.Day) {
				habitSprite = Resources.Load<Sprite> ("Sprites/Icons/day_color");
				habitImage.sprite = habitSprite;

			} else if (habit == AppManeger.Habit.Night) { 
				habitSprite = Resources.Load<Sprite> ("Sprites/Icons/night_color");
				habitImage.sprite = habitSprite;

			} else {
				habitImage.sprite = null;
			}

			usersListOfDic.RemoveAt (showIndex); // Remove from userList

		}
			
	}

	public void MoveGlassToRight(){
		
		PostMethods.InsertCheersIntoMatchsDatabase (AppManeger.instance.userID, choosenId, "true", (result) => {
				Debug.Log(result);
		});

		moveGlassAnimartor.SetTrigger ("goRight"); // trigger animation to right
		fixGlassAnimator.SetBool("rotate",true); //trigger rotation on the fix glass

		StartCoroutine ("FindMoreCheers");

	}

	public void MoveGlassToLeft(){
		
		PostMethods.InsertCheersIntoMatchsDatabase (AppManeger.instance.userID, choosenId, "false", (result) => {
			Debug.Log(result);
		});

		moveGlassAnimartor.SetTrigger ("goLeft"); // trigger animation to left

		StartCoroutine ("FindMoreCheers");
	}

	IEnumerator FindMoreCheers(){
		// ==== Turn off the buttons ======
		ignoreOnGUI = true;
		cheersBut.interactable = false;
		giveupBut.interactable = false;
		//=================================

		Debug.Log ("Find More Cheers");

		yield return new WaitForSeconds (2f);

		if (usersListOfDic.Count == 0) {
			DisplayUsers ();
			yield return new WaitForSeconds (1f);
		} else {
			yield return new WaitForSeconds (1f);
			DisplayUsers ();
		}
			
		fixGlassAnimator.SetBool("rotate",false);
		chooseDrink.SetActive (true);
		failDrink.SetActive (false);
		cheersDrink.SetActive (false);

		// ==== Turn On the buttons ======
		ignoreOnGUI = false;
		cheersBut.interactable = true;
		giveupBut.interactable = true;
		//=================================

	}
}
