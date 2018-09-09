using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShell : MonoBehaviour {

	public List<Node> nodes;
	public List<Shell> shells;

	public void Init () {
		//Structure init
		this.nodes = new List<Node> ();
		this.shells = new List<Shell> ();

		//Get starting nodes & shells in scene
		Node[] nodes = GetComponentsInChildren<Node> (true);
		foreach (Node n in nodes)
			this.nodes.Add (n);
		Shell[] shells = GetComponentsInChildren<Shell> (true);
		foreach (Shell s in shells)
			this.shells.Add (s);

	}

	public void ShowInfoNode(Node node) {

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
