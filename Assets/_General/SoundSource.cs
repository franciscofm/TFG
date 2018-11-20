using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Manager;

public class SoundSource : MonoBehaviour {

	public AudioSource source;
	public AudioClip clip;
	public bool loop;
	public bool paused;

	public List<SoundSource> reference;
	public int channel;

	public void Init (AudioClip clip, bool loop, int channel, List<SoundSource> reference) {
		this.clip = clip;
		this.loop = loop;
		this.channel = channel;
		this.reference = reference;

		source.clip = clip;
		source.loop = loop;
	}

	public void Play () {
		source.Play ();
		if(!loop)
			StartCoroutine (Routines.WaitFor(clip.length, delegate {
				Stop();
			}));
	}

	public void Pause(bool paused) {
		this.paused = paused;
        if (source == null)
            return; // Janster was here
		if (paused) source.Pause ();
		else source.UnPause ();
	}

	/// <summary>
	/// Stops the sound source. If duration stated sound fades before destroying.
	/// </summary>
	public void Stop (float duration = 0f) {
		if (duration > 0f) {
			StartCoroutine (StopRoutine (duration));
			return;
		}
		Sound.StopSound (this);
	}
	IEnumerator StopRoutine(float duration) {
		float t = 0;
		float v = source.volume;
		while (t < duration) {
			yield return null;
			t += Time.deltaTime;
			source.volume = v * (1f - (t / duration));
		}
		Sound.StopSound (this);
	}

	void OnDestroy() {
		StopAllCoroutines ();
	}
}
