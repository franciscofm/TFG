using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ls {

	public static void Command(string[] command, Shell shell, CommandStructure value) {
		value.prompt = true;

		switch (command.Length) {
		case 0:
			value.correct = true;
			ListAll (false, false, false, shell.folder, value);
			break;
		default:
			ReadOptions(command, shell, value);
			break;
		}
	}

	public static void ReadOptions(string[] command, Shell shell, CommandStructure value) {
		bool hiden = false, details = false, color = false;
		bool target = false, multiple = false;
		List<string> paths = new List<string> ();
		Folder root = shell.folder;

		//solo pilla -la y -c como --color
		foreach (string word in command) { //cada parametro
			if (word.StartsWith ("-")) { //si es opcion
				hiden = word.Contains ("a");
				details = word.Contains ("l");
				color = word.Contains ("c");
			} else { //si es directorio o fichero
				if (target)
					multiple = true;
				target = true;
				paths.Add (word);
			}
		}

		if (target) { //si tenemos directorio/fichero especificado
			foreach (string s in paths) { //para cada uno
				Folder folder = root.GetFolder (s);
				if (folder == null) {
					File file = root.GetFile (s);
					if (file != null)
						ListInode (details, color, value, file, false);
				} else {
					if(multiple)
						value.value += folder.name + ":" + Console.jump;
					ListAll (details, hiden, color, folder, value);
				}
			}
		} else { //si no, carpeta actual
			ListAll (details, hiden, color, shell.folder, value);
		}
	}

	public static void ListAll(bool details, bool hiden, bool color, Folder folder, CommandStructure value) {
		int c = 0;
		foreach (Folder f in folder.folders) {
			if (hiden || !f.name.StartsWith (".")) { //Si enseñamos todos o no es oculto, lo mostramos
				ListInode (details, color, value, f, true);
				++c;
				if (!hiden && c % 4 == 0) //si no es lista, hacemos 4 columnas
					value.value += Console.jump;
			}
		}
		c = 0;
		foreach(File f in folder.files) {
			if(hiden || !f.name.StartsWith(".")){
				ListInode(details, color, value, f, false);
				++c;
				if (!hiden && c % 4 == 0)
					value.value += Console.jump;
			}
		}
		if (!hiden && c % 4 != 0) value.value += Console.jump; //pading final si no es multiple de 4
	}
	public static void ListInode(bool details, bool color, CommandStructure value, Inode inode, bool folder) {
		//folder or file
		if (details) {
			value.value += folder ? "d" : "-";

			//permissions rwx
			for (int i = 0; i < 9; ++i) {
				if (inode.permissions [i]) {
					if (i % 3 == 0) value.value += "r";
					else if (i % 3 == 1) value.value += "w";
					else value.value += "x";
				} else value.value += "-";
			}

			value.value += " " + inode.owner;
			value.value += " " + inode.creator;
			value.value += " " + inode.date;
			value.value += " ";
		}

		value.value += inode.name + " ";

		if (details) value.value += Console.jump;
	}
}
