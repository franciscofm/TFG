using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Shell : MonoBehaviour {

	[Header("Configuration")]
	public bool allowResize;
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
	public Node nodeS;

	[Header("Behaviour")]
	public bool focus;
	public bool expanded;
	public bool maximized;
	public string user = "admin@ubuntu:~$";
	public List<string> allOutput;
	public List<string> history;
	public int outputFirstIndex;
	public int outputShownLines;
	public int historyCommandIndex;

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
			substring += splited [i];
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
		historyCommandIndex = 0;
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

	public void History() {
		foreach (string s in history)
			PrintOutput (s + Console.jump);
	}
	public void GetPreviousCommand() {
		historyCommandIndex = Math.Max (history.Count, historyCommandIndex + 1);
	}
	public void GetNextCommand() {
		historyCommandIndex = Math.Min (0, historyCommandIndex - 1);
	}

	public void CallbackClose() {
		UnfocusShell ();
		Destroy (gameObject);
	}
	public void CallbackMinimize() {
		expanded = !expanded;
		bodyRectTransform.gameObject.SetActive (expanded);
		print ("TODO: CallbackMinimize animation");
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
			if(!focusedShells.Contains(this)) {
				focusedShells.Add (this);
			}
		}
	}
	public void UnfocusShell() {
		focus = false;
		if(focusedShells.Contains(this)) {
			focusedShells.Remove (this);
		}
	}

	public void DragHeader() {
		print ("TODO: DragHeader");
	}
}
