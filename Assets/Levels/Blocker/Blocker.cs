using UnityEngine;
using UnityEngine.UI;

namespace UtilsBlocker {

	public class Blocker : MonoBehaviour {
		
		public Transform focusTransform;

		public Image focusImage;
		public RectTransform focusRect;

		void Start() {
			focusRect = focusImage.transform as RectTransform;
		}

		public void Block(bool yes) {
			focusImage.enabled = yes;
		}

		public void SetPosition(Vector3 screenPos) {
			focusTransform.position = screenPos;
		}
		public void SetAnchoredPos(Vector2 anchoredPos) {
			focusRect.anchoredPosition = anchoredPos;
		}
		public void SetSize(Vector2 size) {
			focusRect.sizeDelta = size;
		}
		public void SetPosition(RectTransform rect) {

		}
		public void SetPosition(Vector3 screenPos, Vector2 size) {
			focusTransform.position = screenPos;
			focusRect.sizeDelta = size;
		}
	}

}