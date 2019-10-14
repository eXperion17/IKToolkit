using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoungs : MonoBehaviour {

	private void Update() {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

		if (meshFilter == null) {
			return;
		}

		Bounds bounds = meshFilter.mesh.bounds;

		Vector3[] vertices = new Vector3[8];

		vertices[0] = transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));
		vertices[1] = transform.TransformPoint(new Vector3(-bounds.max.x, bounds.max.y, bounds.max.z));
		vertices[2] = transform.TransformPoint(new Vector3(-bounds.max.x, bounds.max.y, -bounds.max.z));
		vertices[3] = transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, -bounds.max.z));
		vertices[4] = transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
		vertices[5] = transform.TransformPoint(new Vector3(-bounds.min.x, bounds.min.y, bounds.min.z));
		vertices[6] = transform.TransformPoint(new Vector3(-bounds.min.x, bounds.min.y, -bounds.min.z));
		vertices[7] = transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, -bounds.min.z));

		Debug.DrawLine(vertices[0], vertices[1], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[1], vertices[2], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[2], vertices[3], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[3], vertices[0], Color.red, 0.0F, false);

		Debug.DrawLine(vertices[4], vertices[5], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[5], vertices[6], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[6], vertices[7], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[7], vertices[4], Color.red, 0.0F, false);

		Debug.DrawLine(vertices[0], vertices[6], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[1], vertices[7], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[2], vertices[4], Color.red, 0.0F, false);
		Debug.DrawLine(vertices[3], vertices[5], Color.red, 0.0F, false);
	}
}
