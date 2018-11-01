using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour {

	public Image image;
	public Animator animator;
	public string outName = "Out";
	public float outDuration = 0.5f;
	public string inName = "In";

	public LevelEntry currentLevelEntry;
	new bool enabled = false;

	public void SetLevel(LevelEntry entry) {
		currentLevelEntry = entry;
		if (enabled) {
			animator.Play (outName);
			enabled = false;
			StartCoroutine (Routines.WaitFor (outDuration, delegate {
				image.sprite = entry.levelImage.sprite;
				animator.Play (inName);
				enabled = true;
			}));
		} else {
			StartCoroutine (Routines.WaitFor (outDuration, delegate {
				image.sprite = entry.levelImage.sprite;
				animator.Play (inName);
				enabled = true;
			}));
		}
	}

	public void CallbackPlay() {
		if(enabled)
			UnityEngine.SceneManagement.SceneManager.LoadScene (currentLevelEntry.scene);
	}
}
