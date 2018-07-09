using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparenceHolder : MonoBehaviour {

	public aparenceToName[] aparences;
	// Use this for initialization
	void Start () {
		Theme.aparences = aparences;
	}

}

[System.Serializable]
public class aparenceToName {
	public Aparence aparence;
	public string Name;
}
