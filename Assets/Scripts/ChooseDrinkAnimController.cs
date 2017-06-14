using UnityEngine;
using System.Collections;

public class ChooseDrinkAnimController : MonoBehaviour {

	public GameObject turnOnCheersGlass;
	public GameObject turnOnFailGlass;

	public void TurnOnCheersGlass(){

		turnOnCheersGlass.SetActive (true);
	}

	public void TurnOnFailGlass(){

		turnOnFailGlass.SetActive (true);
	}

	public void DisableMe(){

		this.gameObject.SetActive (false);
	}
}
