using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Manager;

public class IngameMenu : MonoBehaviour {

	public void CallbackContinue() {
		Destroy (gameObject);
	}
	public void CallbackExitLevel() {
		Scenes.LoadScene ("SelectLevel");
	}
	public void CallbackExitGame() {
		Scenes.ExitGame ();
	}
}
