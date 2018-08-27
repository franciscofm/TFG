using UnityEngine;

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

	//TODO falta el mac FF:FF:FF:FF:FF:FF 8bit
	public ulong mac = 0xffffffffffff;

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