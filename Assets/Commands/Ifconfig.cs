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
	public static void Command(string[] command, Shell shell) {
		switch (command.Length) {
		case 0:
			List (false); //List up
			break;
		case 1:
			switch (command [0]) {
			case "-a":
				List (true); //List all
				break;
			default:
				List (command [0]); //List family
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

	static void List(bool a) {
		Debug.Log ("List (" + a + ")");
	}
	static void List(string family) {
		Debug.Log ("List (" + family + ")");
	}

	static void Configure(string[] command, Shell shell) {
		if (command.Length < 2) {
			shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
			return;
		}

		Interface _interface = shell.node.HasInterface(command[0]);
		if (_interface == null) { 
			shell.PrintOutput ("Error: the node does not have an interface called: " + command [0] + Console.jump);
			return;
		}

		else {
			int[] intDir = command [1].Split (new string[]{ "." }, 0x0).StringToInt4 ();
			if (intDir == null || intDir.Length != 4) {
				shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
				return;
			}
			if (!intDir.ArrayAboveInt (255)) { //Si la direccion es valida
				if (command.Length > 2) {
					if (command [2] == "netmask") {
						if (command.Length > 3) {
							int[] intNet = command [3].Split (new string[]{ "." }, 0x0).StringToInt4 ();
							if (intNet == null || intDir.Length != 4) {
								shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
								return;
							}
							if (!intNet.ArrayAboveInt (255)) { //Si la netmask es valida
								if (command.Length > 4) {
									if (command [4] == "broadcast") {
										if (command.Length > 5) {
											int[] intBroad = command [5].Split (new string[]{ "." }, 0x0).StringToInt4 ();
											if (intBroad == null || intDir.Length != 4) {
												shell.PrintOutput ("Error: direction & @mask must match x.x.x.x (with x value of 0 to 255)" + Console.jump);
												return;
											}
											if (!intNet.ArrayAboveInt (255)) { //Si la broadcast es valida
												_interface.SetBroadcast(intBroad);
											} else {
												shell.PrintOutput ("Error: mask with values above 255: " + command [1] + Console.jump);
												return;
											}
										}
									} else {
										shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
										return;
									}
								} else {
									//TODO asumir cosas
								}
								_interface.SetNetmask(intNet);
							} else {
								shell.PrintOutput ("Error: mask with values above 255: " + command [1] + Console.jump);
								return;
							}
						} else {
							shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
							return;
						}
					} else {
						shell.PrintOutput ("Error: ifconfig interface direction [netmask @mask [broadcast @mask]]" + Console.jump);
						return;
					}
				} else {
					//TODO asumir cosas
				}
				_interface.SetIp(intDir);
				_interface.isUp = true;
			} else {
				shell.PrintOutput ("Error: ip with values above 255: " + command [1] + Console.jump);
				return;
			}
		}
	}

	public static void IfUp(string[] command, Shell shell) {
		if (command.Length < 1) {
			shell.PrintOutput ("Error: needed interface" + Console.jump);
		} else {
			Interface _interface = shell.node.HasInterface(command[0]);
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
			Interface _interface = shell.node.HasInterface(command[0]);
			if (_interface == null)
				shell.PrintOutput ("Error: the node does not have an interface called: " + command [0] + Console.jump);
			else
				_interface.isUp = false;
		}
	}

}
