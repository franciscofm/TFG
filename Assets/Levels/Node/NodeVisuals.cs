using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[RequireComponent(typeof(Node))]
public class NodeVisuals : MonoBehaviour {

	public Animator animator;
	public GameObject standardModel;
	public GameObject classicModel;
	MeshRenderer meshRenderer;

	// Use this for initialization
	void Awake () {
		if (animator == null) animator = GetComponent<Animator> ();
		Node node = GetComponent<Node> ();
		node.OnClickDown += OnClickDown;
		node.OnClickUp += OnClickUp;
	}

	InterfaceVisuals[] ifaceVisuals;
	void Start() {
		ifaceVisuals = GetComponentsInChildren<InterfaceVisuals> (true);
		if (User.classic_nodes) {
			standardModel.SetActive (false);
			classicModel.SetActive (true);
			meshRenderer = classicModel.GetComponent<MeshRenderer> ();
		} else {
			standardModel.SetActive (true);
			classicModel.SetActive (false);
			meshRenderer = standardModel.GetComponent<MeshRenderer> ();
		}
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
