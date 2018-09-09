using System;
using System.Collections;
using System.Collections.Generic;

public static class Mkdir {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		switch (command.Length) {
		case 1:
			Create (command [0], shell, value);
			break;
		default:
			break;
		}
	}

	public static void Create(string path, Shell shell, CommandStructure value) {
		string[] splited = path.Split (new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
		string s = "";
		if (path.StartsWith ("/")) s += "/";
		for (int i = 0; i < splited.Length - 1; ++i) s += splited [i] + "/";
		Folder f;

		if(splited.Length > 1) 
			f = shell.folder.GetFolder (s);
		else
			f = shell.folder;
		
		if (f == null) {
			value.prompt = true;
			value.correct = false;
			value.value = "Mkdir: bad folder";
		} else {
			if (f.GetFolder (splited [splited.Length - 1]) != null) {
				value.prompt = true;
				value.correct = false;
				value.value = "Mkdir: folder already existing";
			} else {
				value.correct = true;
				Folder nF = new Folder (splited [splited.Length - 1], f);
				f.folders.Add (nF);
			}
		}
	}
}
