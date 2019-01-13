using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

[RequireComponent(typeof(Interface))]
public class InterfaceVisuals : MonoBehaviour {

	public Animator animator;
	public Transform infoAnchor;
	public Material lineMaterial;
	public GameObject standardModel;
	public GameObject classicModel;
	[HideInInspector] public Transform modelTransform;
	[HideInInspector] public MeshRenderer meshRenderer;
	
	[HideInInspector] public Transform nodeAnchor;
	[Header("Debug")]
	public GameObject infoObject;
	GameObject outlineObject;
	InterfaceInfo ifaceInfo;

	public Interface iface;
	public Interface[] otherIfaces;

	// Use this for initialization
	public void InitVisuals() {
		if (animator == null) animator = GetComponent<Animator> ();
		if (nodeAnchor == null) nodeAnchor = transform.parent;

		iface.OnSelect += OnSelect;
		iface.OnUnselect += OnUnselect;
		iface.OnConnect += OnConnect;
		iface.OnDisconnect += OnDisconnect;
		iface.OnGetUp += OnGetUp;
		iface.OnGetDown += OnGetDown;

		if (User.classic_nodes) {
			standardModel.SetActive (false);
			classicModel.SetActive (true);
			meshRenderer = classicModel.GetComponent<MeshRenderer> ();
			modelTransform = classicModel.transform;
		} else {
			standardModel.SetActive (true);
			classicModel.SetActive (false);
			meshRenderer = standardModel.GetComponent<MeshRenderer> ();
			modelTransform = standardModel.transform;
		}
		outlineObject = modelTransform.GetChild (0).gameObject;

		if (iface.IsUp ()) OnGetUp (iface);
		if (iface.connectedTo != null) OnConnect (iface);

		if (infoObject.activeSelf) infoObject.SetActive (false);
		ifaceInfo = infoObject.GetComponentInChildren<InterfaceInfo> ();

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
	void OnDestroy() {
		iface.OnSelect -= OnSelect;
		iface.OnUnselect -= OnUnselect;
		iface.OnConnect -= OnConnect;
		iface.OnDisconnect -= OnDisconnect;
		iface.OnGetUp -= OnGetUp;
		iface.OnGetDown -= OnGetDown;
	}

	public void OnMouseEnter() {
		outlineObject.SetActive (true);
	}
	public void OnMouseExit() {
		outlineObject.SetActive (false);
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
	[HideInInspector] public bool blockAnimation;
	protected virtual void OnConnect(Interface other) {
		if (!string.IsNullOrEmpty (OnConnectAnimation.state))
			animator.Play (OnConnectAnimation.state, OnConnectAnimation.layer);

		//rotate looking at the other node TODO dont overlap with other ifaces
		if(blockAnimation) return;
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
		InterfaceVisuals[] visuals = new InterfaceVisuals[total];
		InterfaceVisuals[] otherVisuals = new InterfaceVisuals[total];
		for (int i = 0; i < total; ++i) {
			visuals[i] = ifaces [i].GetComponent<InterfaceVisuals> ();
			otherVisuals[i] = ifaces [i].connectedTo.GetComponent<InterfaceVisuals> ();
			visuals[i].blockAnimation = true;
			otherVisuals[i].blockAnimation = true;
			transforms [i] = visuals[i].nodeAnchor;
			otherTransforms [i] = otherVisuals[i].nodeAnchor;

			rotsStart [i] = transforms [i].rotation;
			otherRotsStart [i] = otherTransforms [i].rotation;
		}

		//Final rotations maths
		Quaternion[] rotsEnd = new Quaternion[total];
		Quaternion[] otherRotsEnd = new Quaternion[total];
		Quaternion baseRotation = Quaternion.LookRotation(nodeAnchor.position - iface.connectedTo.node.transform.position);
		float offset = 15f;
		float baseY = baseRotation.eulerAngles.y - offset * 0.5f * total;
		float otherBaseY = baseRotation.eulerAngles.y + 180f + offset * 0.5f * total;
		for (int i = 0; i < total; ++i) {
			//print ((baseY + i * offset * 2f) + ", " + (otherBaseY - i * offset * 2f));
			otherRotsEnd[i] = Quaternion.Euler(0, baseY + i * offset * 2f, 0);
			rotsEnd[i] = Quaternion.Euler(0, otherBaseY - i * offset * 2f, 0);
		}

		//Movement
		float t = 0f;
		while (t < 0.5f) {
			yield return null;
			t += Time.deltaTime;
			float p = t / 0.5f;
			for (int i = 0; i < total; ++i) {
				transforms [i].rotation = Quaternion.Lerp (rotsStart[i], rotsEnd[i], p);
				otherTransforms [i].rotation = Quaternion.Lerp (otherRotsStart[i], otherRotsEnd[i], p);
			}
		}

		//Create lines
		for (int i = 0; i < total; ++i) {
			visuals[i].blockAnimation = false;
			if (visuals [i].connectionLine == null) {
				Lines.Pair pair = Lines.RenderStraightLine (visuals [i].modelTransform, otherVisuals [i].modelTransform, 0.02f, 0.2f);
				connectionLine = pair.gameObject;
				pair.lineRenderer.material = lineMaterial;

				visuals [i].StartCoroutine (LineRoutine (pair.lineRenderer, visuals [i].modelTransform, otherVisuals [i].modelTransform, 0.2f));
				otherVisuals [i].blockAnimation = false;
				otherVisuals [i].connectionLine = pair.gameObject;

				visuals [i].line = pair.lineRenderer;
				visuals [i].connectedTo = otherVisuals [i];
				otherVisuals [i].line = pair.lineRenderer;
				otherVisuals [i].connectedTo = visuals [i];

				visuals [i].ChangeColor(visuals [i].meshRenderer.material.color);
			}
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
			connectedTo = other.GetComponent<InterfaceVisuals> ();
			Lines.Pair pair = Lines.RenderStraightLine (modelTransform, connectedTo.modelTransform, 0.02f, 0.2f);
			StartCoroutine (LineRoutine (pair.lineRenderer, modelTransform, connectedTo.modelTransform, 0.2f));
			connectionLine = pair.gameObject;
			pair.lineRenderer.material = lineMaterial;

			connectedTo.connectionLine = pair.gameObject;
			line = pair.lineRenderer;
			connectedTo.connectedTo = this;
			connectedTo.line = line;

			ChangeColor(meshRenderer.material.color);
		}
	}

	[HideInInspector] public LineRenderer line;
	[HideInInspector] public InterfaceVisuals connectedTo;
	IEnumerator LineRoutine(LineRenderer line, Transform start, Transform end, float offsets) {
		while (true) {
			yield return null;
			if(line == null) yield break;
			if (offsets != 0f) {
				Vector3 direction = end.position - start.position;
				direction.Normalize ();
				line.SetPosition (0, start.position + direction * offsets);
				line.SetPosition (1, end.position - direction * offsets);
			} else {
				line.SetPosition (0, start.position);
				line.SetPosition (1, end.position);
			}
		}
	}
	public void ChangeColor(Color c) {
		meshRenderer.material.color = c;
		if (line != null) {
			Gradient gradient = new Gradient ();
			gradient.SetKeys (
				new GradientColorKey[] { new GradientColorKey(c, 0f), new GradientColorKey(connectedTo.meshRenderer.material.color, 1f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
			);
			line.colorGradient = gradient;
		}
	}

	public AnimationInfo OnDisconnectAnimation;
	protected virtual void OnDisconnect(Interface other) {
		if (!string.IsNullOrEmpty (OnDisconnectAnimation.state))
			animator.Play (OnDisconnectAnimation.state, OnDisconnectAnimation.layer);
		
		if (connectionLine != null) {
			Destroy (connectionLine);
		}
		if (connectedTo != null) {
			connectedTo.line = null;
			connectedTo.connectedTo = null;
			connectedTo = null;
			line = null;
		}
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
		ifaceInfo.Set( 
			iface.Name + " " + iface.ip.word + Console.jump +
			"mask " + iface.netmask.word, iface.isUp
		);
		infoObject.transform.position = Camera.main.WorldToScreenPoint(infoAnchor.position);
	}
	public virtual void HideInformation() {
		infoObject.SetActive (false);
	}
}
