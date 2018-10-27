using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfocusArea : MonoBehaviour {

	void OnMouseUp() {
		Shell.UnfocusAll ();
	}
}
