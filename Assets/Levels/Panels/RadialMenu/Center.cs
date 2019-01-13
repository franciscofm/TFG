using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadialMenu;

public class Center : MonoBehaviour {

	public NodeGroup startingGroup;
	public static List<Center> allCenters = new List<Center>();

	[HideInInspector] public Node node;
	[HideInInspector] public Level level;

	public delegate void RadialMenuEvent(Center sender);
	public event RadialMenuEvent OnClose;
	public event RadialMenuEvent OnCommand;
	public event RadialMenuEvent OnIfconfig;
	public event RadialMenuEvent OnPing;
	public event RadialMenuEvent OnRoute;
	public event RadialMenuEvent OnManual;
	public event RadialMenuEvent OnColor;
	public event RadialMenuEvent OnShell;
	public event RadialMenuEvent OnDragStart;
	public event RadialMenuEvent OnDragEnd;

	void Start() {
		allCenters.Add (this);
		startingGroup.ActivateNodes ();
	}

	public void Close() {
		if (OnClose != null) OnClose (this);
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
		if (OnIfconfig != null) OnIfconfig (this);
		if (OnCommand != null) OnCommand (this);
	}
	public void CallbackPing(RectTransform rect) {
		level.CallbackPing (rect, node);
		if (OnPing != null) OnPing (this);
		if (OnCommand != null) OnCommand (this);
	}
	public void CallbackRoute(RectTransform rect) {
		level.CallbackRoute (rect, node);
		if (OnRoute != null) OnRoute (this);
		if (OnCommand != null) OnCommand (this);
	}
	public void CallbackManual(RectTransform rect) {
		level.CallbackManual (rect);
		if (OnManual != null) OnManual (this);
		if (OnCommand != null) OnCommand (this);
	}

	public void CallbackColorWhite() {
		CallbackColorPick (Color.white);
	}
	public void CallbackColorBlack() {
		CallbackColorPick (Color.black);
	}
	public void CallbackColorPick(Color c) {
		level.CallbackColorPick (c, node);
		if (OnColor != null) OnColor (this);
	}

	public void CallbackShell(RectTransform rect) {
		level.CallbackCreateShell (rect, node);
		if (OnShell != null) OnShell (this);
	}

	public void DragingStart() {
		if (OnDragStart != null) OnDragStart (this);
	}
	public void DragingEnd() {
		if (OnDragEnd != null) OnDragEnd (this);
	}
}