﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interface))]
public class InterfaceVisuals : MonoBehaviour {

	public Animator animator;
	public Transform infoAnchor;
	public Material lineMaterial;
	
	public Transform nodeAnchor;
	[Header("Debug")]
	public GameObject infoObject;
	Text infoText;

	public static List<InterfaceVisuals> allVisuals = new List<InterfaceVisuals>();
	public Interface iface;
	public Interface[] otherIfaces;

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

		allVisuals.Add (this);
	}
	public void InitVisuals() {
		if (iface.IsUp ()) OnGetUp (iface);
		if (iface.connectedTo != null) OnConnect (iface);

		if (infoObject.activeSelf) infoObject.SetActive (false);
		infoText = infoObject.GetComponentInChildren<Text> ();

		//print (iface.node);
		Node node = iface.node;
		otherIfaces = new Interface[node.Interfaces.Length - 1];
		for (int i = 0, j = 0; i < node.Interfaces.Length; ++i) {
			if (node.Interfaces [i] != iface) {
				otherIfaces [j] = node.Interfaces [i];
				++j;
			}
		}

		Vector3 localPos = transform.localPosition;
		Vector3 start = Vector3.zero;
		transform.localPosition = start;
		StartCoroutine (Routines.WaitFor (transform.parent.GetSiblingIndex () * 0.15f, delegate {
			StartCoroutine(Routines.DoWhile(0.3f, delegate(float f) {
				transform.localPosition = Vector3.Lerp(start,localPos,f);
			}));
		}));
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
	[HideInInspector] public GameObject connectionLine;
	protected virtual void OnConnect(Interface other) {
		if (!string.IsNullOrEmpty (OnConnectAnimation.state))
			animator.Play (OnConnectAnimation.state, OnConnectAnimation.layer);

		//rotate looking at the other node TODO dont overlap with other ifaces
		List<Interface> overlapingIfaces = new List<Interface>();
		foreach (Interface i in otherIfaces) {
			if (i.connectedTo && i.connectedTo.node == other.node) {
				overlapingIfaces.Add (i);
			}
		}
		if (overlapingIfaces.Count == 0)
			StartCoroutine (LookAt (other));
		else {
			overlapingIfaces.Add (iface);
			StartCoroutine (MultipleLookAt (overlapingIfaces));
		}
	}
	IEnumerator MultipleLookAt(List<Interface> ifaces) {
		int total = ifaces.Count;
		//Get initial rotations
		Transform[] transforms = new Transform[total];
		Transform[] otherTransforms = new Transform[total];
		Quaternion[] rotsStart = new Quaternion[total];
		Quaternion[] otherRotsStart = new Quaternion[total];
		for (int i = 0; i < total; ++i) {
			transforms [i] = ifaces [i].transform;
			otherTransforms [i] = ifaces [i].connectedTo.transform;

			rotsStart [i] = transforms [i].rotation;
			otherRotsStart [i] = otherTransforms [i].rotation;
		}
		
		Quaternion[] rotsEnd = new Quaternion[total];
		Quaternion[] otherRotsEnd = new Quaternion[total];
		Quaternion baseRotation = Quaternion.LookRotation(iface.connectedTo.node.transform.position - nodeAnchor.position);
		float offset = 15f;
		float baseY = baseRotation.eulerAngles.y - offset * 0.5f * total;
		float otherBaseY = baseRotation.eulerAngles.y + offset * 0.5f * total;
		for (int i = 0; i < total; ++i) {
			rotsEnd[i] = Quaternion.Euler(0, baseY + i * offset, 0);
			otherRotsEnd[i] = Quaternion.Euler(0, otherBaseY - i * offset, 0);
		}
		
		float t = 0f;
		while (t < 0.5f) {
			yield return null;
			t += Time.deltaTime;
			for (int i = 0; i < total; ++i) {
				transforms [i].rotation = Quaternion.Lerp (rotsStart[i], rotsEnd[i], t / 0.5f);
				otherTransforms [i].rotation = Quaternion.Lerp (otherRotsStart[i], otherRotsEnd[i], t / 0.5f);
			}
		}

		for (int i = 0; i < total; ++i) {
			Lines.Pair pair = Lines.RenderStraightLine (transforms [i], otherTransforms [i].position, 0.02f, 0.2f);
			connectionLine = pair.gameObject;
			pair.lineRenderer.material = lineMaterial;

			InterfaceVisuals otherVisual = otherTransforms [i].GetComponent<InterfaceVisuals> ();
			otherVisual.connectionLine = pair.gameObject;
		}
	}
	IEnumerator LookAt(Interface other) {
		Quaternion rotStart = nodeAnchor.rotation;
		Quaternion rotEnd = Quaternion.LookRotation(other.node.transform.position - nodeAnchor.position);
		float t = 0f;
		while (t < 0.5f) {
			yield return null;
			t += Time.deltaTime;
			nodeAnchor.rotation = Quaternion.Lerp (rotStart, rotEnd, t / 0.5f);
		}
		//create line
		if (connectionLine == null) {
			Lines.Pair pair = Lines.RenderStraightLine (transform, other.transform.position, 0.02f, 0.2f);
			connectionLine = pair.gameObject;
			pair.lineRenderer.material = lineMaterial;

			InterfaceVisuals otherVisual = other.GetComponent<InterfaceVisuals> ();
			otherVisual.connectionLine = pair.gameObject;
		}
	}
	public AnimationInfo OnDisconnectAnimation;
	protected virtual void OnDisconnect(Interface other) {
		if (!string.IsNullOrEmpty (OnDisconnectAnimation.state))
			animator.Play (OnDisconnectAnimation.state, OnDisconnectAnimation.layer);
		print (gameObject.name);
		if (connectionLine != null)
			Destroy (connectionLine);
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

	public virtual void ShowInformation() {
		infoObject.SetActive (true);
		infoText.text = 
			iface.Name + " " + iface.ip.word + Console.jump +
			"mask " + iface.netmask.word;
		infoObject.transform.position = Camera.main.WorldToScreenPoint(infoAnchor.position);
	}
	public virtual void HideInformation() {
		infoObject.SetActive (false);
	}
}
