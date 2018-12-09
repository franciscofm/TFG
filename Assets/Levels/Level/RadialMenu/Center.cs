using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadialMenu;

public class Center : MonoBehaviour {

	public NodeGroup startingGroup;

	[HideInInspector] public Node node;
	[HideInInspector] public Level level;
	[HideInInspector] public Transform canvasTransform;

	void Start() {
		startingGroup.ActivateNodes ();
	}

	public void Close() {
		level.OnCenterClose (node);
		Destroy (gameObject);
	}

	public void CallbackIfconfig(RectTransform rect) {
		level.CallbackIfconfig (rect);
	}
	public void CallbackPing(RectTransform rect) {
		level.CallbackPing (rect);
	}
	public void CallbackRoute(RectTransform rect) {
		level.CallbackRoute (rect);
	}
	public void CallbackManual(RectTransform rect) {
		level.CallbackManual (rect);
	}

	public void CallbackColorPick(Color c) {
		level.CallbackColorPick (c, node);
	}

	public void CallbackShell(RectTransform rect) {
		level.CallbackCreateShell (rect);
	}
}