using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Repository.Application;

public class SetSmokeAndHabit : MonoBehaviour {

	public Button smokeBut;
	public Button noSmokeBut;
	public Button dayBut;
	public Button nighBut;
	public Button continueBut;

	private Sprite smoke;
	private Sprite smokeColor;
	private Sprite noSmoke;
	private Sprite noSmokeColor;
	private Sprite day;
	private Sprite dayColor;
	private Sprite night;
	private Sprite nightColor;

	private bool chooseSmoke;
	private ManagePanelChanges managePanelChanges;


	void Awake(){

		smoke = Resources.Load<Sprite> ("Sprites/Icons/smoke");
		smokeColor = Resources.Load<Sprite> ("Sprites/Icons/smoke_color");
		noSmoke = Resources.Load<Sprite> ("Sprites/Icons/no_smoke");
		noSmokeColor = Resources.Load<Sprite> ("Sprites/Icons/no_smoke_color");
		day = Resources.Load<Sprite> ("Sprites/Icons/day");
		dayColor = Resources.Load<Sprite> ("Sprites/Icons/day_color");
		night = Resources.Load<Sprite> ("Sprites/Icons/night");
		nightColor = Resources.Load<Sprite> ("Sprites/Icons/night_color");

	}

	void Start(){

		chooseSmoke = false;
		SetTheChoices (Smoke,NoSmoke,PreferDay,PreferNight);
		managePanelChanges = ManagePanelChanges.Instance ();
	}

	void OnGUI(){

		if (chooseSmoke	&& AppManeger.instance.yourHabit != AppManeger.Habit.None) { // Check if all fields were filled

			continueBut.interactable = true;

		} else {

			continueBut.interactable = false;
		}

	}

	void SetTheChoices(UnityAction SmokeBut, UnityAction NoSmokeBut, UnityAction DayBut, UnityAction NightBut){

		smokeBut.onClick.RemoveAllListeners ();
		smokeBut.onClick.AddListener (SmokeBut);

		noSmokeBut.onClick.RemoveAllListeners ();
		noSmokeBut.onClick.AddListener (NoSmokeBut);

		dayBut.onClick.RemoveAllListeners ();
		dayBut.onClick.AddListener (DayBut);

		nighBut.onClick.RemoveAllListeners ();
		nighBut.onClick.AddListener (NightBut);

	}

	void Smoke(){

		smokeBut.image.sprite = smokeColor;
		noSmokeBut.image.sprite = noSmoke;
		AppManeger.instance.isSmoke = true;
		chooseSmoke = true;
	}

	void NoSmoke(){

		smokeBut.image.sprite = smoke;
		noSmokeBut.image.sprite = noSmokeColor;
		AppManeger.instance.isSmoke = false;
		chooseSmoke = true;

	}

	void PreferDay(){

		dayBut.image.sprite = dayColor;
		nighBut.image.sprite = night;
		AppManeger.instance.yourHabit = AppManeger.Habit.Day;
	}

	void PreferNight(){

		dayBut.image.sprite = day;
		nighBut.image.sprite = nightColor;
		AppManeger.instance.yourHabit = AppManeger.Habit.Night;
	}

	public void Continue(){

		Debug.Log ("Continue was pressed");
		managePanelChanges.GoToCheersPanel ();

		AppManeger.isLoadingData = true;
		string id = AppManeger.instance.userID;
		string smoke = AppManeger.instance.isSmoke.ToString();
		string minAge = AppManeger.instance.wantAge [0].ToString();
		string maxAge = AppManeger.instance.wantAge [1].ToString ();
		string habit = AppManeger.instance.yourHabit.ToString ();
		string wantMan = "false";
		string wantWoman = "false";
		if (AppManeger.instance.wantToMeet.Contains (AppManeger.Gender.Male))
			wantMan = "true";
		if(AppManeger.instance.wantToMeet.Contains(AppManeger.Gender.Female))
			wantWoman = "true";

		PostMethods.InsertUserIntoUsersPrefsDatabase(id,smoke,minAge,maxAge,habit,wantMan,wantWoman, (result) => {

			AppManeger.isLoadingData = false;
			Debug.Log(result);
		});
	}
}
