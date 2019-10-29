using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionTest : MonoBehaviour {

	private Vector3 baseL;
	private RaycastHit firstHit;
	private bool firstHitDone;

	private Vector3 prevPos;
	private Vector3 contactPoint;
	private Vector3 projPlane;

	// Use this for initialization
	void Start () {
		firstHitDone = false;
		baseL = Vector3.down * 0.05f;
		prevPos = transform.position;
	}

	private void LateUpdate() {
		prevPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;

		var delta = transform.position - prevPos;

		if (Physics.Raycast(transform.position, transform.forward, out hit, 10)) {
			if (!firstHitDone) {
				firstHit = hit;
				firstHitDone = true;
				contactPoint = firstHit.point;
			}
			Debug.DrawLine(baseL+hit.point, baseL+hit.point + hit.normal, Color.blue);
		}

		Debug.DrawLine(baseL + transform.position, baseL + hit.point, Color.red);
		firstHit.point += delta;

		if (firstHitDone) {
			Debug.DrawLine(contactPoint, contactPoint+Vector3.ProjectOnPlane(transform.forward, firstHit.normal), Color.green);
			Debug.Log(Vector3.ProjectOnPlane(transform.forward, firstHit.normal));
			projPlane = Vector3.ProjectOnPlane(transform.forward, firstHit.normal);

			Debug.DrawLine(contactPoint, contactPoint + Vector3.up, Color.magenta);
			//delta.Scale(Vector3.Project(transform.forward, firstHit.normal));
			contactPoint += delta;
		}

	}
}
