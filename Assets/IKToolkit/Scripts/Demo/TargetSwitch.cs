using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSwitch : MonoBehaviour {

	[SerializeField]
	private vThirdPersonCamera cam;

	private bool zoomedIn = false;


	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Z)) {
			if (!zoomedIn) {
				cam.height = Mathf.Lerp(1.4f, 0.3f, 1);
				cam.defaultDistance = Mathf.Lerp(2.5f, 1, 1);
			}
			else {
				cam.height = Mathf.Lerp(1.4f, 0.3f, 0);
				cam.defaultDistance = Mathf.Lerp(2.5f, 1, 0);
			}
			zoomedIn = !zoomedIn;
		}
		else if (Input.GetKeyDown(KeyCode.X)) {
			if (!zoomedIn) {
				//cam.height = Mathf.Lerp(1.4f, 0.3f, 1);
				cam.defaultDistance = Mathf.Lerp(2.5f, 1, 1);
			}
			else {
				//cam.height = Mathf.Lerp(1.4f, 0.3f, 0);
				cam.defaultDistance = Mathf.Lerp(2.5f, 1, 0);
			}
		}
	}
}
