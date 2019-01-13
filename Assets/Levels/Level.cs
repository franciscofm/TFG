using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsSpeech;
using UtilsBlocker;

[RequireComponent(typeof(Keyboard), typeof(AparenceHolder))]
public class Level : MonoBehaviour {

	public Camera cam;
	public Speech speech;
	public Blocker blocker;
	public Keyboard keyboard;
	public ShortcutPanel shortcutPanel;
	public Transform canvasTransform;
	public GameObject interfaceInfoPrefab;
	public GameObject shellPrefab;
	[Header("Panels")]
	public GameObject radialMenuPrefab;
	public GameObject ifconfigPrefab;
	public GameObject pingPrefab;
	public GameObject routePrefab;
	public GameObject manualPrefab;

	[Header("Level dependent")]
	public Node[] allNodes;
	[HideInInspector] public List<Interface> allInterfaces;
	[HideInInspector] public List<InterfaceVisuals> allIfaceVisuals;
	protected Dictionary<Node,Pair> nodePanels;
	public struct Pair {
		public Center center;
		public NodeVisuals visuals;
		public Pair(Center center, NodeVisuals visuals) {
			this.center = center;
			this.visuals = visuals;
		}
	}

	protected virtual void Start2() {	}
	protected virtual void End2() {	}

	public delegate void LevelEvent(Level Level);
	public static event LevelEvent OnEnd;
	public static event LevelEvent OnStart;

	// Use this for initialization
	void Start () {
		nodePanels = new Dictionary<Node, Pair> ();
		Node.OnClickUpStatic += OnNodeClick;

		allInterfaces = new List<Interface> ();
		allIfaceVisuals = new List<InterfaceVisuals> ();
		foreach (Node n in allNodes) {
			n.Load ();
			foreach (Interface i in n.Interfaces) {
				InterfaceVisuals iv = i.GetComponent<InterfaceVisuals> ();
				allInterfaces.Add (i);
				allIfaceVisuals.Add (iv);

				iv.iface = i;
				iv.infoObject = Instantiate (interfaceInfoPrefab, canvasTransform);
				iv.InitVisuals ();
			}
		}

		keyboard.shortcutPanel = shortcutPanel;
		keyboard.allVisuals = allIfaceVisuals;
		shortcutPanel.allVisuals = allIfaceVisuals;

		if (OnStart != null) OnStart (this);
		Start2 ();
	}
	void OnDestroy() {
		Node.OnClickUpStatic -= OnNodeClick;
	}

	protected void End() {
		if (OnEnd != null) OnEnd (this);

		End2 ();
	}

	public void OnNodeClick(Node node) {
		if (nodePanels.ContainsKey (node)) {
			nodePanels [node].center.transform.SetAsLastSibling ();
			return;
		}

		GameObject radial = Instantiate (radialMenuPrefab, canvasTransform);
		Center center = radial.GetComponent<Center> ();
		NodeVisuals visuals = node.GetComponent<NodeVisuals> ();

		center.node = node;
		center.level = this;
		center.transform.position = cam.WorldToScreenPoint(visuals.meshRenderer.transform.position) + new Vector3(0,150f);
		nodePanels.Add (node, new Pair(center,visuals));
	}
	public void OnCenterClose(Node node) {
		nodePanels.Remove (node);
	}

	[HideInInspector] public GameObject shellInstance;
	public void CallbackCreateShell(RectTransform rect, Node node) {
		if (shellPrefab != null) {
			shellInstance = Instantiate (shellPrefab, canvasTransform);
			shellInstance.GetComponent<Shell> ().Init (node);
		}
	}
	public void CallbackColorPick(Color c, Node node) {
		nodePanels [node].visuals.ChangeNodeColor (c);
	}

	[HideInInspector] public GameObject ifconfigInstance;
	public void CallbackIfconfig(RectTransform rect, Node node) {
		if (ifconfigPrefab != null) {
			if (ifconfigInstance != null)
				Destroy (ifconfigInstance);
			ifconfigInstance = Instantiate (ifconfigPrefab, canvasTransform);
			ifconfigInstance.GetComponent<Panel.Ifconfig> ().node = node;
		}
	}
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
	[HideInInspector] public GameObject routeInstance;
	public void CallbackRoute(RectTransform rect, Node node) {
		if (routePrefab != null) {
			if (routeInstance != null)
				Destroy (routeInstance);
			routeInstance = Instantiate (routePrefab, canvasTransform);
			routeInstance.GetComponent<Panel.Route> ().node = node;
		}
	}
	[HideInInspector] public GameObject manualInstance;
	public void CallbackManual(RectTransform rect) {
		if (manualPrefab != null) {
			if (manualInstance != null) {
				manualInstance = Instantiate (manualPrefab, canvasTransform);
			} else {
				manualInstance.transform.SetAsLastSibling ();
			}
		}
	}
}
