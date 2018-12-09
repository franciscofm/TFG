using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace RadialMenu {

	public class InvokeOnClick : Node {

		[Space]
		public UnityEvent method;
		
		protected override void OnPointerClick2 (UnityEngine.EventSystems.PointerEventData data) {
			method.Invoke ();
		}
	}

}