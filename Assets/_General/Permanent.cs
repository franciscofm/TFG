using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Manager;

public class Permanent : MonoBehaviour {
	
	public GameObject soundPrefab;
	[Range(0f,1f)]
	public float maxMaster, maxMusic, maxEffects, maxVoice;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);

		Sound.Init (soundPrefab, maxMusic, maxEffects, maxVoice, maxMaster);
		Sound.SetAllVolumes (User.volume_master, User.volume_music, User.volume_effects, User.volume_voice);

		Level.OnEnd += OnEndLevel;
	}

	public static void OnEndLevel(Level level) {
		User.ClearLevel(Scenes.activeScene);

		level.StartCoroutine (Routines.WaitFor (2f, delegate {
			Scenes.LoadScene ("SelectLevel");
		}));
	}

}
