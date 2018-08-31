using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple Inode representation, no weight and pages.
/// </summary>
public class Inode {
	
	public bool[] permissions;

	public string owner;
	public string creator;
	public string name;

	public DateTime creation;

	public Inode() {

	}

	public Inode(string name, string creator = "admin", string owner = "admin", bool[] permissions = null) {
		this.name = name;
		this.creator = creator;
		this.owner = owner;

		if (permissions == null) this.permissions = Utils.PermissionDefault;
		else this.permissions = permissions;
	}
}
