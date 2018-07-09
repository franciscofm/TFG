using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Theme {

	public static aparenceToName[] aparences;

	public static void Command(string[] command, Shell shell) {
		if (command.Length == 0 || command [0] == "help") {
			Help (shell);
		} else {
			bool found = false;
			for (int i = 0; i < aparences.Length; ++i) {
				if (command [0] == aparences[i].Name) {
					Apply (shell, aparences [i].aparence);
					found = true;
					i = aparences.Length;
				}
			}
			if (!found)
				Help (shell);
		}
	}

	public static void Help(Shell shell) {
		shell.PrintOutput ("theme <aparence>" + Console.jump);
		shell.PrintOutput ("Avaliable aparences:" + Console.jump);
		foreach (aparenceToName a in aparences)
			shell.PrintOutput (a.Name + Console.jump);
	}

	public static void Apply(Shell shell, Aparence at) {
		shell.ChangeTheme (at);
	}
}
