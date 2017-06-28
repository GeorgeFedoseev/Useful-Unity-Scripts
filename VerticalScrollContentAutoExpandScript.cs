using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VerticalScrollContentAutoExpandScript : MonoBehaviour {


	RectTransform contentRectTransform;


	void Awake(){
		contentRectTransform = GetComponent<RectTransform> ();

		InvokeRepeating ("UpdateContainerHeight", 0, 1f);


	}


	void Start(){
		GetComponent<Image> ().enabled = false;
	}

	void UpdateContainerHeight () {
		if (contentRectTransform == null)
			return;

		// calculate children sum height
		var sumHeight = 0f;
		foreach(Transform child in transform){			
			var rectTransform = child.GetComponent<RectTransform> ();
			if (rectTransform == null)
				continue;
			sumHeight += rectTransform.rect.height;
		}

		// Debug.LogWarning ("Sum height: "+sumHeight);

		// set height
		var newSizeDelta = contentRectTransform.sizeDelta;
		newSizeDelta.y = sumHeight;
		contentRectTransform.sizeDelta = newSizeDelta;

	}

	void LateUpdate(){
		// fix for NaN
		#if UNITY_EDITOR
		if(!Application.isPlaying){
			contentRectTransform.anchoredPosition = Vector2.zero;	
		}

		#endif
	}
}
