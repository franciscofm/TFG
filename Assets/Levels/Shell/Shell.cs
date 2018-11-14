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
	public Aparence currentTheme;

	[Header("References")]
	public Text headerText;
	public Text bodyText;
	public RectTransform shellRectTransform;
	public RectTransform bodyRectTransform;
	public RectTransform maskRectTransform;
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
	public Image backgroundBody;
	public void ChangeTheme(Aparence a) {
		topCornerLeft.sprite = a.topCornerLeft;
		topCornerRight.sprite = a.topCornerRight;
		topCenter.sprite = a.topCenter;
		borderTopLeft.sprite = a.border;
		borderTopRight.sprite = a.border;

		borderRight.color = a.borderColor;
		borderLeft.color = a.borderColor;
		borderBottom.color = a.borderColor;
		backgroundBody.color = a.backgroundColor;

		headerText.color = a.textColor;
		bodyText.color = a.textColor;
		headerText.font = a.font;
		bodyText.font = a.font;

		RaiseEventFull (OnChangeTheme, name);
	}

	[Header("Behaviour")]
	public bool focus;
	public bool expanded;
	public bool maximized;
	public static List<Shell> focusedShells = new List<Shell>();
	public static List<Shell> existingShells = new List<Shell>();

	//location
	string user = "admin";
	string pcName = "PC1";
	public Folder folder;
	public string path;
	public string address;

	[Header("Text management")]
	public string currentOutputText; //text has already been written
	public string currentInputText; //text the user is writing
	public List<string> history; //commands the user has entered
	public int historyCommandIndex;

	public Vector3 dragOffset;

	void Start() {
		Init (); //should be called by others
		RaiseEvent (OnCreate);
	}

	public void Init() {
		existingShells.Add (this);
		expanded = true;

		historyCommandIndex = 0;
		history = new List<string> ();
		
		resizeGameObject.SetActive (allowResize);

		UpdateAddress(node.rootFolder);
		currentOutputText = address;
		SetInput ("");

		FocusShell ();
	}

	public void UpdateAddress(Folder folder) {
		this.folder = folder;
		path = folder.GetPathString ();
		address = user + "@" + pcName + ":" + path + "$ ";

		RaiseEventFull (OnUpdateAddress, folder.name);
	}

	public void AddInput(string input) { //adds characters by Keyboard.cs
		currentInputText += input;
		bodyText.text = currentOutputText + currentInputText;
		UpdateTextMask ();
	}
	public void SetInput(string input) {
		currentInputText = input;
		bodyText.text = currentOutputText + currentInputText;
		UpdateTextMask ();
	}
	public void RemoveInput() { //press Back key by Keyboard.cs
		if (currentInputText.Length > 0) {
			currentInputText = currentInputText.Remove (currentInputText.Length - 1);
			bodyText.text = currentOutputText + currentInputText;
			UpdateTextMask ();
		} 
	}
	public void ReadInput() {
		if (currentInputText.Length == 0) return; //nothing written
		string[] splited = currentInputText.Split (new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

		PrintOutputNoAddress (currentInputText + Console.jump);
		RaiseEventFull (OnOutput, currentInputText);

		//tratar comando
		if (splited [0] == "history") { //espeshial history case
			History ();
			history.Add ("history");
			node.RaiseOnShellCommand ("history", true);
		} else {
			CommandStructure commandReturn = Console.ReadCommand (splited, this);
			history.Add (currentInputText);
			if (commandReturn.prompt) PrintOutputNoAddress (commandReturn.value);
			node.RaiseOnShellCommand (splited [0], commandReturn.correct);
		}
		historyCommandIndex = history.Count;
		currentInputText = "";
		PrintAddress ();
	}
	public void PrintOutput(string output) {
		currentOutputText += output;
		currentOutputText += address;
		bodyText.text = currentOutputText;
		UpdateTextMask ();
	}
	public void PrintAddress() {
		currentOutputText += Console.jump + address;
		bodyText.text = currentOutputText;
		UpdateTextMask ();
	}
	public void PrintOutputNoAddress(string output) {
		currentOutputText += output;
		bodyText.text = currentOutputText;
		UpdateTextMask ();
	}
	public void UpdateTextMask() {
		Canvas.ForceUpdateCanvases ();
		float f = bodyText.preferredHeight;
		bodyRectTransform.anchoredPosition = Vector2.zero;
		if (f > maskRectTransform.rect.height) {
			bodyRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, f);
		} else {
			bodyRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, maskRectTransform.rect.height);
		}
	}

	public void History() {
		foreach (string s in history)
			PrintOutputNoAddress (s + Console.jump);
	}
	public void GetPreviousCommand() {
		historyCommandIndex = Math.Max (0, historyCommandIndex - 1);
		SetInput (history [historyCommandIndex]);
	}
	public void GetNextCommand() {
		historyCommandIndex = Math.Min (history.Count - 1, historyCommandIndex + 1);
		SetInput (history [historyCommandIndex]);
	}

	public void CallbackClose() {
		RaiseEvent (OnClose);
		existingShells.Remove (this);
		UnfocusShell ();
		Destroy (gameObject);
	}
	public void CallbackMinimize() {
		expanded = !expanded;
		bodyRectTransform.gameObject.SetActive (expanded);
		RaiseEvent (OnMinimize);
		if (!expanded)
			UnfocusShell ();
	}
	public void CallbackMaximize() {
		//FocusShell ();
		print ("TODO: CallbackMaximize");
		RaiseEvent (OnMaximize);
	}

	float resizeStart;
	public void CallbackResizeVerticalStart() {
		resizeStart = -shellRectTransform.anchoredPosition.y;
	}
	public void CallbackResizeVertical() {
		float height = (1080f-Input.mousePosition.y) - resizeStart;
		if (height < 100f)
			return;
		shellRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);
	}
	public void CallbackResizeHorizontalStart() {
		resizeStart = shellRectTransform.anchoredPosition.x;
	}
	public void CallbackResizeHorizontal() {
		float width = Input.mousePosition.x - resizeStart;
		if (width < 200f)
			return;
		shellRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
	}
	public void CallbackResizeEnd() {
		UpdateTextMask ();
	}
	public void HoverResizeEnter() {
		print ("Hover resize Enter");
	}
	public void HoverResizeExit() {
		print ("Hover resize Exit");
	}

	public bool IsMouseOver(Vector2 mousePos) {
		return (RectTransformUtility.RectangleContainsScreenPoint(shellRectTransform, mousePos));
	}
	public void FocusShell() {
		if(expanded) {
			RaiseEvent (OnFocus);
			if (!Keyboard.Ctrl) 
				UnfocusAll();
			focus = true;
			if(!focusedShells.Contains(this)) {
				focusedShells.Add (this);
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
	public static void UnfocusAll() {
		while (focusedShells.Count > 0)
			focusedShells [0].UnfocusShell ();
	}

	public void DragHeaderStart() {
		dragOffset = transform.position - Input.mousePosition;
	}
	public void DragHeader() {
		transform.position = Input.mousePosition + dragOffset;
	}
}
