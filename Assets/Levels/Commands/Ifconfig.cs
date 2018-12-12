using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ifconfig {
	
//     ifconfig [-L] [-m] [-r] interface [create] [address_family] [address [dest_address]] [parameters]
//     ifconfig interface destroy
//     ifconfig -a [-L] [-d] [-m] [-r] [-u] [-v] [address_family]
//     ifconfig -l [-d] [-u] [address_family]
//     ifconfig [-L] [-d] [-m] [-r] [-u] [-v] [-C]
//     ifconfig interface vlan vlan-tag vlandev iface
//     ifconfig interface -vlandev iface
//     ifconfig interface bonddev iface
//     ifconfig interface -bonddev iface
//     ifconfig interface bondmode lacp | static
	 
	// ifconfig [-a]
	// ifconfig interface up/down
	// ifconfig interface @address netmask @mask broadcast @mask
	// ifconfig eth0 192.168.60.1
	// ifconfig eth0 192.168.60.1 netmask 255.255.255.0
	// ifconfig eth0 192.168.60.1 netmask 255.255.255.0 broadcast 192.169.60.255
	public static void Command(string[] command, Node node, CommandStructure value) {
		switch (command.Length) {
		case 0:
			List (false, node, value); //List up
			break;
		case 1:
			switch (command [0]) {
			case "-a":
				List (true, node, value); //List all
				break;
			default:
				List (command [0], node, value); //List family
				break;
			}
			break;
		case 2:
			switch (command [1]) {
			case "down":
				IfDown (command, node, value);
				break;
			case "up":
				IfUp (command, node, value);
				break;
			default:
				Configure (command, node, value);
				break;
			}
			break;
		default:
			Configure (command, node, value);
			break;
		}
	}

	static void List(bool a, Node node, CommandStructure value) {
		value.prompt = true;
		value.correct = true;
		foreach (Interface i in node.Interfaces)
			if (a || i.IsUp())
				PrintInterface (i, value);
	}
	static void List(string family, Node node, CommandStructure value) {
		value.prompt = true;
		value.correct = true;
		foreach (Interface i in node.Interfaces)
			if (i.address_family == family)
				PrintInterface (i, value);
	}
	static void PrintInterface(Interface i, CommandStructure value) {
		value.value +=
			i.Name + ": " + i.address_family + " " + i.ip +
			" netmask " + i.netmask +
			" broadcast " + i.broadcast +
			Console.jump
		;
	}

	static void Configure(string[] command, Node node, CommandStructure value) {
		value.prompt = true;

		if (command.Length == 3 || command.Length == 5) {
			value.value = "Error #101: ifconfig interface @direction [netmask @mask [broadcast @mask]]" + Console.jump;
			return;
		}
		if (command.Length > 3 && command[2] != "netmask") {
			value.value = "Error #102: ifconfig interface @direction [netmask @mask [broadcast @mask]]" + Console.jump;
			return;
		}
		if (command.Length > 5 && command[4] != "broadcast") {
			value.value = "Error #103: ifconfig interface @direction [netmask @mask [broadcast @mask]]" + Console.jump;
			return;
		}
		Interface _interface = node.GetInterface(command[0]);
		if (_interface == null) { 
			value.value = "Error #104: the node does not have an interface called: " + command [0] + Console.jump;
			return;
		}

		try {
			IP direction = new IP(command [1]);
			if (command.Length > 2) {
				try {
					IP netmask = new IP(command[3]);
					if (command.Length > 4) {
						try {
							IP broadcast = new IP(command [5]);
							_interface.SetBroadcast(broadcast);
						} catch {
							//Debug.Log(e.Message);
							value.value = "Error #105: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump;
							return;
						}
					} else {
						uint[] broadcast = new uint[4];
						for (int i = 0; i < broadcast.Length; ++i) {
							broadcast [i] = direction.array[i] & netmask.array [i];
							uint not = 255 - netmask.array [i];
							broadcast [i] |= not;
						}
						_interface.SetBroadcast (new IP(broadcast));
					}
					_interface.SetNetmask(netmask);
				} catch {
					//Debug.Log(e.Message);
					value.value = "Error #106: netmas & @mask must match x.x.x.x (with x value of 0 to 255) " + Console.jump;
					return;
				}
			} else {
				_interface.SetNetmask (new IP(new uint[]{ 255, 255, 255, 0 }));
				uint[] broadcast = new uint[4];
				broadcast [3] = 255;
				for (int i = 0; i < 3; ++i)
					broadcast [i] = direction.array [i];
				_interface.SetBroadcast (new IP(broadcast));
			}
			_interface.SetIp(direction);
			_interface.SetStatus(true);
		} catch {
			//Debug.Log(e.Message);
			value.value = "Error #107: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump;
			return;
		}

		value.prompt = false;
		value.correct = true;
	}

	public static void IfUp(string[] command, Node node, CommandStructure value) {
		if (command.Length < 1) {
			value.prompt = true;
			value.value = "Error #108: needed interface" + Console.jump;
		} else {
			Interface _interface = node.GetInterface(command[0]);
			if (_interface == null) {
				value.prompt = true;
				value.value = "Error #109: the node does not have an interface called: " + command [0] + Console.jump;
			} else {
				_interface.SetStatus(true);
				value.correct = true;
			}
		}
	}
	public static void IfDown(string[] command, Node node, CommandStructure value) {
		if (command.Length < 1) {
			value.prompt = true;
			value.value = "Error #110: needed interface" + Console.jump;
		} else {
			Interface _interface = node.GetInterface(command[0]);
			if (_interface == null) {
				value.prompt = true;
				value.value = "Error #111: the node does not have an interface called: " + command [0] + Console.jump;
			} else {
				_interface.SetStatus(false);
				value.correct = true;
			}
		}
	}

}
