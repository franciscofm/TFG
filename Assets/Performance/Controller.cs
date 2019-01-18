using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Performance {

	public class Controller : MonoBehaviour {

		public int totalNodesGridSize;
		public float offsetNodes = 1;
		public GameObject nodePrefab;
		public GameObject interfaceInfoPrefab;
		public Transform canvasTransform;
		Node[] nodes;
		NodeVisuals[] nodesVisuals;
		Interface[] interfaces;
		InterfaceVisuals[] interfacesVisuals;
		int size;

		public void LoadNodes() {
			size = totalNodesGridSize * totalNodesGridSize;
			nodes = new Node[size];
			nodesVisuals = new NodeVisuals[size];
			interfaces = new Interface[size * 3];
			interfacesVisuals = new InterfaceVisuals[size * 3];
			StartCoroutine (LoadNodesRoutine ());
		}
		IEnumerator LoadNodesRoutine() {
			int c = 0;
			for (int i = 0; i < totalNodesGridSize; ++i) {
				for (int j = 0; j < totalNodesGridSize; ++j) {
					yield return null;
					GameObject go = Instantiate (nodePrefab, transform);
					go.transform.position = new Vector3 (offsetNodes * i, 0, offsetNodes * j);
					nodes [c] = go.GetComponent<Node> ();
					++c;
				}
			}
		}

		public void InitNodes() {
			StartCoroutine (InitNodesRoutine ());
		}
		IEnumerator InitNodesRoutine() {
			for (int i = 0; i < size; ++i) {
				yield return null;
				Node n = nodes [i];
				NodeVisuals nv = GetComponent<NodeVisuals> ();
				nodesVisuals [i] = nv;
				n.Load ();
				nv.Load ();
				yield return null;

				for (int j = 0; j < 3; ++j) {
					Interface iface = n.Interfaces [j];
					interfaces [i * 3 + j] = iface;
					InterfaceVisuals iv = iface.GetComponent<InterfaceVisuals> ();
					interfacesVisuals [i * 3 + j] = iv;

					iv.iface = iface;
					iv.infoObject = Instantiate (interfaceInfoPrefab, canvasTransform);
					iv.InitVisuals ();
				}
			}
		}

		public void OpenPanels() {

		}

		public void OpenInfo() {

		}

	}

}