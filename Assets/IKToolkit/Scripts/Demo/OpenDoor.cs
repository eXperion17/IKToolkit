using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

	public AnimationCurve openMotion;
	public bool isOpening;
	private float motion = 0;
	public float distance;
	private float startPos;
	
	// Update is called once per frame
	void Update () {
		if (isOpening && motion < 1) {
			transform.position = new Vector3(transform.position.x, startPos + (distance * openMotion.Evaluate(motion + Time.deltaTime)), transform.position.z);
			motion += Time.deltaTime;
		}
	}

	public void StartOpenDoor() {
		motion = 0;
		isOpening = true;
		startPos = transform.position.y;
	}
}
