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

	[HideInInspector] public LevelEntry currentLevelEntry;

	public void SetLevel(LevelEntry entry) {
		currentLevelEntry = entry;
		animator.Play (outName);
		Routines.WaitFor (outDuration, delegate {
			this.image.sprite = entry.levelImage.sprite;
			animator.Play (inName);
		});
	}

	public void CallbackPlay() {
		UnityEngine.SceneManagement.SceneManager.LoadScene (currentLevelEntry.scene);
	}
}
