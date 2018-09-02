using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public static class Cd {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		switch (command.Length) {
		case 1:
			Move (command [0], shell, value);
			break;
		default:
			break;
		}
	}

	public static void Move(string path, Shell shell, CommandStructure value) {
		Debug.Log (path);
		Folder newFolder = shell.folder.GetFolder (path);
		if (newFolder != null) {
			Debug.Log (newFolder.name);
			shell.UpdateAddress (newFolder);
		}
	}
}
