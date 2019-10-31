using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleScript : MonoBehaviour {
	public KeyCode keyCode;
	public List<IKFoot> scripts;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(keyCode)) {
			scripts.ForEach(x => x.enabled = !x.enabled);
		}
	}
}
