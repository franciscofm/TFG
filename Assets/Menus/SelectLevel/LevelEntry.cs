using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelEntry : MonoBehaviour, IPointerDownHandler {

	public static SelectLevel controller;
	public CanvasGroup canvasGroup;

	public Text levelText;
	public Image levelImage;
	public Text conceptsText;
	public Text objectiveText;
	[HideInInspector] public int id;
	[HideInInspector] public string scene;

	public void OnPointerDown(PointerEventData data) {
		controller.CallbackLevelEntry (this);
	}
}
