using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler {

	public Transform parent;

	Vector3 offset;
	public void OnBeginDrag(PointerEventData data) {
		offset = Input.mousePosition - parent.position;
	}
	public void OnDrag(PointerEventData data) {
		parent.position = Input.mousePosition - offset;
	}
}
