using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Keyboard), typeof(AparenceHolder))]
public class Level : MonoBehaviour {

	public GameObject radialMenuPrefab;
	public Transform canvasTransform;
	public GameObject interfaceInfoPrefab;

	Dictionary<Node,Pair> nodeMenus;
	List<Node> allNodes;
	List<Interface> allInterfaces;

	// Use this for initialization
	void Start () {
		nodeMenus = new Dictionary<Node, Pair> ();
		Node.OnClickUpStatic += OnNodeClick;

		allNodes = Node.allNodes;
		allInterfaces = new List<Interface> ();
		foreach (Node n in allNodes)
			foreach (Interface i in n.Interfaces)
				allInterfaces.Add (i);

		foreach (InterfaceVisuals iv in InterfaceVisuals.allVisuals) {
			iv.infoObject = Instantiate (interfaceInfoPrefab, canvasTransform);
			iv.InitVisuals ();
		}
	}

	public void OnNodeClick(Node node) {
		if (nodeMenus.ContainsKey (node)) {
			nodeMenus [node].center.transform.SetAsLastSibling ();
			return;
		}

		GameObject radial = Instantiate (radialMenuPrefab, canvasTransform);
		Center center = radial.GetComponent<Center> ();
		NodeVisuals visuals = node.GetComponent<NodeVisuals> ();

		center.canvasTransform = canvasTransform;
		center.node = node;
		center.level = this;
		nodeMenus.Add (node, new Pair(center,visuals));
	}
	public void OnCenterClose(Node node) {
		nodeMenus.Remove (node);
	}

	public GameObject shellPrefab;
	[HideInInspector] public GameObject shellInstance;
	public void CallbackCreateShell(RectTransform rect, Node node) {
		if (shellPrefab != null) {
			shellInstance = Instantiate (shellPrefab, canvasTransform);
			shellInstance.GetComponent<Shell> ().Init (node);
		}
	}
	public void CallbackColorPick(Color c, Node node) {
		nodeMenus [node].visuals.ChangeNodeColor (c);
	}

	public GameObject ifconfigPrefab;
	[HideInInspector] public GameObject ifconfigInstance;
	public void CallbackIfconfig(RectTransform rect, Node node) {
		if (ifconfigPrefab != null) {
			if (ifconfigInstance != null)
				Destroy (ifconfigInstance);
			ifconfigInstance = Instantiate (ifconfigPrefab, canvasTransform);
			ifconfigInstance.GetComponent<Panel.Ifconfig> ().node = node;
		}
	}
	public GameObject pingPrefab;
	[HideInInspector] public GameObject pingInstance;
	public void CallbackPing(RectTransform rect, Node node) {
		if (pingPrefab != null) {
			if (pingInstance != null)
				Destroy (pingInstance);
			pingInstance = Instantiate (pingPrefab, canvasTransform);
			pingInstance.SetActive (true);
			Panel.Ping ping = pingInstance.GetComponent<Panel.Ping> ();
			ping.node = node;
			ping.allInterfaces = allInterfaces;
		}
	}
	public GameObject routePrefab;
	[HideInInspector] public GameObject routeInstance;
	public void CallbackRoute(RectTransform rect, Node node) {
		if (routePrefab != null) {
			if (routeInstance != null)
				Destroy (routeInstance);
			routeInstance = Instantiate (routePrefab, canvasTransform);
			routeInstance.GetComponent<Panel.Route> ().node = node;
		}
	}
	public GameObject manualPrefab;
	[HideInInspector] public GameObject manualInstance;
	public void CallbackManual(RectTransform rect) {
		if (manualPrefab != null) {
			manualInstance = Instantiate (manualPrefab, canvasTransform);
		} else {
			manualInstance.transform.SetAsLastSibling ();
		}
	}

	class Pair {
		public Center center;
		public NodeVisuals visuals;
		public Pair(Center center, NodeVisuals visuals) {
			this.center = center;
			this.visuals = visuals;
		}
	}
}
