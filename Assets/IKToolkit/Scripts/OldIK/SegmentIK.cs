using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentIK : MonoBehaviour {
	[SerializeField]
	private List<LookAt> segments;
	[SerializeField]
	private GameObject segmentPrefab;

	[SerializeField]
	private Transform anchor;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private bool keepRotation;

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		
	}

	public void CreateSegment() {
		var copy = Instantiate(segmentPrefab);
		var IK = copy.GetComponent<LookAt>();
		segments.Add(IK);
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			CreateSegment();
			target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2f, target.transform.position.z);
		}

		//mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//mousePosition.z = 0;


		//Setting the very end & the other segments to follow the target
		segments[segments.Count - 1].Follow(target, keepRotation);

		for (int i = segments.Count - 2; i >= 0 ; i--) {
			segments[i].Follow(segments[i + 1].transform);
		}

		//Setting them back to the anchor position
		segments[0].transform.position = anchor.position;
		for (int i = 1; i < segments.Count; i++) {
			segments[i].transform.position = segments[i - 1].GetEnd();
		}
	}
}
