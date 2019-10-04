using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Vector3 direction;

	// Use this for initialization
	void Awake () {
		spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Follow(Vector3 pos, bool extra) {
		//Rotation;
		Vector3 dir = pos - transform.position;
		Quaternion angleAxis = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
		float length;
		if (!extra)
			length = spriteRenderer.size.y;
		else
			length = spriteRenderer.size.y;

		Vector3 lengthDir = dir.normalized * length;

		transform.position = pos + (lengthDir * -1);

		direction = lengthDir;
	}

	public Vector3 GetEnd() {
		return transform.position + direction;
	}
}
