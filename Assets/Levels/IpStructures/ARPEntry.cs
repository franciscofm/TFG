[System.Serializable]
public class ARPEntry {
	//	Address                  HWtype  HWaddress           Flags Mask            Iface                                                                                                               
	//	gateway                  ether   02:42:0e:ff:79:bb   C                     eth0    
	//	gateway (172.17.0.1) at 02:42:0e:ff:79:bb [ether] on eth0   

	public string addressName;
	public IP addressIP;

	public string hwType;
	public ulong mac;

	public string flags;
	public string mask;
	public string iface;


	public ARPEntry() {

	}

}