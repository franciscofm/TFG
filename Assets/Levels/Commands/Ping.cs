using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ping {

	public static void Command(string[] command, Node node, CommandStructure value) {
		value.prompt = true;

		switch (command.Length) {
		case 0:
			Help (value);
			break;
		case 1:
			Action (command, node, value);
			break;
		default:
			Help (value);
			break;
		}
	}

	static void Help(CommandStructure value) {
		value.value = "Structure: ping x.x.x.x";
	}

	static void Action(string[] command, Node node, CommandStructure value) {
		try {
			IP address = new IP(command[0]);
			PingInfo pingInfo = node.CanReach(address);
			value.value = "Address " + command[0] + " " + (pingInfo.reached ? "reached" : "UNreachable") + ".";
			value.correct = true;
		} catch {
			value.value = "Error (1): Invalid IP address";
		}
	}
}
