using UnityEngine;
using System.Collections;

public class UtilMethods : MonoBehaviour {

	public static UtilMethods util;

	void Awake (){

		if (util == null) {

			DontDestroyOnLoad (gameObject);
			util = this;

		} else if (util != this) {

			Destroy (gameObject);

		}
	}

	/// <summary>
	/// Displaies the WWW data result.
	/// </summary>
	/// <param name="data">data.</param>
	public void DisplayWWWDataResult(WWW data){

		StartCoroutine ("DataResult", data);
	}

	IEnumerator DataResult(WWW data){

		yield return data;
		Debug.Log (data.text);
	}

	/// <summary>
	/// Gets the data from WWW.
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="data">Data.</param>
	public void GetData(WWW data, System.Action<string> callback){


		StartCoroutine (GetDataResult (data, (myReturnValue) => {
			callback(myReturnValue);
		}));
	}

	IEnumerator GetDataResult(WWW data, System.Action<string> callback){

		yield return data;
		callback (data.text);
	}
		
}
