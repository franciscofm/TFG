using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public new string name = "PC1";

	//[Header("File system")]
	[HideInInspector] public Folder rootFolder;

	[Header("Interfaces")]
	public Interface[] Interfaces;
	[Space] //SetUp
	public GameObject interfacePrefab;
	public int interfaceQuantity = 3;
	public float interfaceOffsetToNode = 1.5f;

	[Header("Route Table")]
	public List<RouteEntry> RouteTable;

	[Header("ARP")]
	public List<ARPEntry> ARPTable;

	public void Load() {
		LoadFileSystem ();
		LoadInterfaces ();
		LoadRouteTable ();
		LoadARPTable ();
	}
	/// <summary>
	/// Loads the file system.
	/// </summary>
	void LoadFileSystem() {
		Folder temporary;
		rootFolder = new Folder ("", null);
		rootFolder.folders.Add (temporary = new Folder ("bin", rootFolder));
			temporary.files.Add (new File ("ARP"));
	//		temporary.files.Add (new File ("ECHO"));
	//		temporary.files.Add (new File ("IFCONFIG"));
	//		temporary.files.Add (new File ("LS"));
	//		temporary.files.Add (new File ("MKDIR"));
	//		temporary.files.Add (new File ("MKFILE"));
	//		temporary.files.Add (new File ("PING"));
	//		temporary.files.Add (new File ("ROUTE"));

		rootFolder.folders.Add (temporary = new Folder ("boot", rootFolder));
			temporary.files.Add (new File ("KERNEL"));
			temporary.files.Add (new File ("SYSTEM.MAP"));
			temporary.files.Add (new File ("BOOT"));

		rootFolder.folders.Add (temporary = new Folder ("etc", rootFolder));
	//		temporary.files.Add (new File ("FSTAB"));
			temporary.files.Add (new File ("GATEWAYS"));
			temporary.files.Add (new File ("GROUP"));
			temporary.files.Add (new File ("HOSTS"));
			temporary.files.Add (new File ("HOSTS.ALLOW"));
			temporary.files.Add (new File ("HOSTS.DENY"));
			temporary.files.Add (new File ("HOSTS.EQUIV"));
			temporary.files.Add (new File ("HOSTS.LPD"));
			temporary.files.Add (new File ("NETWORKS"));
			temporary.files.Add (new File ("PROTOCOLS"));
	//		temporary.folders.Add (new Folder ("OPT", temporary));

		rootFolder.folders.Add (temporary = new Folder ("usr", rootFolder));
			temporary.folders.Add (new Folder ("LOCAL", temporary));
			temporary.folders.Add (new Folder ("SHARE", temporary));
			temporary.folders.Add (new Folder ("BIN", temporary));
			temporary.folders.Add (new Folder ("INCLUDE", temporary));
			temporary.folders.Add (new Folder ("LIB", temporary));
			temporary.folders.Add (new Folder ("SBIN", temporary));

//		rootFolder.folders.Add (new Folder ("sbin", rootFolder));
//		rootFolder.folders.Add (new Folder ("lost+found", rootFolder));
//		rootFolder.folders.Add (new Folder ("media", rootFolder));
//		rootFolder.folders.Add (new Folder ("proc", rootFolder));
//		rootFolder.folders.Add (new Folder ("sys", rootFolder));
//		rootFolder.folders.Add (new Folder ("var", rootFolder));

		rootFolder.folders.Add (new Folder ("lib", rootFolder));
		rootFolder.folders.Add (new Folder ("mnt", rootFolder));
		rootFolder.folders.Add (new Folder ("opt", rootFolder));
		rootFolder.folders.Add (new Folder ("home", rootFolder));
		rootFolder.folders.Add (new Folder ("root", rootFolder));
		rootFolder.folders.Add (new Folder ("tmp", rootFolder));
		rootFolder.folders.Add (new Folder ("dev", rootFolder));
	}
	/// <summary>
	/// Loads the interfaces.
	/// </summary>
	void LoadInterfaces() {
		Interfaces = new Interface[interfaceQuantity];
		float rotationOffset = 360f / interfaceQuantity;
		Vector3 ifaceLocalPos = new Vector3 (0f, 0f, interfaceOffsetToNode);

		for (int i = 0; i < interfaceQuantity; ++i) {
			//Center
			Transform t = new GameObject ().transform;
			t.gameObject.name = gameObject.name + " iface " + i + " pivot";
			t.parent = this.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.AngleAxis (rotationOffset * i, Vector3.up);

			Interface iface = Instantiate (interfacePrefab, t).GetComponent<Interface> ();
			iface.gameObject.name = gameObject.name + " iface: " + i;
			iface.node = this;
			iface.Name = "eth" + i;
			iface.SetStatus (i == 0);
			iface.transform.localPosition = ifaceLocalPos;
			iface.SetIp (new IP ("192.168.0." + (i + 1)));
			iface.SetNetmask (new IP ("255.255.255.0"));
			iface.SetBroadcast (new IP ("192.168.0.255"));
			Interfaces [i] = iface;

			if(OnIfaceCreated != null) OnIfaceCreated (iface);
		}
	}
	/// <summary>
	/// Loads the route table.
	/// </summary>
	void LoadRouteTable() {
		//	Kernel IP routing table
		//	Destination     Gateway         Genmask         Flags Metric Ref    Use Iface  
		//	0.0.0.0         192.168.0.0     0.0.0.0         UG    0      0        0 eth0      
		//	192.168.0.0     0.0.0.0         255.255.0.0     U     0      0        0 eth0
		RouteTable = new List<RouteEntry>();
		RouteTable.Add (new RouteEntry (new IP ("0.0.0.0"), new IP ("192.168.0.0"), new IP ("0.0.0.0")));
		RouteTable.Add (new RouteEntry (new IP ("192.168.0.0"), new IP ("0.0.0.0"), new IP ("255.255.255.0")));
	}
	/// <summary>
	/// Loads the ARP table.
	/// </summary>
	void LoadARPTable() {
		ARPTable = new List<ARPEntry> ();
	}

	public delegate void NodeEvent(Node sender);
	public delegate void NodeIfaceEvent(Interface sender);
	public delegate void NodeEventFull(Node sender, object obj);

	public event NodeEvent OnClickUp;
	public event NodeEvent OnClickDown;
	public event NodeIfaceEvent OnIfaceCreated;

	public static event NodeEvent OnClickUpStatic;
	public static event NodeEventFull OnPing;
	public static event NodeEventFull OnShellCommand;

	/// <summary>
	/// Raises the event.
	/// </summary>
	/// <param name="e">Event to be raised.</param>
	void RaiseEvent(NodeEvent e) {
		if (e != null) e (this);
	}
	/// <summary>
	/// Raises the event.
	/// </summary>
	/// <param name="e">Event to be raised.</param>
	/// <param name="obj">Object to be passed through the event.</param>
	void RaiseEventFull(NodeEventFull e, object obj) {
		if (e != null) e (this, obj);
	}

	/// <summary>
	/// Raises the mouse down callback event.
	/// </summary>
	public void OnMouseDownCallback() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		if(OnClickDown != null) OnClickDown (this);
	}
	/// <summary>
	/// Raises the mouse up callback event.
	/// </summary>
	public void OnMouseUpCallback() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		if(OnClickUpStatic != null) OnClickUpStatic (this);
		if(OnClickUp != null) OnClickUp (this);
	}

	/// <summary>
	/// Raises OnShellCommand with the specified object as parameter.
	/// </summary>
	public void RaiseOnShellCommand(object obj) {
		RaiseEventFull (OnShellCommand, obj);
	}

	/// <summary>
	/// Gets the interface with specified name.
	/// </summary>
	/// <returns>The interface if it exists; otherwise, null.</returns>
	/// <param name="interf">Interf.</param>
	public Interface GetInterface(string interf) {
		foreach (Interface i in Interfaces) {
			if (interf == i.Name) {
				return i;
			}
		}
		return null;
	}
	/// <summary>
	/// Determines whether the Node has the specified Interface.
	/// </summary>
	/// <returns><c>true</c> if this instance has the specified interface; otherwise, <c>false</c>.</returns>
	public bool HasInterface(Interface iface) {
		foreach (Interface i in Interfaces)
			if (i == iface)
				return true;
		return false;
	}

	/// <summary>
	/// Determines whether the Node can reach the specified destination Node.
	/// Raises event OnPing passing <c>PingInfo</c> as parameter.
	/// </summary>
	/// <returns> Returns <c>PingInfo</c> structure with results.</returns>
	/// <param name="destination">Destination.</param>
	public PingInfo CanReach(IP destination) {
		PingInfo pingInfo = new PingInfo ();
		pingInfo.origin = this;

		//si es una direccion de las interficies propias
		foreach (Interface i in Interfaces) {
			if (i.IsUp () && i.ip.Equals(destination)) {
				pingInfo.destiny = this;
				pingInfo.reached = true;
				RaiseEventFull (OnPing, pingInfo);
				return pingInfo;
			}
		}
		
		//si esta conectado directamente TODO mirar firewall (iptable)
		foreach (Interface i in Interfaces)
			if (i.IsUp() && i.connectedTo != null && i.connectedTo.IsUp() && i.connectedTo.ip.Equals(destination)) {
				pingInfo.destiny = i.connectedTo.node;
				pingInfo.reached = true;
				RaiseEventFull (OnPing, pingInfo);
				return pingInfo;
			}

		//si se puede llegar por route
		foreach(RouteEntry re in RouteTable) {
			if(destination.IsSubnet(re.destination, re.genmask)) {
				Interface iface = GetInterface (re.iface);
				if (iface != null) {
					PingInfo routePingInfo = iface.connectedTo.node.CanReach (destination);
					if (routePingInfo.reached) {
						pingInfo.destiny = routePingInfo.destiny;
						RaiseEventFull (OnPing, pingInfo);
						return pingInfo;
					}
				}
			}
		}

		pingInfo.reached = false;
		pingInfo.destiny = null;
		RaiseEventFull (OnPing, pingInfo);
		return pingInfo;
	}

}

/// <summary>
/// Ping info structure.
/// </summary>
public class PingInfo {
	public bool reached;
	public Node destiny;
	public Node origin;
}