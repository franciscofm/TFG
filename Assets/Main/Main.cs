using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	public void CallbackSelectLevel() {
		SceneManager.LoadScene ("SelectLevel");
	}
	public void CallbackConfiguration() {
		SceneManager.LoadScene ("Configuration");
	}
	public void CallbackExit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
