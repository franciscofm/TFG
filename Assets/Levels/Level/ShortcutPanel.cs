using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShortcutPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public float movementDuration = 0.3f;
	public float expandDuration = 3f;
	public float expandWidth = 385f;
	public float collapsedWidth = 200f;
	public bool expanded = false;

	public RectTransform panelRT;
	public Transform viewportT;
	public GameObject colorPanel;
	float diffWidth;
	ShortcutButton[] shortcutButtons;

	IEnumerator timerRoutine, movementRoutine;

	bool ifaceInfoShown = false;
	List<InterfaceVisuals> allVisuals = InterfaceVisuals.allVisuals;

	void Awake() {
		shortcutButtons = new ShortcutButton[viewportT.childCount];
		for (int i = 0; i < shortcutButtons.Length; ++i)
			shortcutButtons [i] = viewportT.GetChild (i).GetComponent<ShortcutButton> ();
	}
	void Start() {
		diffWidth = expandWidth - collapsedWidth;
		timerRoutine = null;
		if(expanded) panelRT.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, expandWidth);
		else panelRT.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, collapsedWidth);
	}

	public void OnPointerEnter(PointerEventData data) {
		if (expanded) {
			if (timerRoutine != null) StopCoroutine (timerRoutine);
		} else {
			if (movementRoutine != null) StopCoroutine (movementRoutine);
			expanded = true;
			foreach (ShortcutButton sb in shortcutButtons) sb.Expand ();
			StartCoroutine (movementRoutine = Routines.DoWhile (movementDuration, ExpandMethod));
		}
	}
	void ExpandMethod(float f) {
		panelRT.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Mathf.Lerp(collapsedWidth, expandWidth, f));
	}

	public void OnPointerExit(PointerEventData data) {
		StartCoroutine (timerRoutine = CollapseRoutine());
	}
	IEnumerator CollapseRoutine() {
		float timer = 0f;
		while (timer < expandDuration) {
			yield return null;
			timer += Time.deltaTime;
		}
		if (movementRoutine != null) StopCoroutine (movementRoutine);
		expanded = false;
		foreach (ShortcutButton sb in shortcutButtons) sb.Collapse ();
		StartCoroutine (movementRoutine = Routines.DoWhile (movementDuration, CollapseMethod));
	}
	void CollapseMethod(float f) {
		panelRT.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Mathf.Lerp(expandWidth, collapsedWidth, f));
	}

	public void F1_CloseAll() {
		Center.CloseAll ();
		Shell.CloseAll ();
	}
	public void F2_CloseAllNodePanels() {
		Center.CloseAll ();
	}
	public void F3_CloseAllShells() {
		Shell.CloseAll ();
	}
	public void F4_MinimizeAllShells() {
		Shell.MinimizeAll ();
	}
	public void F5_ShowInterfaceData() {
		if(!ifaceInfoShown)
			foreach (InterfaceVisuals iface in allVisuals)
				iface.ShowInformation ();
		else
			foreach (InterfaceVisuals iface in allVisuals)
				iface.HideInformation ();
	}
	public void F6_ShowBackgroundColorPanel() {
		colorPanel.SetActive (!colorPanel.activeSelf);
	}
	public void F7_ShowMenu() {
		Manager.Scenes.LoadSceneAdditiveMerge ("IngameMenu");
	}
}
