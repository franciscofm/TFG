using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererAnimator : MonoBehaviour {

	public LineRenderer[] top;
	public LineRenderer[] mid;
	public LineRenderer[] bot;

	void Start() {
		AnimateLineGroupUp (bot, 6f);
		StartCoroutine (Routines.WaitFor (6f, delegate {
			AnimateLineGroup (mid, 3f);
		}));
		StartCoroutine (Routines.WaitFor (9f, delegate {
			AnimateLineGroupUp (top, 6f);
		}));
	}
	void OnDestroy() {
		StopAllCoroutines ();
	}

	void AnimateLineGroupUp(LineRenderer[] lines, float duration) {
		Vector3[] starts = new Vector3[lines.Length], ends = new Vector3[lines.Length];
		for (int i = 0; i < lines.Length; ++i) {
			Vector3 v1 = lines [i].GetPosition (0);
			Vector3 v2 = lines [i].GetPosition (1);
			if (v1.z < v2.z) {
				starts [i] = v1;
				ends [i] = v2;
				lines [i].SetPosition (0, v1);
				lines [i].SetPosition (1, v1);
			} else {
				starts [i] = v2;
				ends [i] = v1;
				lines [i].SetPosition (0, v2);
				lines [i].SetPosition (1, v2);
			}
			lines [i].enabled = true;
		}
		StartCoroutine (AnimationRoutine (lines, starts, ends, duration));
	}
	void AnimateLineGroup(LineRenderer[] lines, float duration) {
		Vector3[] starts = new Vector3[lines.Length], ends = new Vector3[lines.Length];
		for (int i = 0; i < lines.Length; ++i) {
			Vector3 v1 = lines [i].GetPosition (0);
			starts [i] = v1;
			ends [i] = lines [i].GetPosition (1);
			lines [i].SetPosition (0, v1);
			lines [i].SetPosition (1, v1);
			lines [i].enabled = true;
		}
		StartCoroutine (AnimationRoutine (lines, starts, ends, duration));
	}

	IEnumerator AnimationRoutine(LineRenderer[] lines, Vector3[] starts, Vector3[] ends, float duration) {
		float t = 0f;
		while (t < duration) {
			yield return null;
			t += Time.deltaTime;
			float p = t / duration;
			for (int i = 0; i < lines.Length; ++i) 
				lines [i].SetPosition (1, Vector3.Lerp (starts [i], ends [i], p));
		}
	}
}
