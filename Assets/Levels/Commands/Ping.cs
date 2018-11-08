using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ping {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		value.prompt = true;

		switch (command.Length) {
		case 0:
			Help (shell, value);
			break;
		case 1:
			Action (command, shell, value);
			break;
		default:
			Help (shell, value);
			break;
		}
	}

	static void Help(Shell shell, CommandStructure value) {
		value.value = "Structure: ping x.x.x.x";
	}

	static void Action(string[] command, Shell shell, CommandStructure value) {
		try {
			IP address = new IP(command[0]);
			bool reached = shell.node.CanReach(address);
			value.value = "Address " + command[0] + " " + (reached ? "reached" : "UNreachable") + ".";
			value.correct = true;
		} catch {
			value.value = "Error (1): Invalid IP address";
		}
	}
}
