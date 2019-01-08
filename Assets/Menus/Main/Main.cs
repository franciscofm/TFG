using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Manager;

public class Main : MonoBehaviour {

	public GameObject soundPrefab;
	[Range(0f,1f)]
	public float maxMaster, maxMusic, maxEffects, maxVoice;

	void Start() {
		Scenes.Initialize ();
		User.LoadConfiguration ();
		Sound.Init (soundPrefab, maxMusic, maxEffects, maxVoice, maxMaster);
		Sound.SetAllVolumes (User.volume_master, User.volume_music, User.volume_effects, User.volume_voice);
	}

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
