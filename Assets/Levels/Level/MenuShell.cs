using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {

	public class MenuShell : MonoBehaviour {

		public GameObject infoPrefab;

		Dictionary<Node, InfoPanel> openedPanels;

		void Awake () {
			//Structure init
			openedPanels = new Dictionary<Node, InfoPanel>();

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
			while (Shell.existingShells.Count > 0)
				Shell.existingShells[0].CallbackClose ();
		}
		public void MinimizeAllShells() {
			bool minimize = false;
			for(int i=0; i<Shell.existingShells.Count && !minimize;++i) {
				if (Shell.existingShells[i].expanded) {
					minimize = true;
					for(int j=i; j<Shell.existingShells.Count; ++j)
						if (Shell.existingShells[j].expanded) Shell.existingShells[j].CallbackMinimize ();
				}
			}
			if (!minimize) {
				foreach (Shell s2 in Shell.existingShells) {
					if (!s2.expanded) s2.CallbackMinimize ();
				}
			}
		}
		public void MaximizeAllShells() {
			print("TODO implementar MaximizeAllShells");
		}
	}

}