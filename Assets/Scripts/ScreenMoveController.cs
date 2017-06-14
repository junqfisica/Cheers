using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScreenMoveController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {

	public RectTransform canvasRectTrans;

	private Vector2 startPoint;
	private Vector2 endPoint;
	private RectTransform rectTrans;
	private float maxHeight;
	private float canvasScale;

	void Awake(){

		rectTrans = gameObject.GetComponent<RectTransform> ();

	}

	void Start(){

		maxHeight = canvasRectTrans.rect.height; // max size of the canvas
		canvasScale = canvasRectTrans.localScale.y; // scale factor that canvas is using 
	}

	public void OnBeginDrag(PointerEventData eventData){
		startPoint = eventData.position;
	}

	public void OnDrag(PointerEventData eventData){
		endPoint = eventData.position;
		//Debug.Log ("OnEndDrag on: " + endPoint);

		float dx =  endPoint.x - startPoint.x;
		float dy =  endPoint.y - startPoint.y;
		// Angle goes from 0 - 180 (upper part) , then from -180 to 0 (lower part)
		float angle = Mathf.Atan2 (dy, dx)*Mathf.Rad2Deg;

		if (angle > -105f && angle < -75f) {// To down with 30 degree limit
			OnDragDown(eventData);

		} else if (angle > 75f && angle < 105f) {// Up
			OnDragUp(eventData);
		}

	}

	public void OnEndDrag(PointerEventData eventData){

		float frac = rectTrans.sizeDelta.y / maxHeight;

		endPoint = eventData.position;
		//Debug.Log ("OnEndDrag on: " + endPoint);

		float dx =  endPoint.x - startPoint.x;
		float dy =  endPoint.y - startPoint.y;
		// Angle goes from 0 - 180 (upper part) , then from -180 to 0 (lower part)
		float angle = Mathf.Atan2 (dy, dx)*Mathf.Rad2Deg;

		if (angle > -105f && angle < -75f) {// To down with 30 degree limit

			if (frac > 0.2f) {

				StartCoroutine("MoveScreenDown", maxHeight);

			} else {

				StartCoroutine ("MoveScreenUp", maxHeight);
			}

		} else if (angle > 75f && angle < 105f) {// Up

			if (frac > 0.8f) {

				StartCoroutine("MoveScreenDown", maxHeight);

			} else {

				StartCoroutine ("MoveScreenUp", maxHeight);
			}
			
		}

	}

	IEnumerator MoveScreenUp(float vel){

		while (rectTrans.sizeDelta.y > 10f) {

			// moves up with the touch
			rectTrans.sizeDelta += new Vector2 (0f,-vel*Time.deltaTime);
			yield return null;
		}

		rectTrans.sizeDelta = new Vector2(0f,Mathf.Clamp (rectTrans.sizeDelta.y, 10f, maxHeight));

	}

	IEnumerator MoveScreenDown(float vel){


		while (rectTrans.sizeDelta.y < maxHeight) {

			// moves down with the touch
			rectTrans.sizeDelta += new Vector2 (0f,vel*Time.deltaTime);
			yield return null;
		}

		rectTrans.sizeDelta = new Vector2(0f,Mathf.Clamp (rectTrans.sizeDelta.y, 10f, maxHeight));

	}

	void OnDragDown(PointerEventData eventData){

		// moves down with the touch
		rectTrans.sizeDelta = new Vector2 (0f,maxHeight - eventData.pointerCurrentRaycast.screenPosition.y/canvasScale);
		rectTrans.sizeDelta = new Vector2(0f,Mathf.Clamp (rectTrans.sizeDelta.y, 10f, maxHeight));
	}

	void OnDragUp(PointerEventData eventData){

		// moves up with the touch
		rectTrans.sizeDelta = new Vector2 (0f,maxHeight - eventData.pointerCurrentRaycast.screenPosition.y/canvasScale);
		rectTrans.sizeDelta = new Vector2(0f,Mathf.Clamp (rectTrans.sizeDelta.y, 10f, maxHeight));
	}
}
