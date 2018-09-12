using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {

	public class MenuShell : MonoBehaviour {

		public List<Node> nodes;
		public List<Shell> shells;

		public GameObject infoPrefab;

		Dictionary<Node, InfoPanel> openedPanels;

		void Awake () {
			//Structure init
			this.nodes = new List<Node> ();
			this.shells = new List<Shell> ();
			this.openedPanels = new Dictionary<Node, InfoPanel>();

			Node.OnClickUp += ShowInfoNode;
			InfoPanel.menu = this;
		}

		public void ShowInfoNode(Node node) {
			if (openedPanels.ContainsKey (node)) {
				openedPanels [node].transform.SetAsLastSibling ();
				openedPanels [node].transform.position = Input.mousePosition;
			} else {
				GameObject info = Instantiate (infoPrefab, transform);
				info.transform.position = Input.mousePosition;
				InfoPanel p = info.GetComponent<InfoPanel> ();
				p.Init (node);
				openedPanels.Add (node, p);
			}
		}
		public void ClosePanel(Node node) {
			openedPanels.Remove (node);
		}

		public void CloseAllShells() {
			while (shells.Count > 0) {
				shells [0].CallbackClose ();
				shells.RemoveAt (0);
			}
		}
		public void MinimizeAllShells() {
			bool minimize = false;
			for(int i=0; i<shells.Count && !minimize;++i) {
				if (shells[i].expanded) {
					minimize = true;
					for(int j=i; j<shells.Count; ++j)
						if (shells[j].expanded) shells[j].CallbackMinimize ();
				}
			}
			if (!minimize) {
				foreach (Shell s2 in shells) {
					if (!s2.expanded) s2.CallbackMinimize ();
				}
			}
		}
		public void MaximizeAllShells() {
			print("TODO implementar MaximizeAllShells");
		}
	}

}