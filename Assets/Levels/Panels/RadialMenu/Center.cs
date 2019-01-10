using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadialMenu;

public class Center : MonoBehaviour {

	public NodeGroup startingGroup;
	public static List<Center> allCenters = new List<Center>();

	[HideInInspector] public Node node;
	[HideInInspector] public Level level;
	[HideInInspector] public Transform canvasTransform;

	void Start() {
		allCenters.Add (this);
		startingGroup.ActivateNodes ();
	}

	public void Close() {
		allCenters.Remove (this);
		level.OnCenterClose (node);
		Destroy (gameObject);
	}
	public static void CloseAll() {
		while (allCenters.Count > 0)
			allCenters [0].Close ();
	}

	public void CallbackIfconfig(RectTransform rect) {
		level.CallbackIfconfig (rect, node);
	}
	public void CallbackPing(RectTransform rect) {
		level.CallbackPing (rect, node);
	}
	public void CallbackRoute(RectTransform rect) {
		level.CallbackRoute (rect, node);
	}
	public void CallbackManual(RectTransform rect) {
		level.CallbackManual (rect);
	}

	public void CallbackColorWhite() {
		CallbackColorPick (Color.white);
	}
	public void CallbackColorBlack() {
		CallbackColorPick (Color.black);
	}
	public void CallbackColorPick(Color c) {
		print ("Callback color: " + c);
		level.CallbackColorPick (c, node);
	}

	public void CallbackShell(RectTransform rect) {
		level.CallbackCreateShell (rect, node);
	}
}