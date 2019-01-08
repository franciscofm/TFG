using UnityEngine;

using Manager;

public class Main : MonoBehaviour {

	public void CallbackSelectLevel() {
		Scenes.LoadScene ("SelectLevel");
	}
	public void CallbackConfiguration() {
		Scenes.LoadSceneAdditiveMerge ("Configuration");
	}
	public void CallbackExit() {
		Scenes.ExitGame ();
	}
}
