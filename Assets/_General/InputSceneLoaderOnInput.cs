using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class InputSceneLoaderOnInput : MonoBehaviour {

	public string scene = "Main";
	[Space]
	public bool anyKey = true;
	public KeyCode[] triggerKeys;
	public int[] triggerMouseButtons;
	
	// Update is called once per frame
	void Update () {
		if (anyKey) {
			if (Input.anyKeyDown)
				GoToScene ();
		} else {
			foreach (int i in triggerMouseButtons)
				if (Input.GetMouseButtonDown (i)) {
					GoToScene ();
					return;
				}
			foreach(KeyCode k in triggerKeys)
				if(Input.GetKeyDown(k)) {
					GoToScene ();
					return;
				}
		}
	}

	void GoToScene() {
		SceneManager.LoadScene (scene);
	}
}
