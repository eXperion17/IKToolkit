using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UnityEventRay : UnityEvent<RaycastHit> {
	//public RaycastHit raycastHit;
}

public class RaycastChecker : MonoBehaviour {
	[SerializeField]
	private float raycastInterval = 1f;
	[SerializeField]
	private float maxDistance = 1f;
	
	private Vector3[] directions;
	private float currInterval;
	public UnityEventRay OnRayHitEvent;
	private bool isActive = true;

	// Use this for initialization
	void Start () {
		currInterval = 0;

		directions = new Vector3[6];
		directions[0] = Vector3.forward;
		directions[1] = Vector3.back;
		directions[2] = Vector3.left;
		directions[3] = Vector3.right;
		directions[4] = Vector3.up;
		directions[5] = Vector3.down;

		//OnRayHitEvent.AddListener(TestyTest);
	}

	public void AddListener(UnityAction<RaycastHit> method) {
		if (OnRayHitEvent == null)
			OnRayHitEvent = new UnityEventRay();

		OnRayHitEvent.AddListener(method);
	}

	public void Toggle(bool value) {
		isActive = value;
	}
	
	
	// Update is called once per frame
	void Update () {
		//Debug lines
		Debug.DrawLine(transform.position, transform.position + (directions[0] * maxDistance), Color.red);
		Debug.DrawLine(transform.position, transform.position + (directions[1] * maxDistance), Color.green);
		Debug.DrawLine(transform.position, transform.position + (directions[2] * maxDistance), Color.blue);
		Debug.DrawLine(transform.position, transform.position + (directions[3] * maxDistance), Color.cyan);
		Debug.DrawLine(transform.position, transform.position + (directions[4] * maxDistance), Color.yellow);
		Debug.DrawLine(transform.position, transform.position + (directions[5] * maxDistance), Color.magenta);

		if (isActive) {
			currInterval += Time.deltaTime;
			if (currInterval >= raycastInterval) {
				CastRays();
				currInterval = 0;
			}
		}
	}

	private void CastRays() {
		for (int i = 0; i < directions.Length; i++) {
			var currDir = directions[i];

			RaycastHit hit;
			if (Physics.Raycast(transform.position, currDir, out hit, maxDistance)) {
				if (hit.collider.tag == "Player")
					return;

				OnRayHitEvent.Invoke(hit);
				return;
			}
		}
	}
}
