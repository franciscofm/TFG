using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ping {

	public static void Command(string[] command, Shell shell) {
		switch (command.Length) {
		case 0:
			Help (shell);
			break;
		case 1:
			switch (command[0]) {
			case "-n":
				Action (command, shell); //List up
				break;
			}
			break;
		default:
			Help (shell);
			break;
		}
	}

	static void Help(Shell shell) {
		shell.PrintOutput ("Error: ping address" + Console.jump);
	}

	static int Action(string[] command, Shell shell) {
		try {
			IP address = new IP(command[0]);
//			bool reached = shell.node.CanReach(address);
//			if(reached)
//				shell.ReturnError ("Address "+command[0]+" reached.");
//			else
//				shell.ReturnError ("Address "+command[0]+" NOT reached.");
		} catch {
			return shell.ReturnError ("Error (1): Wrong IP address");
		}
		return 0;
	}
}
