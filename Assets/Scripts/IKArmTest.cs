using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKArmTest : MonoBehaviour {

	private Vector3 mousePosition;

	[SerializeField]
	private IKJoint[] segments;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;


		segments[0].transform.position = mousePosition;
		IKJoint piece = segments[0];

		piece.MoveTo(mousePosition);

		/*
		while (piece.transform.parent != null) {


			piece = piece.transform.parent.GetComponent<IKJoint>();
		}*/

		//segments[0].LookAt(mousePosition);
	}
}
