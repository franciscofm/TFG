using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interface))]
public class InterfaceVisuals : MonoBehaviour {

	public Animator animator;
	public Transform nodeAnchor;
	Interface iface;
	// Use this for initialization
	void Awake () {
		if (animator == null) animator = GetComponent<Animator> ();
		if (nodeAnchor == null) nodeAnchor = transform.parent;

		iface = GetComponent<Interface> ();
		iface.OnSelect += OnSelect;
		iface.OnUnselect += OnUnselect;
		iface.OnConnect += OnConnect;
		iface.OnDisconnect += OnDisconnect;
		iface.OnGetUp += OnGetUp;
		iface.OnGetDown += OnGetDown;
	}

	void Start() {
		if (iface.IsUp ()) OnGetUp (iface);
		if (iface.connectedTo != null) OnConnect (iface);
	}

	public AnimationInfo OnSelectAnimation;
	protected virtual void OnSelect(Interface iface) {
		if (!string.IsNullOrEmpty (OnSelectAnimation.state))
			animator.Play (OnSelectAnimation.state, OnSelectAnimation.layer);
	}
	public AnimationInfo OnUnselectAnimation;
	protected virtual void OnUnselect(Interface iface) {
		if (!string.IsNullOrEmpty (OnUnselectAnimation.state))
			animator.Play (OnUnselectAnimation.state, OnUnselectAnimation.layer);
	}

	public AnimationInfo OnConnectAnimation;
	protected virtual void OnConnect(Interface iface) {
		//ring
		if (!string.IsNullOrEmpty (OnConnectAnimation.state))
			animator.Play (OnConnectAnimation.state, OnConnectAnimation.layer);

		//rotate looking at the other node
		StartCoroutine(LookAt());
	}
	IEnumerator LookAt() {
		Quaternion rotStart = nodeAnchor.localRotation;
		Quaternion rotEnd = Quaternion.LookRotation(iface.connectedTo.node.transform.position - nodeAnchor.position);
		float t = 0f;
		while (t < 0.5f) {
			yield return null;
			t += Time.deltaTime;
			nodeAnchor.localRotation = Quaternion.Lerp (rotStart, rotEnd, t / 0.5f);
		}
		//create line
	}
	public AnimationInfo OnDisconnectAnimation;
	protected virtual void OnDisconnect(Interface iface) {
		if (!string.IsNullOrEmpty (OnDisconnectAnimation.state))
			animator.Play (OnDisconnectAnimation.state, OnDisconnectAnimation.layer);
	}

	public AnimationInfo OnGetUpAnimation;
	protected virtual void OnGetUp(Interface iface) {
		if (!string.IsNullOrEmpty (OnGetUpAnimation.state))
			animator.Play (OnGetUpAnimation.state, OnGetUpAnimation.layer);
	}
	public AnimationInfo OnGetDownAnimation;
	protected virtual void OnGetDown(Interface iface) {
		if (!string.IsNullOrEmpty (OnGetDownAnimation.state))
			animator.Play (OnGetDownAnimation.state, OnGetDownAnimation.layer);
	}
}
