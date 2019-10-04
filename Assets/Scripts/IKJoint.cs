using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKJoint : MonoBehaviour {
	public Vector3 length;


	private void Start() {
		if (transform.childCount > 0) {
			length = (transform.GetChild(0).position - transform.position);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		
	}


	private void OnDrawGizmos() {
		if (transform.childCount > 0)
			Gizmos.DrawLine(transform.position, transform.position + length);
	}

	public void MoveTo(Vector3 pos) {
		IKJoint piece = transform.parent.GetComponent<IKJoint>();

		while (piece && piece.transform.parent != null) {
			piece.transform.parent.GetComponent<IKJoint>();
		}
	}
}
