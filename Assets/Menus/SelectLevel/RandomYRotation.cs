using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomYRotation : MonoBehaviour {

	public float minSpeed = 3f;
	public float maxSpeed = 5f;
	float speed;

	// Use this for initialization
	void Start () {
		speed = Random.Range (minSpeed, maxSpeed) * ((Random.value <= 0.5f) ? -1f : 1f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0f, speed * Time.deltaTime, 0f);
	}

}
