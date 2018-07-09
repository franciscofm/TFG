using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Console {

	public static string jump = Environment.NewLine;

	delegate void Template(string[] command, Shell shell);

	private static readonly Dictionary<string, Template> commands = new Dictionary<string, Template>() {
		{"help", help },
		{"ifconfig", ifconfig },
		{"theme", theme }
//		{"cmd", 	delegate { Debug.Log("Hello"); } }
	};

	static void help(string[] command, Shell shell) {
		shell.PrintOutput ("List of avaliable commands:"+jump);
		foreach (string key in commands.Keys) 
			shell.PrintOutput (key + jump);
	}
	static void ifconfig(string[] command, Shell shell) {
		Ifconfig.Command (command, shell);
	}
	static void theme(string[] command, Shell shell) {
		Theme.Command (command, shell);
	}

	public static bool ReadCommand(string[] command, Shell shell) {
		if (commands.ContainsKey (command [1])) {
			commands [command [1]] (command.SubArray (2, command.Length - 2), shell);
		} else {
			shell.PrintOutput ("No match found"+jump);
		}
		return true;
	}

	public static T[] SubArray<T>(this T[] data, int index, int length) {
		T[] result = new T[length];
		Array.Copy (data, index, result, 0, length);
		return result;
	}
}
