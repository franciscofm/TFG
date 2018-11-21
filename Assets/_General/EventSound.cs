using UnityEngine;
using Manager;

public class EventSound : MonoBehaviour {

	public AudioClip sound;

	public void PlaySound() {
		Sound.PlayEffect (sound, transform.localPosition); //TODO precise position
	}
}
