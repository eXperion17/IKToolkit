using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKKK : MonoBehaviour {
	[SerializeField]
	private LookAt[] segments;

	[SerializeField]
	private Transform anchor;
	[SerializeField]
	private Transform target;

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//mousePosition.z = 0;


		//Setting the very end & the other segments to follow the target
		segments[segments.Length - 1].Follow(target);

		for (int i = segments.Length - 2; i >= 0 ; i--) {
			segments[i].Follow(segments[i + 1].transform);
		}

		//Setting them back to the anchor position
		segments[0].transform.position = anchor.position;
		for (int i = 1; i < segments.Length; i++) {
			segments[i].transform.position = segments[i - 1].GetEnd();
		}
	}
}
