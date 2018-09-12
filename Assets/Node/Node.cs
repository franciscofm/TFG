using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	//FileSystem
	public Folder rootFolder;

	//Puertos LAN
	public Interface[] Interfaces;

	//Routing
	public List<RouteEntry> RouteTable;

	//ARP
	public List<ARPEntry> ARPTable;

	//ignored atm
	public enum Type { Pc, Switch, HUB };
	public Type type = Type.Pc;


	void Awake() {
		LoadFileSystem ();
	}
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

	public delegate void NodeEvent(Node sender);
	public delegate void NodeEventFull(Node sender, string value, bool correct);

	public static event NodeEvent OnClick;
	public static event NodeEvent OnClickUp;
	public static event NodeEvent OnClickDown;
	public static event NodeEventFull OnPing;

	void RaiseEvent(NodeEvent e) {
		if (e != null) e (this);
	}
	void RaiseEventFull(NodeEventFull e, string value, bool correct) {
		if (e != null) e (this, value, correct);
	}

	void OnMouseDown() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		if(OnClickDown != null)
			OnClickDown (this);
	}
	void OnMouseUp() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		if(OnClickUp != null)
			OnClickUp (this);
	}
	void OnMouse() {
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
	public bool HasInterface(Interface iface) {
		foreach (Interface i in Interfaces)
			if (i == iface)
				return true;
		return false;
	}

	public bool CanReach(IP destination) {
		//si es una direccion de las interficies propias
		foreach (Interface i in Interfaces)
			if (i.isUp && i.ip == destination) {
				RaiseEventFull (OnPing, destination.word, true);
				return true;
			}
		
		//si esta conectado directamente
		foreach (Interface i in Interfaces)
			if (i.isUp && i.connectedTo != null && i.connectedTo.isUp && i.connectedTo.ip == destination) {
				RaiseEventFull (OnPing, destination.word, true);
				return true;
			}

		//si se puede llegar por route
//		foreach (RouteEntry re in RouteTable) {
//			foreach (Interface i in Interfaces) {
//				if (i.isUp && re.gateway == i.ip) {
//					foreach (Connection c in Connections) {
//						if(Interfaces[c.ownIfaceId] == i.ip
//					}	
//				}
//			}
//		}

		RaiseEventFull (OnPing, destination.word, false);
		return false;
	}
}