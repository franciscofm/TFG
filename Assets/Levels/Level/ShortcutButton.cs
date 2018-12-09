using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ShortcutButton : MonoBehaviour {

	public Image icon;
	public GameObject text;

	public void Expand() {
		text.SetActive (true);
	}

	public void Collapse() {
		text.SetActive (false);
	}
}
