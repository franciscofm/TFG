using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Interface : MonoBehaviour {

	public delegate void InterfaceEvent(Interface sender);
	public event InterfaceEvent OnSelect;
	public event InterfaceEvent OnUnselect;
	public event InterfaceEvent OnConnect;
	public event InterfaceEvent OnDisconnect;
	public event InterfaceEvent OnGetUp;
	public event InterfaceEvent OnGetDown;

	public Node node;

	public string Name = "eth0";
	public string address_family = "inet";
	public string dest_address = "inet";

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

	public bool IsUp() { return isUp; }
	public void SetStatus(bool up) {
		if(isUp != up)
			if (up)
				if (OnGetUp != null) OnGetUp (this);
			else
				if (OnGetDown != null) OnGetDown (this);
		isUp = up;
	}

	[Header("Debug")]
	public GameObject connectionRepresentation;
	public Interface connectedTo;
	public static Interface lastDown;
	public bool isUp = true;
	public bool selected;

	public void OnMouseUpCallback() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		
		if (selected) Unselect ();
		else CheckSelect ();
	}


	public void CheckSelect() {
		if (lastDown != null) { //Selecting second Iface

			if (lastDown.node == node) { //Selecting second Iface of same Node
				lastDown.Unselect ();
				Select ();
			} else { //Selecting second Iface of other Node

				if (connectedTo != null)  //Changing Iface this one is connected to
					Disconnect ();

				Connect ();
				lastDown.Unselect ();
			}
		} else { //Selecting first Iface
			Select ();
		}
	}

	public void Select() {
		selected = true;
		lastDown = this;

		if (OnSelect != null) OnSelect (this);
	}
	public void Unselect() {
		lastDown = null;
		selected = false;

		if (OnUnselect != null) OnUnselect (this);
	}

	public void Connect() {
		connectedTo = lastDown;
		connectionRepresentation = RenderLine (transform, connectedTo.transform);

		connectedTo.connectedTo = this;
		connectedTo.connectionRepresentation = connectionRepresentation;

		if (OnConnect != null) OnConnect (this);
		if (connectedTo.OnConnect != null) connectedTo.OnConnect (this);
	}
	public void Disconnect() {
		if (OnDisconnect != null) OnDisconnect (this);
		if (connectedTo.OnDisconnect != null) connectedTo.OnDisconnect (this);

		connectedTo.connectionRepresentation = null;
		connectedTo.connectedTo = null;
		connectedTo = null;
		Destroy (connectionRepresentation);
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
}