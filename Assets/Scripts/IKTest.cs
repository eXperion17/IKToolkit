using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour {

	private Vector3 mousePosition;

	[SerializeField]
	private Transform segment;

	private SpriteRenderer segmentSprite;

	// Use this for initialization
	void Start () {
		segmentSprite = segment.GetComponent<SpriteRenderer>();

		Debug.Log(Mathf.Cos(0.69f));
	}
	
	// Update is called once per frame
	void Update () {
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		/*
		var pos = new Vector3(segment.position.x, segment.position.y - segmentSprite.bounds.size.y / 2, segment.position.z);
		var diff = segment.position - pos;

		segment.position = mousePosition + diff;*/

		var diff = mousePosition - segment.position;
		diff.z = 0;
		//Debug.Log(Mathf.Tan(diff.x / diff.y));	

		segment.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(segment.position, mousePosition, Vector3.forward));
		
	}
}
