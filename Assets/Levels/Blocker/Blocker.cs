using UnityEngine;
using UnityEngine.UI;

namespace UtilsBlocker {

	public class Blocker : MonoBehaviour {
		
		public Transform focusTransform;
		public Image focusImage;

		public void Block(bool yes) {
			focusImage.enabled = yes;
		}

		public void SetPosition(Vector3 screenPos) {
			focusTransform.position = screenPos;
		}
		public void SetSize(Vector2 size) {
			(focusImage.transform as RectTransform).sizeDelta = size;
		}
		public void SetPosition(RectTransform rect) {

		}
		public void SetPosition(Vector3 screenPos, Vector2 size) {
			focusTransform.position = screenPos;
			(focusImage.transform as RectTransform).sizeDelta = size;
		}
	}

}