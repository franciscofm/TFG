using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Shell : MonoBehaviour {

	public delegate void ShellEvent(Shell sender);
	public delegate void ShellEventFull(Shell sender, string value);
	public event ShellEvent OnFocus;
	public event ShellEvent OnUnfocus;
	public event ShellEvent OnCreate;
	public event ShellEvent OnClose;
	public event ShellEvent OnMinimize;
	public event ShellEvent OnMaximize;

	public event ShellEventFull OnUpdateAddress;
	public event ShellEventFull OnChangeTheme;
	public event ShellEventFull OnOutput;

	void RaiseEvent(ShellEvent e) {
		if (e != null) e (this);
	}
	void RaiseEventFull(ShellEventFull e, string value) {
		if (e != null) e (this, value);
	}

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

		RaiseEventFull (OnChangeTheme, name);
	}

	[Header("Behaviour")]
	public bool focus;
	public bool expanded;
	public bool maximized;

	//location
	string user = "admin";
	string pcName = "PC1";
	public Folder folder;
	public string path;
	public string address;
	public void UpdateAddress(Folder folder) {
		this.folder = folder;
		path = folder.GetPathString ();
		address = user + "@" + pcName + ":" + path + "$";

		RaiseEventFull (OnUpdateAddress, folder.name);
	}

	//history
	public List<string> allOutput;
	public List<string> history;
	public int outputFirstIndex;
	public int outputShownLines;
	public int historyCommandIndex;

	public Vector3 dragOffset;

	public static List<Shell> focusedShells = new List<Shell> ();


	void Start() {
		expanded = true;
		outputText.text = "";
		outputFirstIndex = 0;
		outputShownLines = 0;
		historyCommandIndex = 0;

		history = new List<string> ();
		allOutput = new List<string> ();
		resizeGameObject.SetActive (allowResize);

		folder = node.rootFolder;
		path = folder.GetPathString ();
		address = user + "@" + pcName + ":" + path + "$";
		inputText.text = address + " ";

		//TODO fix
		transform.position += new Vector3 (bodyRectTransform.rect.width, -bodyRectTransform.rect.height, 0f) * .5f;

		FocusShell ();

		RaiseEvent (OnCreate);
	}

	public void SetInput(string input) {
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
			inputText.text = address + " ";
		}
	}
	public void ReadInput() {
		string command = inputText.text;
		string[] splited = command.Split (new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

		//si solo esta el directorio
		if (splited.Length < 2) return;

		//coger el comando
		string substring = "";
		for (int i = 1; i < splited.Length; ++i) 
			substring += splited [i] + " ";

		//poner el comando en el output
		string output = address + " " + substring + Console.jump;
		PrintOutput (output);
		RaiseEventFull (OnOutput, output);

		//tratar comando
		if (splited [1] == "history") { //espeshial history case
			History ();
			history.Add ("history");
		} else {
			CommandStructure commandReturn = Console.ReadCommand (splited, this);
			history.Add (substring);
			if (commandReturn.prompt) PrintOutput (commandReturn.value);
		}
		inputText.text = address + " ";
		historyCommandIndex = history.Count;
	}
	public void PrintOutput(string output) {
		string[] splited = output.Split (new string[] { Console.jump }, StringSplitOptions.RemoveEmptyEntries);
		for (int n = 0; n < splited.Length; ++n) {
			allOutput.Add (splited[n] + Console.jump);
			++outputShownLines;
			if (outputShownLines > outputMaxLines) {
				int dif = outputShownLines - outputMaxLines;
				outputFirstIndex += dif;
				outputText.text = "";
				for (int i = 0; i < outputMaxLines; ++i)
					outputText.text += allOutput [i + outputFirstIndex];
				outputShownLines = outputMaxLines;
			} else {
				outputText.text += splited[n] + Console.jump;
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
		inputText.text = address + " " + history[historyCommandIndex];
	}
	public void GetNextCommand() {
		historyCommandIndex = Math.Min (history.Count - 1, historyCommandIndex + 1);
		inputText.text = address + " " + history[historyCommandIndex];
	}

	public void CallbackClose() {
		RaiseEvent (OnClose);

		UnfocusShell ();
		Destroy (gameObject);
	}
	public void CallbackMinimize() {
		expanded = !expanded;
		bodyRectTransform.gameObject.SetActive (expanded);
		print ("TODO: CallbackMinimize animation");
		RaiseEvent (OnMinimize);
		if (!expanded)
			UnfocusShell ();
	}
	public void CallbackMaximize() {
		//FocusShell ();
		print ("TODO: CallbackMaximize");
		RaiseEvent (OnMaximize);
	}

	public void CallbackResizeVertical() {

	}
	public void CallbackResizeHorizontal() {

	}

	public void FocusShell() { //Nos hacemos focus si no lo teniamos aun
		print("Focused shells size: "+focusedShells.Count);
		if(expanded) {
			print("Focused shells size: "+focusedShells.Count);
			RaiseEvent (OnFocus);
			focus = true;
			if(!focusedShells.Contains(this)) {
				focusedShells.Add (this);
				print("Focused shells size: "+focusedShells.Count);
			}
		}
		transform.SetAsLastSibling ();
	}
	public void UnfocusShell() {
		focus = false;
		RaiseEvent (OnUnfocus);
		if(focusedShells.Contains(this)) {
			focusedShells.Remove (this);
		}
  	}

	public void DragHeaderStart() {
		dragOffset = transform.position - Input.mousePosition;
	}
	public void DragHeader() {
		transform.position = Input.mousePosition + dragOffset;
	}
}
