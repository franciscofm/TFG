﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	//FileSystem
	public Folder rootFolder;

	//Puertos LAN
	public Interface[] Interfaces = new Interface[] {
		new Interface("192.168.60.1","255.255.255.255","192.168.60.1")
	};
	public Connection[] Connections;

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