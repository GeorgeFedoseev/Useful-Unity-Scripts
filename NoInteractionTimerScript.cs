using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoInteractionTimerScript : MonoBehaviour {

	public Action TimerFire = () => {};
	public float noInteractionTimeoutSeconds = 30f;

	float lastInteractionTime = 0;

	bool noInteractionTimerFired = true;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		noInteractionTimeoutSeconds = 99999;
		#endif

		lastInteractionTime = Time.time;
		noInteractionTimerFired = false;
	}

	public void Touch(){
		noInteractionTimerFired = false;
		lastInteractionTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) || Input.touchCount > 0) {
			noInteractionTimerFired = false;
			lastInteractionTime = Time.time;
		}

		if (!noInteractionTimerFired && Time.time - lastInteractionTime > noInteractionTimeoutSeconds) {
			TimerFire ();
			noInteractionTimerFired = true;
		}
	}
}
