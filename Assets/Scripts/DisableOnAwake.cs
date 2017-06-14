using UnityEngine;
using System.Collections;

public class DisableOnAwake : MonoBehaviour {

	void Awake(){

		this.gameObject.SetActive (false);
	}
}
