using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {

	private Vector2 startPoint;
	private Vector2 endPoint;
	private static float angle;
	private static bool onDragEnd = false;

	/* // ======= For test ======
	void OnGUI(){

		if (OnLeftDrag ()) {
			Debug.Log ("Drag left");
		} else if (OnRightDrag()){
			Debug.Log ("Drag right");
		}else if (OnUpDrag()){
			Debug.Log ("Drag Up");
		}else if (OnDownDrag()){
			Debug.Log ("Drag Down");
		}
	}*/

	public void OnBeginDrag(PointerEventData eventData){

		onDragEnd = false;
		startPoint = eventData.position;
		//Debug.Log ("OnBeginDrag on: " + startPoint);
	}

	public void OnDrag(PointerEventData eventData){
		onDragEnd = false;
	}

	public void OnEndDrag(PointerEventData eventData){
		endPoint = eventData.position;
		//Debug.Log ("OnEndDrag on: " + endPoint);

		float dx =  endPoint.x - startPoint.x;
		float dy =  endPoint.y - startPoint.y;
		// Angle goes from 0 - 180 (upper part) , then from -180 to 0 (lower part)
		angle = Mathf.Atan2 (dy, dx)*Mathf.Rad2Deg;
		onDragEnd = true;
		StartCoroutine ("CleanOnDragEnd"); // Thia avoid the onDragEnd being kepped for to long if any function is used

	}

	IEnumerator CleanOnDragEnd(){

		yield return new WaitForSeconds (0.2f);
		onDragEnd = false;
		angle = 900f;
	}

	/// <summary>
	/// Return true if right drag event.
	/// </summary>
	public static bool OnRightDrag{get{

			if (Mathf.Abs (angle) < 30f && onDragEnd) {// To right with 30 degree limit
				onDragEnd = false;
				return true;
			} else {
				return false;
			}
		}
		
	}
	/// <summary>
	/// Return true if left drag event.
	/// </summary>
	public static bool OnLeftDrag{get{

			if (Mathf.Abs (angle) > 165f && Mathf.Abs (angle) < 195f && onDragEnd) {// To left with 30 degree limit
				onDragEnd = false;
				return true;
			} else {
				return false;
			}
		}
	}
	/// <summary>
	/// Return true if Up drag event.
	/// </summary>
	public static bool OnUpDrag{get{

			if (angle > 75f && angle < 105f && onDragEnd) { // To up with 30 degree limit
				onDragEnd = false;
				return true;
			} else {
				return false;
			}
		}
	}
	/// <summary>
	/// Return true if down drag event.
	/// </summary>
	public static bool OnDownDrag{get{

			if (angle > -105f && angle < -75f && onDragEnd) {// To down with 30 degree limit
				onDragEnd = false;
				return true;
			} else {
				return false;
			}
		}
	}

}
