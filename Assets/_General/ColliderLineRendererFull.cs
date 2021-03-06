﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLineRendererFull : MonoBehaviour {

	public Material lineMaterial;
	public float width = 0.02f;
	public Transform firstChild;

	// Use this for initialization
	void Start () {
		if (firstChild == null) {
			if (transform.childCount == 0) {
				Debug.Log ("No childs detected");
				firstChild = transform;
			} else {
				firstChild = transform.GetChild (0);
			}
		}
		CreateBorders ();
	}

	public void CreateBorders() {
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertex = mesh.vertices;
		int[] triangles = mesh.triangles;
		List<Line> lines = new List<Line> ();

		for (int i = 0; i < triangles.Length; i += 3) {
			Vector3 v1 = vertex [triangles [i]];
			Vector3 v2 = vertex [triangles [i+1]];
			Vector3 v3 = vertex [triangles [i+2]];
			DrawLine(v1, v2, lines);
			DrawLine(v2, v3, lines);
		}
	}


	void DrawLine(Vector3 start, Vector3 end, List<Line> lines) {
		for (int i = 0; i < lines.Count; ++i) {
			if ( 
				(start == lines [i].v1 || start == lines [i].v2) &&
				(end == lines [i].v1 || end == lines [i].v2))
					return;
		}
		Line line = new Line ();
		line.v1 = start;
		line.v2 = end;
		lines.Add (line);
		Lines.RenderStraightLine (firstChild, start, end, width, lineMaterial);
	}

	class Line {
		public Vector3 v1;
		public Vector3 v2;
	}
}
