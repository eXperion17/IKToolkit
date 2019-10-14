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

	[Header("Constraints")]
	public Vector2 ZRotationLimits;
	public Vector2 YRotationLimits;
	public Vector2 XRotationLimits;

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

	public void Follow(Transform obj, bool keepRotation = false, bool repeat = false) {
		//Rotation;
		Vector3 dir = obj.position - transform.position;
		/*
		Quaternion angleAxis = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
		*/

		//Might need some replacin
		if (!keepRotation) {
			if (!repeat) {
				transform.LookAt(obj);
				transform.Rotate(90, 0, 0);
			}

			if (ZRotationLimits.x > 0 && !repeat) {
				var currRotation = transform.rotation.eulerAngles;
				if (transform.rotation.eulerAngles.z > ZRotationLimits.x) {
					currRotation.z = ZRotationLimits.x;
				} else if (transform.rotation.eulerAngles.z < ZRotationLimits.y) {
					currRotation.z = ZRotationLimits.y;
				}
				//Y
				if (transform.rotation.eulerAngles.y > YRotationLimits.x) {
					currRotation.y = YRotationLimits.x;
				}
				else if (transform.rotation.eulerAngles.y < YRotationLimits.y) {
					currRotation.y = YRotationLimits.y;
				}
				//Y
				
				if (transform.rotation.eulerAngles.x > XRotationLimits.x) {
					currRotation.x = XRotationLimits.x;
				}
				else if (transform.rotation.eulerAngles.x < XRotationLimits.y) {
					currRotation.x = XRotationLimits.y;
				}


				transform.rotation = Quaternion.Euler(currRotation);
				repeat = true;
				if (repeat) {
					Follow(obj, keepRotation, true);
					return;
				}
			}
		}

		float length = GetLength(keepRotation);

		Vector3 lengthDir = dir.normalized * length;
		if (!keepRotation)
			transform.position = obj.position + (lengthDir * -1);
		else {
			transform.position = obj.position;
			transform.rotation = obj.rotation;
		}
			

		direction = lengthDir;
	}

	private float GetLength(bool keepRotation = false) {
		//return meshRenderer.bounds.size.y;
		if (is3D) {
			if (keepRotation)
				return meshRenderer.bounds.size.y;
			else
				return 3;
		}
		else
			return spriteRenderer.size.y;
	}


	public Vector3 GetEnd() {
		//return transform.position + direction;
		return transform.position + transform.TransformDirection(Vector3.up) * GetLength();
	}
}
