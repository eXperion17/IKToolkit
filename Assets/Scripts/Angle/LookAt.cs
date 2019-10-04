using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private MeshRenderer meshRenderer;

	private Vector3 direction;
	[SerializeField]
	private bool is3D;

	// Use this for initialization
	void Awake () {
		/*if (is3D)
			meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
		else
			spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Follow(Transform obj) {
		//Rotation;
		Vector3 dir = obj.position - transform.position;
		/*
		Quaternion angleAxis = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
		*/

		//Might need some replacin
		transform.LookAt(obj);
		transform.Rotate(90, 0, 0);

		float length = GetLength();

		Vector3 lengthDir = dir.normalized * length;

		transform.position = obj.position + (lengthDir * -1);

		direction = lengthDir;
	}

	private float GetLength() {
		//return meshRenderer.bounds.size.y;
		if (is3D)
			return 3;
		else
			return spriteRenderer.size.y;
	}
	

	public Vector3 GetEnd() {
		return transform.position + direction;
	}
}
