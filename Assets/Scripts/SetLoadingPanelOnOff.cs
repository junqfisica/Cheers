using UnityEngine;
using System.Collections;

public class SetLoadingPanelOnOff : MonoBehaviour {

	Transform trans;

	void Awake(){

		trans = transform.FindChild("Loading Image").GetComponent<Transform> ();
	}

	void Update(){
		
		if (AppManeger.isLoadingData) {
			if (!trans.gameObject.activeSelf)
				trans.gameObject.SetActive (true);
		} else {
			if (trans.gameObject.activeSelf)
				trans.gameObject.SetActive (false);
		}
	}
}
