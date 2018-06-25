using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Console {

	public static string jump = Environment.NewLine;

	delegate void Template(Shell shell);

	private static readonly Dictionary<string, Template> commands = new Dictionary<string, Template>() {
		{"help", Log }
//		{"ls", 	delegate { Debug.Log("Hello"); } },
//		{"ifconfig", 	delegate { Debug.Log("Hello"); } },
//		{"cmd", 	delegate { Debug.Log("Hello"); } }
	};

	static void Log(Shell shell) {
		Debug.Log ("This sht worked: "+shell.gameObject.name);
	}

	public static bool ReadCommand(string[] command, Shell shell) {
		if (commands.ContainsKey (command [1]))
			commands [command [1]] (shell);
		else {
			shell.PrintOutput ("No match found"+jump);
		}
		return true;
	}
}
