using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Manager;

public class Main : MonoBehaviour {

	void Start() {
		User.LoadConfiguration ();
	}

	public void CallbackSelectLevel() {
		Scenes.LoadScene ("SelectLevel");
	}
	public void CallbackConfiguration() {
		Scenes.LoadScene ("Configuration");
	}
	public void CallbackExit() {
		Scenes.ExitGame ();
	}
}
