using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Manager;

public class Permanent : MonoBehaviour {
	
	public GameObject soundPrefab;
	[Range(0f,1f)]
	public float maxMaster, maxMusic, maxEffects, maxVoice;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		Sound.Init (soundPrefab, maxMusic, maxEffects, maxVoice, maxMaster);

		Level.OnEnd += OnEndLevel;
		Level.OnStart += OnStartLevel;
	}
	void Start() {
		Sound.SetAllVolumes (User.volume_master, User.volume_music, User.volume_effects, User.volume_voice);
	}

	SoundSource levelBackground;
	public void OnStartLevel(Level level) {
		if (levelBackground != null) {
			levelBackground.Stop (1f);
			levelBackground = null;
		}
		levelBackground = Sound.PlayBackground (level.backgroundMusic, Vector3.zero);
		levelBackground.transform.localPosition = Camera.main.transform.position;
	}

	public void OnEndLevel(Level level) {
		if (levelBackground != null) {
			levelBackground.Stop (1f);
			levelBackground = null;
		}

		User.ClearLevel(Scenes.activeScene);
		User.SaveConfiguration ();

		StartCoroutine (Routines.WaitFor (1f, delegate {
			Scenes.LoadSceneAdditiveMerge ("FinishScreen");
		}));
	}

}
