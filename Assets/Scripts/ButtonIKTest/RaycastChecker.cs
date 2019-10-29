using System;
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
	private float slowedInterval = 1f;
	[SerializeField]
	private float maxDistance = 1f;

	[SerializeField]
	private Transform directionHelper;

	private Vector3[] directionsBase;
	private Vector3[] directions;
	private float currInterval;
	public UnityEventRay OnRayHitEvent;
	private bool isActive = true;
	private bool isSlowed;
	private int currentRay;

	[SerializeField]
	private TMPro.TextMeshProUGUI textt;

	// Use this for initialization
	void Start () {
		currInterval = 0;

		directions = new Vector3[1];
		directions[0] = Vector3.forward;

		//directions[1] = Vector3.right;
		//directions[1] = (Vector3.forward + Vector3.right * 0.8f).normalized;
		//directions[2] = (Vector3.forward + Vector3.right * 2f).normalized;
		//directions[3] = (Vector3.forward + Vector3.left * 0.8f).normalized;
		//directions[4] = (Vector3.forward + Vector3.left * 2f).normalized;

		directionsBase = directions;
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

	public void ToggleSlowed(bool value) {
		isSlowed = value;
	}


	// Update is called once per frame
	void Update () {
		//Debug lines
		Debug.DrawLine(transform.position, transform.position + (directionHelper.TransformDirection(directions[0]) * maxDistance), Color.red);
		//Debug.DrawLine(transform.position, transform.position + directions[0] * maxDistance, Color.red);
		//Debug.DrawLine(transform.position, transform.position + (directionHelper.TransformDirection(directions[1]) * maxDistance), Color.green);
		//Debug.DrawLine(transform.position, transform.position + (directionHelper.TransformDirection(directions[2]) * maxDistance), Color.blue);
		//Debug.DrawLine(transform.position, transform.position + (directionHelper.TransformDirection(directions[3]) * maxDistance), Color.cyan);
		//Debug.DrawLine(transform.position, transform.position + (directionHelper.TransformDirection(directions[4]) * maxDistance), Color.yellow);
		//Debug.DrawLine(transform.position, transform.position + (directionHelper.TransformDirection(directions[5]) * maxDistance), Color.magenta);

		if (isActive) {
			currInterval += Time.deltaTime;
			var interval = isSlowed ? slowedInterval : raycastInterval;
			if (currInterval >= interval) {
				CastRays();
				currInterval = 0;
			}
		}
	}

	public void AssignTarget(int index, Vector3 point) {
		directions[0] = point;
	}

	private void CastRays() {
		for (int i = 0; i < directions.Length; i++) {
			if (currentRay != -1 && currentRay != i)
				continue;

			var currDir = directions[i];

			RaycastHit hit;
			Vector3 dir;
			if (directionHelper)
				dir = directionHelper.TransformDirection(currDir);
			else
				dir = transform.TransformDirection(currDir);

			var distance = isSlowed ? 15 : maxDistance;
			if (Physics.Raycast(transform.position, dir, out hit, distance)) {
				if (hit.collider.tag == "Player")
					return;
				currentRay = i;
				if (textt != null)
					textt.text = currentRay.ToString();
				OnRayHitEvent.Invoke(hit);
				return;
			}
		}
	}

	public void Reset() {
		currentRay = -1;
	}
}
