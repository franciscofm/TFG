using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mkdir {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		switch (command.Length) {
		case 1:
			CreateFolder (command [0], shell, value);
			break;
		default:
			break;
		}
	}

	static void Action(string path, Shell shell, CommandStructure value) {
		
	}

	static void CreateFolder(string path, Shell shell, CommandStructure value) {
		Folder folder = shell.folder.GetFolder (path);

	}
}
