using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public EthPort[] Ports;

	public GameObject RenderLine(Transform t1, Transform t2) {
		GameObject go = new GameObject ("Line: " + t1.gameObject.name + " --> " + t2.gameObject.name);
		go.transform.parent = t1;
		LineRenderer line = go.AddComponent<LineRenderer> ();
		line.positionCount = 2;
		line.SetPosition (0, t1.position);
		line.SetPosition (1, t2.position);
		line.widthMultiplier = 0.1f;
		return go;
	}

	public delegate void NodeEvent(Node sender);
	public event NodeEvent OnClick;

	void OnMouseUp() {
		if(EventSystem.current.IsPointerOverGameObject()) return;
		if(OnClick != null)
			OnClick (this);
	}
}

[System.Serializable]
public class EthPort {
	public string Name;
	public GameObject Port;
	public IPDirection IP;
}

[System.Serializable]
public class IPDirection {

	public int[] ip;
	public int[] mask;
	public int[] broadcast;

	public IPDirection(
		int i_a, int i_b, int i_c, int i_d,
		int m_a, int m_b, int m_c, int m_d,
		int b_a, int b_b, int b_c, int b_d) {

		//TODO comprobacion errores
		if(i_a > 255 || i_b > 255 || i_c > 255 || i_d > 255) Debug.LogFormat("Error ip: {0}.{1}.{2}.{3}", new object[]{ i_a, i_b, i_c, i_d });
		if(m_a > 255 || m_b > 255 || m_c > 255 || m_d > 255) Debug.LogFormat("Error mask: {0}.{1}.{2}.{3}", new object[]{ m_a, m_b, m_c, m_d });
		if(b_a > 255 || b_b > 255 || b_c > 255 || b_d > 255) Debug.LogFormat("Error broadcast: {0}.{1}.{2}.{3}", new object[]{ b_a, b_b, b_c, b_d });

		ip = new int[]{ i_a, i_b, i_c, i_d };
		mask = new int[]{ m_a, m_b, m_c, m_d };
		broadcast = new int[]{ b_a, b_b, b_c, b_d };
	}

	public string GetClass() {
		if (ip [0] < 128) return "A";
		if (ip [0] < 192) return "B";
		if (ip [0] < 224) return "C";
		if (ip [0] < 240) return "D";
		return "E";
	}
}