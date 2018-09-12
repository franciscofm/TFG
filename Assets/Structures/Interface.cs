using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Interface : MonoBehaviour {

	public GameObject connectionRepresentation;
	public Interface connectedTo;
	public MeshRenderer mesh;

	void OnMouseUp() {
		print ("Up "+transform.parent.name+"/"+gameObject.name);
		if(EventSystem.current.IsPointerOverGameObject()) return;

		if (selected) Unselect ();
		else Select ();
	}

	public static Interface lastDown;
	public bool selected;

	public void Select() {
		
		if (lastDown != null) {

			if (lastDown.node == node) { //Not 2 interfaces of the same node
				lastDown.selected = false;
				selected = true;

				lastDown.mesh.material.color = Color.white;
				mesh.material.color = Color.red;

				lastDown = this;
			} else {

				if (connectedTo != null) {
					connectedTo.connectionRepresentation = null;
					connectedTo.connectedTo = null;
					Destroy (connectionRepresentation);
				}

				connectedTo = lastDown;
				connectionRepresentation = RenderLine (transform, connectedTo.transform);

				connectedTo.connectedTo = this;
				connectedTo.connectionRepresentation = connectionRepresentation;

				lastDown.Unselect ();
				lastDown = null;
			}
		} else {
			selected = true;
			lastDown = this;
			mesh.material.color = Color.red;
		}
	}
	public void Unselect() {
		lastDown = null;
		selected = false;
		mesh.material.color = Color.white;
	}

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

	public Node node;

	public string Name = "eth0";
	public string address_family = "inet";
	public string dest_address = "inet";

	public bool isUp = true;

	public IP ip;
	public IP netmask;
	public IP broadcast;

	//mac FF:FF:FF:FF:FF:FF 8bit
	public ulong mac = 0xffffffffffff;

	public void SetIp(IP ip) {
		this.ip = ip;
	}
	public void SetNetmask(IP netmask) {
		this.netmask = netmask;
	}
	public void SetBroadcast(IP broadcast) {
		this.broadcast = broadcast;
	}
}