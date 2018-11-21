using UnityEngine;
using UnityEngine.Events;

public class OnMouseDownInvoke : MonoBehaviour {
	public UnityEvent method;

	void OnMouseDown() {
		method.Invoke ();
	}
}
