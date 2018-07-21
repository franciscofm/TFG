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
}

[System.Serializable]
public class Interface {
	public GameObject representation;
	public Node node;

	public string Name = "eth0";
	public string address_family = "inet";
	public string dest_address = "inet";

	public bool isUp = true;

	public IP ip;
	public IP netmask;
	public IP broadcast;

	public Interface(string ip, string mask, string broadcast, 
			string name = "eth0", bool isUp = true, 
			GameObject representation = null, Node node = null) {
		this.ip = new IP (ip);
		this.netmask = new IP (mask);
		this.broadcast = new IP (broadcast);
		this.Name = name;
		this.isUp = isUp;
		this.representation = representation;
		this.node = node;
	}
	public Interface(uint[] ip, uint[] mask, uint[] broadcast) {
		this.ip = new IP (ip);
		this.netmask = new IP (mask);
		this.broadcast = new IP (broadcast);
	}
	public Interface(
		uint i_a, uint i_b, uint i_c, uint i_d,
		uint m_a, uint m_b, uint m_c, uint m_d,
		uint b_a, uint b_b, uint b_c, uint b_d) : 
		this(
			new uint[]{ i_a, i_b, i_c, i_d }, 
			new uint[]{ m_a, m_b, m_c, m_d }, 
			new uint[]{ b_a, b_b, b_c, b_d })
			{
		//Returns Interface(int[],int],int[])
	}

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

[System.Serializable]
public class RouteEntry {   
	public IP genmask;
	public IP destination;
	public IP gateway;

	public string iface;
	public string flags; //?
	public int metric;
	public int refe;
	public int use;

	public RouteEntry(IP destination, IP gateway, IP genmask) {
		this.genmask = genmask;
		this.destination = destination;
		this.gateway = gateway;
	}
	public RouteEntry(IP destination, IP gateway, IP genmask, string iface, string flags, int metric, int refe, int use)
		: this(destination, gateway, genmask) {
		this.iface = iface;
		this.flags = flags;
		this.metric = metric;
		this.refe = refe;
		this.use = use;
	}
}

[System.Serializable]
public class ARPEntry {
	//	Address                  HWtype  HWaddress           Flags Mask            Iface                                                                                                               
	//	gateway                  ether   02:42:0e:ff:79:bb   C                     eth0    
	//	gateway (172.17.0.1) at 02:42:0e:ff:79:bb [ether] on eth0   

	public string addressName;
	public IP addressIP;

	public string hwType;
	public IP hwAddress;

	public string flags;
	public string mask;
	public string iface;


	public ARPEntry() {
		
	}
	
}

[System.Serializable]
public class Connection {

	public Interface iface;
	public Interface ifaceConnection;

	public Connection(Interface iface, Interface ifaceConnection) {
		this.iface = iface;
		this.ifaceConnection = ifaceConnection;
	}
}