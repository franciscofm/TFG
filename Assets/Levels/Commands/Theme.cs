using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Theme {

	public static aparenceToName[] aparences;

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		if (command.Length == 0 || command [0] == "help") {
			Help (shell, value);
		} else {
			bool found = false;
			for (int i = 0; i < aparences.Length; ++i) {
				if (command [0] == aparences[i].Name) {
					Apply (shell, aparences [i].aparence);
					found = true;
					i = aparences.Length;
					value.prompt = false;
					value.correct = true;
				}
			}
			if (!found)
				Help (shell, value);
		}
	}

	public static void Help(Shell shell, CommandStructure value) {
		value.value = "theme <aparence>" + Console.jump;
		value.value += "Avaliable aparences:" + Console.jump;
		foreach (aparenceToName a in aparences)
			value.value += a.Name + Console.jump;

		value.prompt = true;
	}

	public static void Apply(Shell shell, Aparence at) {
		shell.ChangeTheme (at);
	}
}
