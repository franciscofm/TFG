using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public GameObject target;

	public void OnPointerEnter(PointerEventData data) {
		target.SetActive (true);
	}

	public void OnPointerExit(PointerEventData data) {
		target.SetActive (false);
	}
}
