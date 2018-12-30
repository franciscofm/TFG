﻿using System.Collections;
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
	public Interface connectedTo;
	public static Interface lastSelected;
	public bool isUp = true;
	public bool selected;

	public void OnMouseUpCallback() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		
		if (selected) Unselect ();
		else CheckSelect ();
	}

	public void CheckSelect() {
		if (lastSelected != null) { //Selecting second Iface

			if (lastSelected.node == node) { //Selecting second Iface of same Node
				lastSelected.Unselect ();
				Select ();

			} else { //Selecting second Iface of other Node
				print("Selecting second Iface of other Node");
				if (connectedTo != null) { //Changing Iface this one is connected to
					Disconnect ();
				}
				if (lastSelected.connectedTo != null) {
					if (lastSelected.connectedTo == this) {
						lastSelected.Disconnect ();
						Disconnect ();
						return;
					} else {
						lastSelected.Disconnect ();
					}
				}

				Connect ();
				lastSelected.Unselect ();
			}
		} else { //Selecting first Iface
			Select ();
		}
	}

	public void Select() {
		selected = true;
		lastSelected = this;

		if (OnSelect != null) OnSelect (this);
	}
	public void Unselect() {
		lastSelected = null;
		selected = false;

		if (OnUnselect != null) OnUnselect (this);
	}

	public void Connect() {
		connectedTo = lastSelected;

		connectedTo.connectedTo = this;

		if (OnConnect != null) OnConnect (connectedTo);
		if (connectedTo.OnConnect != null) connectedTo.OnConnect (this);
	}
	public void Disconnect() {
		if (OnDisconnect != null) OnDisconnect (connectedTo);
		//if (connectedTo.OnDisconnect != null) connectedTo.OnDisconnect (this);

		connectedTo.connectedTo = null;
		connectedTo = null;
	}

	public override string ToString () {
		string text = "Interface " + Name + Console.jump;
		text += "Is up? " + (isUp ? "yes" : "no");
		text += "IP:" + ip.word + ", "+ ip.numeric;
		return text;
	}
}