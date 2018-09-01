using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder : Inode {

	public static Folder root;
	public Folder parent;
	public Folder self;

	public List<Folder> folders;
	public List<File> files;

	public Folder(string name, Folder parent, string creator = "admin", string owner = "admin", bool[] permissions = null) {

		this.parent = parent;
		self = this;

		this.name = name;
		this.creator = creator;
		this.owner = owner;

		if (permissions == null) this.permissions = Utils.PermissionDefault;
		else this.permissions = permissions;

		folders = new List<Folder> ();
		files = new List<File> ();
	}

	public Folder GetFolder(string path) {
		string[] splited = path.Split (new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
		Folder current = (path.StartsWith("/")) ? root : self;
		for (int i = 0; i < splited.Length; ++i) {
			bool found = false;
			for (int j = 0; j < current.folders.Count && !found; ++j) {
				if (current.folders [j].name == splited [i]) {
					found = true;
					current = current.folders [j];
				}
			}
			if (!found) return null;
		}
		return current;
	}
	public File GetFile(string path) {
		string[] splited = path.Split (new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
		Folder current = self;
		for (int i = 0; i < splited.Length; ++i) {
			if (i == splited.Length - 1) {
				bool found = false;
				for (int j = 0; j < current.files.Count && !found; ++j) {
					if (current.files [j].name == splited [i]) {
						found = true;
						return current.files [j];
					}
				}
				if (!found) return null;
			} else {
				bool found = false;
				for (int j = 0; j < current.folders.Count && !found; ++j) {
					if (current.folders [j].name == splited [i]) {
						found = true;
						current = current.folders [j];
					}
				}
				if (!found) return null;
			}
		}
		return null;
	}

	public string GetPathString() {
		string path = "";
		Folder current = this;
		while (current.parent != current.self) {
			path.Insert (0, "/" + current.name);
			current = current.parent;
		}
		return path;
	}
	public List<Folder> GetPathList() {
		List<Folder> folders = new List<Folder> ();
		Folder current = this;
		while (current.parent != current.self) {
			folders.Insert (0, current);
			current = current.parent;
		}
		return folders;
	}
}
