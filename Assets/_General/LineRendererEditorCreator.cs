using UnityEngine;

[RequireComponent(typeof(ColliderLineRendererFull))]
[ExecuteInEditMode]
public class LineRendererEditorCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ColliderLineRendererFull> ().CreateBorders ();
	}

}
