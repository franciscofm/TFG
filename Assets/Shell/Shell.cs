﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Shell : MonoBehaviour {

	[Header("Configuration")]
	public bool allowResize = false;
	public int outputMaxLines = 20;

	[Header("References")]
	public Text headerText;
	public Text inputText;
	public Text outputText;
	public ButtonShell closeButton;
	public ButtonShell minimizeButton;
	public ButtonShell maximizeButton;
	public RectTransform bodyRectTransform;
	public GameObject resizeGameObject;
	[Space]
	public Node node;

	[Header("Theme")]
	public Image topCornerLeft;
	public Image topCornerRight;
	public Image topCenter;
	public Image borderTopLeft;
	public Image borderLeft;
	public Image borderTopRight;
	public Image borderRight;
	public Image borderBottom;
	public Image background;
	public Image input;
	public void ChangeTheme(Aparence a) {
		topCornerLeft.sprite = a.topCornerLeft;
		topCornerRight.sprite = a.topCornerRight;
		topCenter.sprite = a.topCenter;
		borderTopLeft.sprite = a.border;
		borderTopRight.sprite = a.border;

		borderRight.color = a.borderColor;
		borderLeft.color = a.borderColor;
		borderBottom.color = a.borderColor;
		background.color = a.backgroundColor;
		input.color = a.backgroundColor;

		headerText.color = a.textColor;
		inputText.color = a.textColor;
		outputText.color = a.textColor;
		headerText.font = a.font;
		inputText.font = a.font;
		outputText.font = a.font;
	}

	[Header("Behaviour")]
	public bool focus;
	public bool expanded;
	public bool maximized;
	public bool pointed;
	public bool routined;
	public IEnumerator routine;
	[HideInInspector] string user = "admin@ubuntu:~$";
	public List<string> allOutput;
	public List<string> history;
	public int outputFirstIndex;
	public int outputShownLines;
	public int historyCommandIndex;
	public Vector3 dragOffset;

	public static List<Shell> focusedShells = new List<Shell> ();


	void Start() {
		expanded = true;
		inputText.text = user + " ";
		outputText.text = "";
		outputFirstIndex = 0;
		outputShownLines = 0;
		historyCommandIndex = 0;

		history = new List<string> ();
		allOutput = new List<string> ();
		resizeGameObject.SetActive (allowResize);

		FocusShell ();
	}

	public void WriteInput(string input) {
		inputText.text = input;
	}
	public void AddInput(string input) {
		if (pointed)
			inputText.text = inputText.text.Insert (inputText.text.Length - 1, input);
		else
			inputText.text += input;
	}
	public void RemoveInput(int index) {
		string[] splited = inputText.text.Split (new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
		if (splited.Length > 1) {
			inputText.text = inputText.text.Remove (inputText.text.Length - 1);
		} else {
			inputText.text = user + " ";
		}
	}
	public void ReadInput() {
		string command = inputText.text;
		string[] splited = command.Split (new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

		if (splited.Length < 2) return;

		string substring = "";
		for (int i = 1; i < splited.Length; ++i) {
			substring += splited [i] + " ";
			//print (splited [i] +" "+ i);
		}

		PrintOutput (user + " " + substring + Console.jump);
		if (splited [1] == "history") { //espeshial history case
			History ();
			history.Add ("history");
		} else if (Console.ReadCommand (splited, this)) {
			history.Add (substring);
		}
		inputText.text = user + " ";
		historyCommandIndex = history.Count;
	}
	public void PrintOutput(string output) {
		string[] splited = output.Split (new string[] { Console.jump }, StringSplitOptions.RemoveEmptyEntries);
		for (int n = 0; n < splited.Length; ++n) {
			allOutput.Add (splited[n] + Console.jump);
			++outputShownLines;
			if (outputShownLines > outputMaxLines) {
//			int firstShownLength = allOutput [outputFirstIndex].Length;
//			outputText.text = outputText.text.Remove (0, firstShownLength);
				int dif = outputShownLines - outputMaxLines;
				outputFirstIndex += dif;
				outputText.text = "";
				for (int i = 0; i < outputMaxLines; ++i)
					outputText.text += allOutput [i + outputFirstIndex];
				outputShownLines = outputMaxLines;
			} else {
				outputText.text += output;
			}
		}
	}
	public int ReturnError(string error) {
		PrintOutput (error + Console.jump);
		return 0;
	}

	public void History() {
		foreach (string s in history)
			PrintOutput (s + Console.jump);
	}
	public void GetPreviousCommand() {
		historyCommandIndex = Math.Max (0, historyCommandIndex - 1);
		inputText.text = user + " " + history[historyCommandIndex];
	}
	public void GetNextCommand() {
		historyCommandIndex = Math.Min (history.Count - 1, historyCommandIndex + 1);
		inputText.text = user + " " + history[historyCommandIndex];
	}

	public void CallbackClose() {
		UnfocusShell ();
		Destroy (gameObject);
	}
	public void CallbackMinimize() {
		expanded = !expanded;
		bodyRectTransform.gameObject.SetActive (expanded);
		print ("TODO: CallbackMinimize animation");
		if (!expanded)
			UnfocusShell ();
	}
	public void CallbackMaximize() {
		//FocusShell ();
		print ("TODO: CallbackMaximize");
	}

	public void CallbackResizeVertical() {

	}
	public void CallbackResizeHorizontal() {

	}

	public void FocusShell() { //Nos hacemos focus si no lo teniamos aun
		if(expanded) {
			focus = true;
			//StartCoroutine (routine = FocusRoutine ());
			if(!focusedShells.Contains(this)) {
				focusedShells.Add (this);
			}
		}
	}
	public void UnfocusShell() {
		focus = false;
		if (routined) {
			StopCoroutine (routine);
			if (pointed) {
				inputText.text = inputText.text.Remove (inputText.text.Length - 1);
				pointed = false;
			}
			routined = false;
		}
		if(focusedShells.Contains(this)) {
			focusedShells.Remove (this);
		}
  	}
	IEnumerator FocusRoutine() {
		pointed = false;
		routined = true;
		while (true) {
			yield return new WaitForSecondsRealtime (1f);
			pointed = true;
			inputText.text += "_";
			yield return new WaitForSecondsRealtime (1f);
			pointed = false;
			inputText.text = inputText.text.Remove (inputText.text.Length - 1);
		}
	}

	public void DragHeaderStart() {
		dragOffset = transform.position - Input.mousePosition;
	}
	public void DragHeader() {
		transform.position = Input.mousePosition + dragOffset;
	}
}
