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
	public static void Command(string[] command, Shell shell) {
		switch (command.Length) {
		case 0:
			List (false, shell); //List up
			break;
		case 1:
			switch (command [0]) {
			case "-a":
				List (true, shell); //List all
				break;
			default:
				List (command [0], shell); //List family
				break;
			}
			break;
		case 2:
			switch (command [1]) {
			case "down":
				IfDown (command, shell);
				break;
			case "up":
				IfUp (command, shell);
				break;
			default:
				Configure (command, shell);
				break;
			}
			break;
		default:
			Configure (command, shell);
			break;
		}
	}

	static void List(bool a, Shell shell) {
		foreach (Interface i in shell.node.Interfaces) 
			if(a || i.isUp)
				PrintInterface(i,shell);
	}
	static void List(string family, Shell shell) {
		foreach (Interface i in shell.node.Interfaces) 
			if(i.address_family == family)
				PrintInterface(i,shell);
	}
	static void PrintInterface(Interface i, Shell shell) {
		shell.PrintOutput (
			i.Name + ": " + i.address_family + " " + i.ip +
			" netmask " + i.netmask +
			" broadcast " + i.broadcast +
			Console.jump
		);
	}

	static void Configure(string[] command, Shell shell) {
		if (command.Length == 3 || command.Length == 5) {
			shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
			return;
		}
		if (command.Length > 3 && command[2] != "netmask") {
			shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
			return;
		}
		if (command.Length > 5 && command[2] != "broadcast") {
			shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
			return;
		}
		Interface _interface = shell.node.GetInterface(command[0]);
		if (_interface == null) { 
			shell.PrintOutput ("Error: the node does not have an interface called: " + command [0] + Console.jump);
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
						} catch (Exception e) {
							Debug.Log(e.Message);
							shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
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
				} catch (Exception e) {
					Debug.Log(e.Message);
					shell.PrintOutput ("Error: netmas & @mask must match x.x.x.x (with x value of 0 to 255) " + Console.jump);
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
			_interface.isUp = true;
		} catch (Exception e) {
			Debug.Log(e.Message);
			shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
			return;
		}


//		int[] intDir = command [1].IPToInt4 ();	// Split (new string[]{ "." }, 0x0).StringToInt4 ();
//		if (intDir == null || intDir.Length != 4) {
//			shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
//			return;
//		}
//		if (!intDir.ArrayAboveInt (255)) { //Si la direccion es valida
//			if (command.Length > 2) {
//				if (command [2] == "netmask") {
//					if (command.Length > 3) {
//						int[] intNet = command [3].IPToInt4 (); //.Split (new string[]{ "." }, 0x0).StringToInt4 ();
//						if (intNet == null || intDir.Length != 4) {
//							shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
//							return;
//						}
//						if (!intNet.ArrayAboveInt (255)) { //Si la netmask es valida
//							if (command.Length > 4) {
//								if (command [4] == "broadcast") {
//									if (command.Length > 5) {
//										int[] intBroad = command [5].IPToInt4 (); //Split (new string[]{ "." }, 0x0).StringToInt4 ();
//										if (intBroad == null || intDir.Length != 4) {
//											shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
//											return;
//										}
//										if (!intNet.ArrayAboveInt (255)) { //Si la broadcast es valida
//											_interface.SetBroadcast(intBroad);
//										} else {
//											shell.PrintOutput ("Error: mask with values above 255: " + command [1] + Console.jump);
//											return;
//										}
//									}
//								} else {
//									shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
//									return;
//								}
//							} else {
//								int[] bc = new int[4];
//								for (int i = 0; i < bc.Length; ++i) {
//									bc [i] = intDir[i] & intNet [i];
//									int not = 255 - intNet [i];
//									bc [i] |= not;
//								}
//								_interface.SetBroadcast (bc);
//							}
//							_interface.SetNetmask(intNet);
//						} else {
//							shell.PrintOutput ("Error: mask with values above 255: " + command [1] + Console.jump);
//							return;
//						}
//					} else {
//						shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
//						return;
//					}
//				} else {
//					shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
//					return;
//				}
//			} else {
//				_interface.SetNetmask (new int[]{ 255, 255, 255, 0 });
//				int[] bc = new int[4];
//				bc [3] = 255;
//				for (int i = 0; i < 3; ++i)
//					bc [i] = intDir [i];
//				_interface.SetBroadcast (bc);
//			}
//			_interface.SetIp(intDir);
//			_interface.isUp = true;
//		} else {
//			shell.PrintOutput ("Error: ip with values above 255: " + command [1] + Console.jump);
//			return;
//		}
	}

	public static void IfUp(string[] command, Shell shell) {
		if (command.Length < 1) {
			shell.PrintOutput ("Error: needed interface" + Console.jump);
		} else {
			Interface _interface = shell.node.GetInterface(command[0]);
			if (_interface == null)
				shell.PrintOutput ("Error: the node does not have an interface called: " + command [0] + Console.jump);
			else
				_interface.isUp = true;
		}
	}
	public static void IfDown(string[] command, Shell shell) {
		if (command.Length < 1) {
			shell.PrintOutput ("Error: needed interface" + Console.jump);
		} else {
			Interface _interface = shell.node.GetInterface(command[0]);
			if (_interface == null)
				shell.PrintOutput ("Error: the node does not have an interface called: " + command [0] + Console.jump);
			else
				_interface.isUp = false;
		}
	}

}
