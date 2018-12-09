using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadialMenu {

	public class NodeGroup : MonoBehaviour {

		public Node[] nodes;

		void Awake() {
			if (nodes.Length <= 1)
				return;

			for (int i = 0; i < nodes.Length; ++i) {
				Node[] otherNodes = new Node[nodes.Length - 1];
				for(int n=0, j=0; n<nodes.Length; ++n) {
					if (n != i) {
						otherNodes [j] = nodes [n];
						++j;
					}
				}
				nodes [i].siblings = otherNodes;
				nodes [i].siblingGroup = this;
			}
		}

		public void ActivateNodes() {
			foreach (Node n in nodes)
				n.gameObject.SetActive (true);
		}
	}

}