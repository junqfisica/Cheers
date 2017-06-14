using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Repository.Application;

public class SexAndAge : MonoBehaviour {

	public Text ageText;
	public Slider minAgeSlider;
	public Slider maxAgeSlider;
	public Button yourSexManBut;
	public Button yourSexWomanBut;
	public Button wantMeetWomanBut;
	public Button wantMeetManBut;
	public Button continueBut;

	// ================== temp variable =========================
	private AppManeger.Gender myGenderChoice;
	private List<AppManeger.Gender> wantToMeet = new List<AppManeger.Gender>();
	private int[] wantAge = new int[2];
	//==========================================================

	private float deltaAge = 3f;
	private Sprite manColor;
	private Sprite womanColor;
	private Sprite man;
	private Sprite woman;

	private ManagePanelChanges managePanelChanges;

	void Awake(){

		manColor = Resources.Load<Sprite> ("Sprites/Icons/Man_color");
		man = Resources.Load<Sprite> ("Sprites/Icons/Man");
		womanColor = Resources.Load<Sprite> ("Sprites/Icons/Woman_color");
		woman = Resources.Load<Sprite> ("Sprites/Icons/Woman");
		minAgeSlider.value = minAgeSlider.minValue;
		maxAgeSlider.value = maxAgeSlider.minValue;

		managePanelChanges = ManagePanelChanges.Instance ();
	}

	void OnEnable(){
		
		string fileName = string.Format("Prefs_{0}",AppManeger.instance.userID);

		if (SaveLoadData.FileExits (fileName)) {// If exit, user already used it before.

			Text contButText = continueBut.GetComponentInChildren<Text> ();
			contButText.text = "Salvar";
			OnChooseSex (YourSexIsMale,YourSexIsFemale,WantToMeetMan,WantToMeetWoman,UpdatePrefs);
			SetInitialConditions (); 
		
		} else {//Otherwise, means the user is using it for the first time

			OnChooseSex (YourSexIsMale,YourSexIsFemale,WantToMeetMan,WantToMeetWoman,Continue);
			AppManeger.instance.wantAge [0] = (int)minAgeSlider.value;
			AppManeger.instance.wantAge [1] = -1 * (int)maxAgeSlider.value;
		}

	}

	void SetInitialConditions(){

		//---------------------- Your Gender ------------------------------
		myGenderChoice = AppManeger.instance.userGender;
		if (AppManeger.instance.userGender == AppManeger.Gender.Male) {

			yourSexManBut.image.sprite = manColor;
			yourSexWomanBut.image.sprite = woman;
		
		} else {

			yourSexManBut.image.sprite = man;
			yourSexWomanBut.image.sprite = womanColor;
		}

		//------------------------ Want man/woman -------------------------
		wantToMeet = new List<AppManeger.Gender>(); // reset the local list
		if (AppManeger.instance.wantToMeet.Contains (AppManeger.Gender.Male)) {
			wantMeetManBut.image.sprite = manColor;
			wantToMeet.Add (AppManeger.Gender.Male);
		} else {
			wantMeetManBut.image.sprite = man;
		}

		if (AppManeger.instance.wantToMeet.Contains (AppManeger.Gender.Female)) {
			wantMeetWomanBut.image.sprite = womanColor;
			wantToMeet.Add (AppManeger.Gender.Female);
		} else {
			wantMeetWomanBut.image.sprite = woman;
		}
			

		//------------------- From Ages -------------------------------
		minAgeSlider.value = (float)AppManeger.instance.wantAge [0];
		maxAgeSlider.value = -1f*(float)AppManeger.instance.wantAge [1];
	}

	void OnGUI(){

		if (wantToMeet.Count != 0 && myGenderChoice != AppManeger.Gender.None) { // Check if all fields were filled

			continueBut.interactable = true;

		} else {

			continueBut.interactable = false;
		}

	}

	public void OnAgeChandeLeftToRight(){

		minAgeSlider.value = Mathf.Clamp(minAgeSlider.value, minAgeSlider.minValue,Mathf.Abs(maxAgeSlider.value) - deltaAge);
		ChandeAgeText ();
		wantAge [0] = (int) minAgeSlider.value;
	}

	public void OnAgeChandeRightToLeft(){

		maxAgeSlider.value = Mathf.Clamp(maxAgeSlider.value, maxAgeSlider.minValue,-1f*(minAgeSlider.value + deltaAge));
		ChandeAgeText ();
		wantAge [1] = -1 * (int)maxAgeSlider.value;

	}

