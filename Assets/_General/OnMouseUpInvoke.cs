using UnityEngine;
using UnityEngine.Events;

public class OnMouseUpInvoke : MonoBehaviour {
	public UnityEvent method;

	void OnMouseUp() {
		method.Invoke ();
	}
}
