using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour {

	List<Shell> focusedShells;
	List<Shell> existingShells;
	public static bool Ctrl;

	void Start() {
		focusedShells = Shell.focusedShells;
		existingShells = Shell.existingShells;
	}

	void Update () {
		if (focusedShells.Count > 0) { //Shell interaction
			Ctrl = Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl); //No shell click
			if (!Ctrl && Input.GetMouseButton(0)) {
				bool found = false;
				for (int i = 0; i < existingShells.Count; ++i) {
					if (found = existingShells [i].IsMouseOver (Input.mousePosition))
						i = existingShells.Count;
				}
				if (!found)
					Shell.UnfocusAll ();
			}

			if (Input.GetKeyDown (KeyCode.Return)) foreach (Shell shell in focusedShells) shell.ReadInput (); //Enter
			else if (Input.GetKeyDown (KeyCode.Backspace)) foreach (Shell shell in focusedShells) shell.RemoveInput (); //Borrar
			else if (Input.GetKeyDown (KeyCode.UpArrow)) foreach (Shell shell in focusedShells) shell.GetPreviousCommand (); //Arriba
			else if (Input.GetKeyDown (KeyCode.DownArrow))  foreach (Shell shell in focusedShells) shell.GetNextCommand (); //Abajo
			else if (Input.inputString.Length > 0) foreach (Shell shell in focusedShells) shell.AddInput (Input.inputString); //Escribir normy
		}
	}

}
