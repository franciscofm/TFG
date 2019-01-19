using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RTPosFixer))]
public class EditorRTPosFixer : Editor {

	RTPosFixer script;
	void OnEnable() {
		script = target as RTPosFixer;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();
		if (GUILayout.Button ("Update values")) {
			script.UpdateValues ();
		}
		if (GUILayout.Button ("Update position")) {
			script.UpdatePosition ();
		}

	}

}
