using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetDirectionTest : MonoBehaviour {

	[SerializeField]
	public Transform helperObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
			Debug.DrawLine(transform.position, hit.point, Color.blue);
			Debug.DrawLine(hit.point, hit.point+hit.normal, Color.cyan);
			helperObj.position = hit.point;
			//helperObj.LookAt(hit.point + hit.normal);
			//helperObj.LookAt()
			//Quaternion angleAxis = Quaternion.AngleAxis(Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg, Vector3.forward);
			helperObj.rotation = Quaternion.LookRotation(hit.normal);
			//helperObj.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);

		}
	}
}
