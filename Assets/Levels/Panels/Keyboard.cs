using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour {

	[HideInInspector] public List<Shell> focusedShells;
	[HideInInspector] public List<Shell> existingShells;
	[HideInInspector] public List<InterfaceVisuals> allVisuals;
	[HideInInspector] public ShortcutPanel shortcutPanel;
	public static bool Ctrl;

	void Start() {
		focusedShells = Shell.focusedShells;
		existingShells = Shell.existingShells;
		shortcutPanel = GetComponent<ShortcutPanel> ();
	}

	void Update () {
		//Shell interaction
		if (focusedShells.Count > 0) { 
			Ctrl = Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl); //No shell click
			if (!Ctrl && Input.GetMouseButton (0)) {
				bool found = false;
				for (int i = 0; i < existingShells.Count; ++i) {
					if (found = existingShells [i].IsMouseOver (Input.mousePosition))
						i = existingShells.Count;
				}
				if (!found)
					Shell.UnfocusAll ();
			}

			if (Input.GetKeyDown (KeyCode.Return))
				foreach (Shell shell in focusedShells)
					shell.ReadInput (); //Enter
			else if (Input.GetKeyDown (KeyCode.Backspace))
				foreach (Shell shell in focusedShells)
					shell.RemoveInput (); //Borrar
			else if (Input.GetKeyDown (KeyCode.UpArrow))
				foreach (Shell shell in focusedShells)
					shell.GetPreviousCommand (); //Arriba
			else if (Input.GetKeyDown (KeyCode.DownArrow))
				foreach (Shell shell in focusedShells)
					shell.GetNextCommand (); //Abajo
			else if (Input.inputString.Length > 0)
				foreach (Shell shell in focusedShells)
					shell.AddInput (Input.inputString); //Escribir normy
			
		} 

		//Interface interaction
		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			foreach (InterfaceVisuals iface in allVisuals)
				iface.ShowInformation ();
		} else if (Input.GetKeyUp (KeyCode.LeftAlt)) {
			foreach (InterfaceVisuals iface in allVisuals)
				iface.HideInformation ();
		}

		//Shortcut panel
		if (Input.GetKeyDown (KeyCode.F1)) shortcutPanel.F1_CloseAll();
		if (Input.GetKeyDown (KeyCode.F2)) shortcutPanel.F2_CloseAllNodePanels ();
		if (Input.GetKeyDown (KeyCode.F3)) shortcutPanel.F3_CloseAllShells ();
		if (Input.GetKeyDown (KeyCode.F4)) shortcutPanel.F4_MinimizeAllShells ();
		if (Input.GetKeyDown (KeyCode.F5)) shortcutPanel.F5_ShowInterfaceData ();
		if (Input.GetKeyDown (KeyCode.F6)) shortcutPanel.F6_ShowBackgroundColorPanel ();
		if (Input.GetKeyDown (KeyCode.F7)) shortcutPanel.F7_ShowMenu ();
	}

}
