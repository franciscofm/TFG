using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Route {

	//	route
	//	Kernel IP routing table                                                                                                                                                                        
	//	Destination     Gateway         Genmask         Flags Metric Ref    Use Iface                                                                                                                  
	//	default         gateway         0.0.0.0         UG    0      0        0 eth0                                                                                                                   
	//	172.17.0.0      0.0.0.0         255.255.0.0     U     0      0        0 eth0  

	//	route -n
	//	Kernel IP routing table                                                                                                                                                                        
	//	Destination     Gateway         Genmask         Flags Metric Ref    Use Iface                                                                                                                  
	//	0.0.0.0         172.17.0.1      0.0.0.0         UG    0      0        0 eth0                                                                                                                   
	//	172.17.0.0      0.0.0.0         255.255.0.0     U     0      0        0 eth0 

	//	Address                  HWtype  HWaddress           Flags Mask            Iface                                                                                                               
	//	gateway                  ether   02:42:0e:ff:79:bb   C                     eth0 
	//	gateway (172.17.0.1) at 02:42:0e:ff:79:bb [ether] on eth0   

	//	eth0: flags=4163<UP,BROADCAST,RUNNING,MULTICAST>  mtu 1500                                                                                                                                     
	//	inet 172.17.0.130  netmask 255.255.0.0  broadcast 0.0.0.0                                                                                                                              
	//	ether 02:42:ac:11:00:82  txqueuelen 0  (Ethernet)

	public static void Command(string[] command, Shell shell) {
		switch (command.Length) {
		case 0:
			List (true, shell); //List up
			break;
		case 1:
			switch (command[0]) {
			case "-n":
				List (true, shell); //List up
				break;
			}
			break;
		case 2:
			break;
		default:
			break;
		}
	}

	static void List(bool arp, Shell shell) {
		shell.PrintOutput ("Kernel IP routing table" + Console.jump);
		Node n = shell.node;
		foreach (RouteEntry re in n.RouteTable) {
			PrintEntry (re, shell, arp);
		}
	}
	static void List(string[] command, Shell shell) {
		
	}
	static void PrintEntry(RouteEntry e, Shell shell, bool arp) {
		if(!arp)
			shell.PrintOutput (
				"D:" + e.destination + 
				", GW:" + e.gateway + 
				", Gmask:" + e.genmask + 
				", F" + e.flags + 
				", M" + e.metric + 
				", R" + e.refe + 
				", U" + e.use + 
				", Iface" + e.iface + 
				Console.jump
			);
	}

	static void Add(string[] command, Shell shell) {

	}
	static void Remove(string[] command, Shell shell) {

	}
}
