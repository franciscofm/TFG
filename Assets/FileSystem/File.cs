using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : Inode {

	public string value;

	public File(string name, string value = "", string creator = "admin", string owner = "admin", bool[] permissions = null) {
		this.name = name;
		this.value = value;
		this.creator = creator;
		this.owner = owner;

		if (permissions == null) this.permissions = Utils.PermissionDefault;
		else this.permissions = permissions;
	}

}
