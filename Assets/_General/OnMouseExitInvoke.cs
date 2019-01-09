using UnityEngine;
using UnityEngine.Events;

public class OnMouseExitInvoke : MonoBehaviour {
	public UnityEvent method;

	void OnMouseExit() {
		method.Invoke ();
	}
}
