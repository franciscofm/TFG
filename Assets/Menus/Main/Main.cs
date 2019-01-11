using UnityEngine;

using Manager;

public class Main : MonoBehaviour {

	public CanvasGroup mainCanvas;

	bool inSelectLevel = false;
	public void CallbackSelectLevel() {
		inSelectLevel = true;
		Scenes.LoadSceneAdditiveMerge ("SelectLevel");
	}
	public void CallbackConfiguration() {
		Scenes.LoadSceneAdditiveMerge ("Configuration");
	}
	public void CallbackExit() {
		Scenes.ExitGame ();
	}
}
