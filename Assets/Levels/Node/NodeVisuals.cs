﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[RequireComponent(typeof(Node))]
public class NodeVisuals : MonoBehaviour {

	public Animator animator;
	public GameObject standardModel;
	public GameObject classicModel;
	[HideInInspector] public MeshRenderer meshRenderer;
	GameObject outlineObject;
	bool init;

	// Use this for initialization
	void Awake () {
		if (animator == null) animator = GetComponent<Animator> ();
		Node node = GetComponent<Node> ();
		node.OnClickDown += OnClickDown;
		node.OnClickUp += OnClickUp;
	}

	InterfaceVisuals[] ifaceVisuals;
	public void Load() {
		ifaceVisuals = GetComponentsInChildren<InterfaceVisuals> (true);
		init = true;
		if (User.classic_nodes) {
			Destroy(standardModel);
			classicModel.SetActive (true);
			meshRenderer = classicModel.GetComponent<MeshRenderer> ();
			outlineObject = classicModel.transform.GetChild (0).gameObject;
		} else {
			Destroy(classicModel);
			standardModel.SetActive (true);
			meshRenderer = standardModel.GetComponent<MeshRenderer> ();
			outlineObject = standardModel.transform.GetChild (0).gameObject;
		}
	}

	public void OnMouseEnter() {
		if (!init) return;
		outlineObject.SetActive (true);
	}
	public void OnMouseExit() {
		if (!init) return;
		outlineObject.SetActive (false);
	}

	public AnimationInfo OnClickDownAnimation;
	protected virtual void OnClickDown(Node node) {
		if (!string.IsNullOrEmpty (OnClickDownAnimation.state))
			animator.Play (OnClickDownAnimation.state, OnClickDownAnimation.layer);
	}

	public AnimationInfo OnClickUpAnimation;
	protected virtual void OnClickUp(Node node) {
		if (!string.IsNullOrEmpty (OnClickUpAnimation.state))
			animator.Play (OnClickUpAnimation.state, OnClickUpAnimation.layer);
	}

	public void ChangeNodeColor(Color c) {
		meshRenderer.material.color = c;
		foreach (InterfaceVisuals iv in ifaceVisuals)
			iv.ChangeColor (c);
	}
}
