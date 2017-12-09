using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapTriggerScript : MonoBehaviour {
	

	[Range(1, 10)]
	public int numberOfFingers = 3;

	[Range(1, 10)]
	public int numberOfTaps = 3;

	public float timeLimit = 1f;

	public KeyCode keyboardKey;

	public UnityEvent OnTrigger;


	int currentNumberOfTaps;
	float startRegisterTime = -1;

	// Use this for initialization
	void Start () {
		
	}
	

	bool hasTouchedWithRequiredNumberOfFingers = false;
	void Update () {
		if (Input.GetKeyDown (keyboardKey)) {
			OnGestureDone ();
		}


		if (Input.touchCount == numberOfFingers) {
			if (!hasTouchedWithRequiredNumberOfFingers) { // if already touched dont read again before untouch
				hasTouchedWithRequiredNumberOfFingers = true;
				Tap ();
			}
		} else {
			hasTouchedWithRequiredNumberOfFingers = false;
		}
	}


	void Tap(){
		if (currentNumberOfTaps == 0) {
			startRegisterTime = Time.time;
		}

		currentNumberOfTaps++;

		if (currentNumberOfTaps >= numberOfTaps) {
			if (Time.time - startRegisterTime <= timeLimit) {
				OnGestureDone ();	
			}

			currentNumberOfTaps = 0;
			startRegisterTime = -1;
		}


	}

	void OnGestureDone(){
		OnTrigger.Invoke ();

	}
}
