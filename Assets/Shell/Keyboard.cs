﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour {

	List<Shell> focusedShells;
	void Start() {
		focusedShells = Shell.focusedShells;
	}
	// Update is called once per frame
	void Update () {
		if (focusedShells.Count > 0) {
			if (Input.GetKeyDown (KeyCode.Return))
				foreach (Shell shell in focusedShells)
					shell.ReadInput ();
			else if (Input.GetKeyDown (KeyCode.Backspace))
				foreach (Shell shell in focusedShells)
					shell.RemoveInput (0);
			else if (Input.GetKeyDown (KeyCode.UpArrow)) {

			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {

			} else {
				string input = Input.inputString;
				if (input.Length > 0) {
					foreach (Shell shell in focusedShells)
						shell.AddInput (input);
				}
			}
		}
	}

	public static void History() {
		foreach (Shell shell in Shell.focusedShells)
			shell.History ();
	}
}
