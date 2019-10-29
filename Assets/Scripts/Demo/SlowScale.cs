using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScale : MonoBehaviour {
	public float minScale;
	public float maxScale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var scale = Mathf.Lerp(minScale, maxScale, 0.6f + (Mathf.Sin(Time.time) * 0.6f));

		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scale);
	}
}
