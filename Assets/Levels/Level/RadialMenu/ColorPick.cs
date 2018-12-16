using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RadialMenu {

	public class ColorPick : Node, IDragHandler, IBeginDragHandler {

		public Center center;
		RectTransform rect;

		void Start() {
			canvasGroup = gameObject.AddComponent<CanvasGroup> ();
			canvasGroup.alpha = 0.7f;
			rect = transform as RectTransform;
			GetComponent<UnityEngine.UI.Image> ().alphaHitTestMinimumThreshold = 0.5f;
		}

		public void OnBeginDrag(PointerEventData data) {
			OnDrag (data);
		}
		public void OnDrag(PointerEventData data) {
			Vector2 localCursor;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle (rect, data.position, null, out localCursor))
				return;

			float ypos = localCursor.y;
			ypos += rect.rect.height / 2;
			ypos /= rect.rect.height;


			Color hsv = Color.HSVToRGB (ypos, 1f, 1f);
			center.CallbackColorPick (hsv);
		}
	}

}
