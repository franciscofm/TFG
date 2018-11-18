using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interface))]
public class InterfaceVisuals : MonoBehaviour {

	public Animator animator;

	// Use this for initialization
	void Awake () {
		if (animator == null) animator = GetComponent<Animator> ();
	}

}
