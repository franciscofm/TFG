using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShortcutButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Image groupImage;
	public Image icon;
	public GameObject text;

	Color click = new Color(0.75f, 0.75f, 0.75f, 1f);

	public void Expand() {
		//text.SetActive (true);
	}

	public void Collapse() {
		//text.SetActive (false);
	}

	public void OnPointerDown(PointerEventData data) {
		StartCoroutine (Routines.DoWhile (0.15f, delegate(float f) {
			LerpColor (Color.white, click, f);
		}));
	}
	public void OnPointerUp(PointerEventData data) {
		StartCoroutine (Routines.DoWhile (0.15f, delegate(float f) {
			LerpColor (click, Color.white, f);
		}));
	}
	public void LerpColor(Color c1, Color c2, float f) {
		groupImage.color = Color.Lerp (c1, c2, f);
	}
}