	void OnChooseSex(UnityAction Event1, UnityAction Event2, UnityAction Event3, UnityAction Event4,UnityAction Event5){

		yourSexManBut.onClick.RemoveAllListeners ();
		yourSexManBut.onClick.AddListener (Event1);

		yourSexWomanBut.onClick.RemoveAllListeners ();
		yourSexWomanBut.onClick.AddListener (Event2);

		wantMeetManBut.onClick.RemoveAllListeners ();
		wantMeetManBut.onClick.AddListener (Event3);

		wantMeetWomanBut.onClick.RemoveAllListeners ();
		wantMeetWomanBut.onClick.AddListener (Event4);

		continueBut.onClick.RemoveAllListeners ();
		continueBut.onClick.AddListener (Event5);
	}

	void YourSexIsMale(){

		yourSexManBut.image.sprite = manColor;
		yourSexWomanBut.image.sprite = woman;
		myGenderChoice = AppManeger.Gender.Male;
	}

	void YourSexIsFemale(){

		yourSexManBut.image.sprite = man;
		yourSexWomanBut.image.sprite = womanColor;
		myGenderChoice = AppManeger.Gender.Female;
	}

	void WantToMeetWoman(){

		if (wantToMeet.Contains (AppManeger.Gender.Female)) {
			wantToMeet.Remove (AppManeger.Gender.Female);
			wantMeetWomanBut.image.sprite = woman;
		} else {
			wantToMeet.Add (AppManeger.Gender.Female);
			wantMeetWomanBut.image.sprite = womanColor;
		}

	}

	void WantToMeetMan(){

		if (wantToMeet.Contains (AppManeger.Gender.Male)) {
			wantToMeet.Remove (AppManeger.Gender.Male);
			wantMeetManBut.image.sprite = man;
		} else {
			wantToMeet.Add (AppManeger.Gender.Male);
			wantMeetManBut.image.sprite = manColor;
		}

	}

	void ChandeAgeText(){

		if (maxAgeSlider.value == -60f) {
			ageText.text = string.Format ("{0} - {1}+", minAgeSlider.value, -1f*maxAgeSlider.value);
		} else {
			ageText.text = string.Format ("{0} - {1}", minAgeSlider.value, -1f*maxAgeSlider.value);
		}
	}

	void Continue(){

		Debug.Log ("Continue was pressed");
		managePanelChanges.SetSmokeAndHabitPanelOn ();

		AppManeger.instance.wantAge[0] = wantAge[0];
		AppManeger.instance.wantAge[1] = wantAge[1];
		foreach (AppManeger.Gender gender in wantToMeet) {
			AppManeger.instance.wantToMeet.Add(gender);
		}

		// Update Gender choice at the user database and AppManeger
		UpdateMethods.UpdateUsersDatabase(AppManeger.instance.userID,AppManeger.instance.userName,
			AppManeger.instance.userAge.ToString(),myGenderChoice.ToString(),
			(result)=>{
				Debug.Log("User update " + result);
			}
		);
	}

	void UpdatePrefs(){

		Debug.Log ("Update was pressed");

		string wantMan = false.ToString();
		string wantWoman = false.ToString();

		if(wantToMeet.Contains(AppManeger.Gender.Male))
			wantMan = true.ToString();
		if (wantToMeet.Contains (AppManeger.Gender.Female))
			wantWoman = true.ToString();

		AppManeger.isLoadingData = true;
		UpdateMethods.UpdateUsersPrefsDatabase (AppManeger.instance.userID,AppManeger.instance.isSmoke.ToString(),
			wantAge[0].ToString(),wantAge[1].ToString(),AppManeger.instance.yourHabit.ToString(),wantMan,wantWoman,
			(result)=>{
				Debug.Log("User Prefs update " + result);
				AppManeger.isLoadingData = false;
				managePanelChanges.GoToCheersPanel (); // Call cheers screen after make the update
			}
		);

		// Update Gender choice at the user database and AppManeger
		UpdateMethods.UpdateUsersDatabase(AppManeger.instance.userID,AppManeger.instance.userName,
			AppManeger.instance.userAge.ToString(),myGenderChoice.ToString(),
			(result)=>{
				Debug.Log("User update " + result);
			}
		);
	}
}
