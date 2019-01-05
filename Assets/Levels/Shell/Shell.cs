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
	public bool initOnStart = false;

	[Header("References")]
	public Text headerText;
	public Text bodyText;
	public RectTransform shellRectTransform;
	public RectTransform bodyRectTransform;
	public RectTransform contentTextRectTransform;
	public RectTransform maskRectTransform;
	public GameObject resizeGameObject;
	[Space]
	public Node node;

	[Header("Theme")]
	public Aparence currentTheme;
	public Image topCornerLeft;
	public Image topCornerRight;
	public Image topCenter;
	public Image borderTopLeft;
	public Image borderLeft;
	public Image borderTopRight;
	public Image borderRight;
	public Image borderBottom;
	public Image backgroundBody;
	/// <summary>
	/// Changes the shell theme.
	/// </summary>
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
		RaiseEvent (OnCreate);
		if(initOnStart) Init (node);
	}

	/// <summary>
	/// Init the shell.
	/// </summary>
	/// <param name="node">Node this shell is referencing.</param>
	public void Init(Node node) {
		existingShells.Add (this);
		expanded = true;

		historyCommandIndex = 0;
		history = new List<string> ();
		
		resizeGameObject.SetActive (allowResize);

		this.node = node;
		UpdateAddress(node.rootFolder);
		currentOutputText = address;
		SetInput ("");

		FocusShell ();
	}

	/// <summary>
	/// Updates the address in which the shell is placed (folder and user).
	/// </summary>
	public void UpdateAddress(Folder folder) {
		this.folder = folder;
		path = folder.GetPathString ();
		address = user + "@" + pcName + ":" + path + "$ ";

		RaiseEventFull (OnUpdateAddress, folder.name);
	}

	/// <summary>
	/// Adds text to the current input.
	/// </summary>
	public void AddInput(string input) { //adds characters by Keyboard.cs
		currentInputText += input;
		bodyText.text = currentOutputText + currentInputText;
		UpdateTextMask ();
	}
	/// <summary>
	/// Sets the current input.
	/// </summary>
	public void SetInput(string input) {
		currentInputText = input;
		bodyText.text = currentOutputText + currentInputText;
		UpdateTextMask ();
	}
	/// <summary>
	/// Removes a character from the current input.
	/// </summary>
	public void RemoveInput() { //press Back key by Keyboard.cs
		if (currentInputText.Length > 0) {
			currentInputText = currentInputText.Remove (currentInputText.Length - 1);
			bodyText.text = currentOutputText + currentInputText;
			UpdateTextMask ();
		} 
	}
	/// <summary>
	/// Reads the current input, evaluates it and print its result.
	/// </summary>
	public void ReadInput() {
		if (currentInputText.Length == 0) return; //nothing written
		string[] splited = currentInputText.Split (new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

		PrintOutputNoAddress (currentInputText + Console.jump);
		RaiseEventFull (OnOutput, currentInputText);

		//tratar comando
		CommandStructure commandReturn = Console.ReadCommand (splited, this);
		history.Add (currentInputText);
		if (commandReturn.prompt) PrintOutputNoAddress (commandReturn.value);
		node.RaiseOnShellCommand (commandReturn);

		historyCommandIndex = history.Count;
		currentInputText = "";
		PrintAddress ();
	}
	/// <summary>
	/// Prints text with address and clears current input.
	/// </summary>
	/// <param name="output">Output text.</param>
	public void PrintOutput(string output) {
		currentOutputText += output;
		currentOutputText += address;
		bodyText.text = currentOutputText;
		UpdateTextMask ();
	}
	/// <summary>
	/// Prints the current address and clears current input.
	/// </summary>
	public void PrintAddress() {
		currentOutputText += Console.jump + address;
		bodyText.text = currentOutputText;
		UpdateTextMask ();
	}
	/// <summary>
	/// Prints text without address and clears current input.
	/// </summary>
	/// <param name="output">Output text.</param>
	public void PrintOutputNoAddress(string output) {
		currentOutputText += output;
		bodyText.text = currentOutputText;
		UpdateTextMask ();
	}
	/// <summary>
	/// Updates the RectTransforms anchores to display text properly.
	/// </summary>
	public void UpdateTextMask() {
		Canvas.ForceUpdateCanvases ();
		float f = bodyText.preferredHeight;
		contentTextRectTransform.anchoredPosition = Vector2.zero;
		if (f > maskRectTransform.rect.height) {
			contentTextRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, f);
		} else {
			contentTextRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, maskRectTransform.rect.height);
		}
	}

	/// <summary>
	/// Gets the previous command.
	/// </summary>
	public void GetPreviousCommand() {
		historyCommandIndex = Math.Max (0, historyCommandIndex - 1);
		SetInput (history [historyCommandIndex]);
	}
	/// <summary>
	/// Gets the next command.
	/// </summary>
	public void GetNextCommand() {
		historyCommandIndex = Math.Min (history.Count - 1, historyCommandIndex + 1);
		SetInput (history [historyCommandIndex]);
	}

	/// <summary>
	/// Closes the shell.
	/// </summary>
	public void CallbackClose() {
		RaiseEvent (OnClose);
		existingShells.Remove (this);
		UnfocusShell ();
		Destroy (gameObject);
	}
	/// <summary>
	/// Minimizes the shell.
	/// </summary>
	public void CallbackMinimize() {
		expanded = !expanded;
		bodyRectTransform.gameObject.SetActive (expanded);
		RaiseEvent (OnMinimize);
		if (!expanded)
			UnfocusShell ();
	}
	/// <summary>
	/// Maximizes the shell.
	/// </summary>
	public void CallbackMaximize() {
		//FocusShell ();
		print ("TODO: CallbackMaximize");
		RaiseEvent (OnMaximize);
	}

	float resizeStart;
	/// <summary>
	/// Prepares the shell for hight resize.
	/// </summary>
	public void CallbackResizeVerticalStart() {
		resizeStart = -shellRectTransform.anchoredPosition.y;
	}
	/// <summary>
	/// Updates the hight while resizing. 
	/// </summary>
	public void CallbackResizeVertical() {
		float height = (1080f-Input.mousePosition.y) - resizeStart;
		if (height < 100f)
			return;
		shellRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);
	}
	/// <summary>
	/// Prepares the shell for width resize.
	/// </summary>
	public void CallbackResizeHorizontalStart() {
		resizeStart = shellRectTransform.anchoredPosition.x;
	}
	/// <summary>
	/// Updates the width while resizing. 
	/// </summary>
	public void CallbackResizeHorizontal() {
		float width = Input.mousePosition.x - resizeStart;
		if (width < 200f)
			return;
		shellRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
	}
	/// <summary>
	/// Finishes resize calculations.
	/// </summary>
	public void CallbackResizeEnd() {
		UpdateTextMask ();
	}
	public void HoverResizeEnter() {
		print ("Hover resize Enter");
	}
	public void HoverResizeExit() {
		print ("Hover resize Exit");
	}

	/// <summary>
	/// Determines whether this instance is containing the specified position.
	/// </summary>
	/// <returns><c>true</c> if this instance is containing the specified position; otherwise, <c>false</c>.</returns>
	public bool IsMouseOver(Vector2 mousePos) {
		return (RectTransformUtility.RectangleContainsScreenPoint(shellRectTransform, mousePos));
	}

	/// <summary>
	/// Focuses the shell, making it read keyboard input. If Ctrl key is not held down, unfocuses other shells.
	/// </summary>
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
	/// <summary>
	/// Unfocuses the shell.
	/// </summary>
	public void UnfocusShell() {
		focus = false;
		RaiseEvent (OnUnfocus);
		if(focusedShells.Contains(this)) {
			focusedShells.Remove (this);
		}
  	}

	/// <summary>
	/// Unfocuses all shells.
	/// </summary>
	public static void UnfocusAll() {
		while (focusedShells.Count > 0)
			focusedShells [0].UnfocusShell ();
	}
	/// <summary>
	/// Closes all shells.
	/// </summary>
	public static void CloseAll() {
		while (existingShells.Count > 0)
			existingShells [0].CallbackClose ();
	}
	/// <summary>
	/// Minimizes all shells.
	/// </summary>
	public static void MinimizeAll() {
		foreach(Shell s in existingShells)
			s.CallbackMinimize ();
	}

	/// <summary>
	/// Prepares the shell for being draged.
	/// </summary>
	public void DragHeaderStart() {
		dragOffset = transform.position - Input.mousePosition;
	}
	/// <summary>
	/// Updates position while draging.
	/// </summary>
	public void DragHeader() {
		transform.position = Input.mousePosition + dragOffset;
	}
}
