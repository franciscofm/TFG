using UnityEngine;
using UnityEngine.Events;

public class OnMouseEnterInvoke : MonoBehaviour {
	public UnityEvent method;

	void OnMouseEnter() {
		method.Invoke ();
	}
}
