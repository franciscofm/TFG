using UnityEngine;

[System.Serializable]
public class Connection {

	public int ownIfaceId;
	public int otherIfaceId;
	public Node ownNode;
	public Node otherNode;
	public GameObject line;

	public Connection() {
		
	}
}