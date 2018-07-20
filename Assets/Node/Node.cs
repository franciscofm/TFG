using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public Interface[] Interfaces = new Interface[3];
	public Connection[] LANs = new Connection[3];

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
}

[System.Serializable]
public class Interface {
	public GameObject representation;

	public string Name = "eth0";
	public string address = "";
	public string address_family = "inet";
	public string dest_address = "inet";

	public bool isUp = true;
	public int[] ip = new int[]{ 192, 186, 60, 1 };
	public int[] netmask = new int[]{ 255, 255, 255, 0 };
	public int[] broadcast = new int[]{ 192, 168, 0, 255 };

	public Interface(int[] ip, int[] mask, int[] broadcast) {
		if(ip.ArrayAboveInt(255)) Debug.Log("Error ip: "+ip);
		if(mask.ArrayAboveInt(255)) Debug.Log("Error ip: "+mask);
		if(broadcast.ArrayAboveInt(255)) Debug.Log("Error ip: "+broadcast);

		//TODO comprobacion errores

		this.ip = ip;
		this.netmask = mask;
		this.broadcast = broadcast;
	}
	public Interface(
		int i_a, int i_b, int i_c, int i_d,
		int m_a, int m_b, int m_c, int m_d,
		int b_a, int b_b, int b_c, int b_d) : 
		this(
			new int[]{ i_a, i_b, i_c, i_d }, 
			new int[]{ m_a, m_b, m_c, m_d }, 
			new int[]{ b_a, b_b, b_c, b_d })
			{
		//Returns Interface(int[],int],int[])
	}

	public string GetClass() {
		if (ip [0] < 128) return "A";
		if (ip [0] < 192) return "B";
		if (ip [0] < 224) return "C";
		if (ip [0] < 240) return "D";
		return "E";
	}
	public void SetIp(int[] ip) {
		this.ip = ip;
	}
	public void SetNetmask(int[] netmask) {
		this.netmask = netmask;
	}
	public void SetBroadcast(int[] broadcast) {
		this.broadcast = broadcast;
	}
}

[System.Serializable]
public class RouteEntry {   
	public int[] genmask;
	public int[] destination;
	public int[] gateway;

	public string iface;
	public string flags; //?
	public int metric;
	public int refe;
	public int use;

	public RouteEntry(int[] destination, int[] gateway, int[] genmask) {
		this.genmask = genmask;
		this.destination = destination;
		this.gateway = gateway;
	}
	public RouteEntry(int[] destination, int[] gateway, int[] genmask, string iface, string flags, int metric, int refe, int use)
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
	public int[] addressIP;

	public string hwType;
	public int[] hwAddress;

	public string flags;
	public string mask;
	public string iface;


	public ARPEntry() {
		
	}
	
}

[System.Serializable]
public class Connection {

	public Transform iface1, iface2;
	public Node node1, node2;

	public Connection(Transform i1, Transform i2, Node n1, Node n2) {
		iface1 = i1;
		iface2 = i2;
		node1 = n1;
		node2 = n1;
	}
}