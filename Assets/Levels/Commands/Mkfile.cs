using System.Collections;
using System.Collections.Generic;

public static class Mkfile {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		switch (command.Length) {
		default:
			value.prompt = true;
			value.value = "Not implemented yet.";
			value.correct = true;
			break;
		}
	}
}
