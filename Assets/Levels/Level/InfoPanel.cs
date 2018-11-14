using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {

	public class InfoPanel : MonoBehaviour {

		public static MenuShell menu;
		public RectTransform rect;
		public GameObject shellPrefab;
		Node node;

		public Text header;
		public Text[] ifacesName;
		public Text[] ifacesIP;
		public Image[] ifacesUp;

		public void Init(Node node) {
			this.node = node;
			header.text = "Node: " + node.name;
			UpdateValues ();
		}
		public void UpdateValues() { //todo update when node evaluates commands
			for (int i = 0; i < 3; ++i) {
				Interface iface = node.Interfaces[i];
				ifacesName [i].text = iface.Name;
				ifacesIP [i].text = iface.ip.word;
				ifacesUp [i].color = iface.isUp ? Color.green : Color.red;
			}
		}

		//Buttons
		public void CallbackShell() {
			GameObject shell = Instantiate (shellPrefab, transform.parent);
			shell.transform.position = transform.position;

			Shell s = shell.GetComponent<Shell> ();
			s.node = this.node;
		}
		public void CallbackClose() {
			menu.ClosePanel (node);
			Destroy (gameObject);
		}

		Vector3 dragOffset;
		public void DragStart() {
			dragOffset = transform.position - Input.mousePosition;
		}
		public void Drag() {
			transform.position = Input.mousePosition + dragOffset;
		}
	}

}