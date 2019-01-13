using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Manager;

public class FinishScreen : MonoBehaviour {

	public Text timerText;
	public float duration = 5f;
	IEnumerator routine;
	// Use this for initialization
	void Start () {
		StartCoroutine (routine = TimerRoutine ());
	}

	IEnumerator TimerRoutine() {
		while (duration >= 0f) {
			timerText.text = Mathf.CeilToInt (duration).ToString ();
			yield return new WaitForSeconds (1f);
			duration -= 1f;
		}
		yield return new WaitForSeconds (1f);
		Scenes.LoadScene ("Main");
	}

	public void GoNowCallback() {
		StopCoroutine (routine);
		Scenes.LoadScene ("Main");
	}

	public void StayCallback() {
		StopCoroutine (routine);
		Destroy (gameObject);
	}
}
