using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Theme {

	public static aparenceToName[] aparences;

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		switch (command.Length) {
		case 1:
			if (command [0] == "help") {
				Help (shell, value);
				value.correct = true;
			} else {
				for (int i = 0; i < aparences.Length; ++i) {
					if (command [0] == aparences [i].Name) {
						Apply (shell, aparences [i].aparence);
						value.correct = true;
						return;
					}
				}
				Help (shell, value);
			}
			break;
		case 4:
			float r, g, b;
			Color c;
			try {
				r = float.Parse(command[1]);
				g = float.Parse(command[2]);
				b = float.Parse(command[3]);
				c = new Color(r/255f, g/255f, b/255f);
			} catch {
				Help (shell, value);
				return;
			}
			for (int i = 0; i < aparences.Length; ++i) {
				if (command [0] == aparences[i].Name) {
					Aparence a = new Aparence (aparences [i].aparence, c, c, Color.black); //TODO add back to parameter
					Apply (shell, a);
					value.correct = true;
					return;
				}
			}
			break;
		default:
			Help (shell, value);
			break;
		}

	}

	public static void Help(Shell shell, CommandStructure value) {
		value.prompt = true;
		value.value = "theme <aparence> [r g b]" + Console.jump;
		value.value += "Avaliable aparences:" + Console.jump;
		foreach (aparenceToName a in aparences)
			value.value += a.Name + Console.jump;
	}

	public static void Apply(Shell shell, Aparence at) {
		shell.ChangeTheme (at);
	}
}
