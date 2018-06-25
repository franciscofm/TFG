using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public EthPort[] Ports;

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
		int ip_a, int ip_b, int ip_c, int ip_d,
		int m_a, int m_b, int m_c, int m_d,
		int b_a, int b_b, int b_c, int b_d) {

		//TODO comprobacion errores

		ip = new int[]{ ip_a, ip_b, ip_c, ip_d };
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