using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKKK : MonoBehaviour {
	[SerializeField]
	private LookAt[] segments;

	[SerializeField]
	private Transform anchor;

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;

		segments[segments.Length - 1].Follow(mousePosition, false);
		for (int i = segments.Length - 2; i >= 0 ; i--) {
			segments[i].Follow(segments[i + 1].transform.position, true);
		}
		/*
		segments[0].Follow(segments[segments.Length - 3].transform.position, true);
		segments[1].Follow(segments[segments.Length - 2].transform.position, true);
		segments[2].Follow(segments[segments.Length - 1].transform.position, true);*/

		segments[0].transform.position = anchor.position;

		for (int i = 1; i < segments.Length; i++) {
			segments[i].transform.position = segments[i - 1].GetEnd();
		}
	}
}
