using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	public float minDistance = 5f;
	public Transform parent;

	Vector3 offset, start;
	bool draged = false;
	public void OnBeginDrag(PointerEventData data) {
		start = Input.mousePosition;
		offset = start - parent.position;
		draged = false;
	}
	public void OnDrag(PointerEventData data) {
		Vector3 mouse = Input.mousePosition;
		if(draged || Vector3.Distance(start, mouse) > minDistance) {
			draged = true;
			parent.position = mouse - offset;
		}
	}

	public void OnPointerDown(PointerEventData data) {
	}
	public void OnPointerUp(PointerEventData data) {
		if (!draged) Destroy (parent.gameObject);
	}
}
