using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class History {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		value.prompt = true;

		switch (command.Length) {
			case 0:
				value.correct = true;
				PrintHistory (shell, value);
				break;
			default:
				value.correct = false;
				value.value = "Just type history dude";
				break;
		}
	}

	static void PrintHistory(Shell shell, CommandStructure value) {
		value.correct = true;
		value.value = "";
		foreach (string s in shell.history)
			value.value += (s + Console.jump);
	}

}
