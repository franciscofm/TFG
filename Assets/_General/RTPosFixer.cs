using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPosFixer : MonoBehaviour {

	public Vector2 anchoredPosition;
	RectTransform rt;
	// Use this for initialization
	void Start() {
		rt = transform as RectTransform;
		rt.anchoredPosition = anchoredPosition;
	}

	public void UpdateValues() {
		rt = transform as RectTransform;
		anchoredPosition = rt.anchoredPosition;
	}

	public void UpdatePosition() {
		rt.anchoredPosition = anchoredPosition;
	}

}
