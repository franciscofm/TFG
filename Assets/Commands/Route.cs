using System;
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
				List (false, shell); //List up
				break;
			}
			break;
		default:
			Check (command, shell);
			break;
		}
	}

	// route
	// route -n
	static void List(bool arp, Shell shell) {
		shell.PrintOutput ("Kernel IP routing table" + Console.jump);
		Node n = shell.node;
		foreach (RouteEntry re in n.RouteTable) {
			PrintEntry (re, shell, arp);
		}
	}
	static void PrintEntry(RouteEntry e, Shell shell, bool arp) {
		if(!arp)
			shell.PrintOutput (
				"Iface:" + e.iface + 
				" (D)" + e.destination + 
				" (GW)" + e.gateway + 
				" (M)" + e.genmask + 
				" (F)" + e.flags + 
				" (M)" + e.metric + 
				" (R)" + e.refe + 
				" (U)" + e.use + 
				Console.jump
			);
	}

	// add -net x.x.x.x
	// add -net x.x.x.x netmask 255.255.255.0
	// add -net x.x.x.x gw 192.168.60.2
	// add -net x.x.x.x dev eth0
	// add -net x.x.x.x gw 192.168.60.2 dev eth0
	static int Check(string[] command, Shell shell) {
		if (command.Length < 3 || command.Length > 9) return shell.ReturnError ("Error (0): route add|del -net|-host x.x.x.x [netmask x.x.x.x] [gw x.x.x.x] [dev iface]");
		if ((command.Length % 2) == 0) return shell.ReturnError ("Error (1): route add|del -net|-host x.x.x.x [netmask x.x.x.x] [gw x.x.x.x] [dev iface]");
		if ((command[0] != "add") && (command[0] != "del")) return shell.ReturnError ("Error (2): route add|del -net|-host x.x.x.x [netmask x.x.x.x] [gw x.x.x.x] [dev iface]");
		if ((command[1] != "-net") && (command[1] != "-host")) return shell.ReturnError ("Error (3): route add|del -net|-host x.x.x.x [netmask x.x.x.x] [gw x.x.x.x] [dev iface]");

		IP dest = null, netmask = null, gw = null;
		bool net = command [1] == "-net";
		string iface = "";

		try {
			dest = new IP(command[2]);
		} catch {
			return shell.ReturnError ("Error (4): Wrong IP");
		}

		for (int i = 3; i < command.Length; i += 2) {
			switch (command [i]) {
			case "netmask":
				if(netmask != null) return shell.ReturnError ("Error (5): Repeated option");
				try {
					netmask = new IP(command[i+1]);
				} catch {
					return shell.ReturnError ("Error (6): Wrong IP");
				}
				break;
			case "gw":
				if(gw != null) return shell.ReturnError ("Error 7): Repeated option");
				try {
					gw = new IP(command[i+1]);
				} catch {
					return shell.ReturnError ("Error (8): Wrong IP");
				}
				break;
			case "dev":
				if (iface != "") return shell.ReturnError ("Error (9): Repeated option");
				iface = command [i + 1];
				break;
			default:
				return shell.ReturnError ("Error (10): Unknown option");
			}
		}

		if (netmask == null)
			netmask = new IP (new uint[]{ 255, 255, 255, 0 });
		if (iface == "") {
			if (gw == null) iface = DeductIface (dest, shell.node);
			else iface = DeductIface (gw, shell.node);
		}
		if (gw == null)
			gw = IP.Empty;

		if (command [0] == "add")
			Add (dest, netmask, gw, net, iface, shell);
		else
			Remove (dest, netmask, gw, net, iface, shell);

		return 1;
	}
	static string DeductIface(IP ip, Node n) {
		foreach (Connection c in n.Connections)
			if (c.ifaceConnection.ip.numeric == ip.numeric)
				return c.iface.Name;
		return "";
	}

	static void Add(IP dest, IP netmask, IP gw, bool net, string iface, Shell shell) {
		RouteEntry entry = null;
		foreach (RouteEntry r in shell.node.RouteTable)
			if (r.destination.numeric == dest.numeric)
				entry = r;
		if (entry == null) {
			entry = new RouteEntry (dest, gw, netmask, iface, "U", 0, 0, 0);
			shell.node.RouteTable.Add (entry);
		} else {
			entry.genmask = netmask;
			entry.gateway = gw;
			entry.iface = iface;
			entry.flags = "U";
		}
		if(entry.destination.numeric == 0)
			entry.flags += "G";
	}
	static void Remove(IP dest, IP netmask, IP gw, bool net, string iface, Shell shell) {
		foreach (RouteEntry r in shell.node.RouteTable)
			if (r.destination == dest)
				shell.node.RouteTable.Remove (r);
	}
}
