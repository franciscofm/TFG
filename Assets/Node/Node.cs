using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public Interface[] Interfaces = new Interface[] {
		new Interface("192.168.60.1","255.255.255.255","192.168.60.1")
	};
	public Connection[] Connections;

	public List<RouteEntry> RouteTable;
	public List<ARPEntry> ARPTable;

	public enum Type { Pc, Switch, HUB };
	public Type type = Type.Pc;

	public GameObject RenderLine(Transform t1, Transform t2) {
		GameObject go = new GameObject ("Line: " + t1.gameObject.name + " --> " + t2.gameObject.name);
		go.transform.parent = t1;
		LineRenderer line = go.AddComponent<LineRenderer> ();
		line.positionCount = 2;
		line.SetPosition (0, t1.position);
		line.SetPosition (1, t2.position);
		line.widthMultiplier = 0.1f;
		return go;
	}

	public delegate void NodeEvent(Node sender);
	public event NodeEvent OnClick;

	void OnMouseUp() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		if(OnClick != null)
			OnClick (this);
	}

	public Interface GetInterface(string interf) {
		foreach (Interface i in Interfaces) {
			if (interf == i.Name) {
				return i;
			}
		}
		return null;
	}

	public bool CanReach(IP destination) {
		//si esta conectado directamente
		//si se puede llegar por route
		return false;
	}
}