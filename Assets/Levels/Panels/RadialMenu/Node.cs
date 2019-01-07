using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RadialMenu {

	public class Node : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

		public GameObject[] childs;
		public Image graphicTarget;

		[HideInInspector] public CanvasGroup canvasGroup;
		[HideInInspector] public Node[] siblings;
		[HideInInspector] public Node siblingShown;
		[HideInInspector] public NodeGroup siblingGroup;

		void Start() {
			canvasGroup = gameObject.AddComponent<CanvasGroup> ();
			canvasGroup.alpha = 0.7f;
			if (graphicTarget != null)
				graphicTarget.alphaHitTestMinimumThreshold = 0.5f;
		}

		public void OnPointerEnter(PointerEventData data) {
			if (siblingShown == this)
				return;
			else if (siblingShown != null)
				siblingShown.OnSiblingEntered ();

			foreach(Node n in siblings)
				n.siblingShown = this;
			siblingShown = this;
			foreach (GameObject g in childs)
				g.SetActive (true);

			StartCoroutine (Routines.DoWhile (0.1f, delegate(float f) {
				canvasGroup.alpha = 0.7f + f * 0.3f;
			}));
		}

		public void OnSiblingEntered() {
			foreach (GameObject g in childs)
				g.SetActive (false);

			StartCoroutine (Routines.DoWhile (0.1f, delegate(float f) {
				canvasGroup.alpha = 1f - f * 0.3f;
			}));
		}
		
		public void OnPointerClick(PointerEventData data) {
			OnPointerClick2 (data);
		}
		protected virtual void OnPointerClick2(PointerEventData data) {
			print ("Click on" + gameObject.name);
		}
	}

}