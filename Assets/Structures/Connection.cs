using UnityEngine;

[System.Serializable]
public class Connection {

	public Interface iface;
	public Interface ifaceConnection;
	public GameObject line;

	public Connection(Interface iface, Interface ifaceConnection, GameObject line) {
		this.iface = iface;
		this.ifaceConnection = ifaceConnection;
		this.line = line;
	}
}