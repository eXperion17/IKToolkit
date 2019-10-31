using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointProperties {
	public GameObject dummy;
	public Transform transform;
	public Vector3 length;
	public Vector3 direction;

	public JointProperties(GameObject dummy, Transform obj, Vector3 length) {
		this.dummy = dummy;
		this.transform = obj;
		this.length = length;
		direction = Vector3.zero;
	}

	public void ApplyLocation() {
		transform.position = dummy.transform.position;
		transform.rotation = dummy.transform.rotation;
	}

	public Vector3 GetEnd() {
		throw new NotImplementedException();
	}
}

public class TargetIK : MonoBehaviour {

	[SerializeField]
	private Transform baseJoint;
	[SerializeField]
	private Transform endJoint;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private Transform baseAnchor;

	//Visible in inspector only for debugging
	[SerializeField]
	private List<JointProperties> joints;

	public bool apply;

	// Use this for initialization
	void Start () {
		joints = new List<JointProperties>();

		CreateJoints();
	}

	void Update() {
		//Follow
		for (int i = 0; i < joints.Count; i++) {
			JointProperties currJoint = joints[i];
			if (i == 0) {
				Follow(currJoint, target);
			} else {
				Follow(currJoint, joints[i - 1].transform);
			}
		}
		
		joints[1].dummy.transform.position = baseAnchor.transform.position;

		if (apply) {
			joints.ForEach(x => x.ApplyLocation());
		}

		
		/*
		//Setting them back to the anchor position
		joints[0].transform.position = baseAnchor.position;
		for (int i = 1; i < joints.Count; i++) {
			joints[i].transform.position = joints[i - 1].transform.position + joints[i-1].length;
		}*/
	}

	private void CreateJoints() {
		Transform currentJoint = endJoint;
		Transform previousJoint = null;

		baseAnchor = new GameObject("d").transform;
		baseAnchor.position = baseJoint.position;
		int id = 0;
		//Needs a backup incase transform assignment results in an endless loop
		while (currentJoint != null) {
			var dummy = new GameObject("Dummy Joint " + baseAnchor.name + " " + id++);
			dummy.transform.parent = baseAnchor;

			joints.Add(new JointProperties(dummy, currentJoint, GetJointLength(currentJoint, previousJoint)));

			previousJoint = currentJoint;
			currentJoint = currentJoint.parent;

			if (previousJoint == baseJoint)
				currentJoint = null;
		}
		
	}

	private Vector3 GetJointLength(Transform joint, Transform prev) {
		if (joint == endJoint)
			return Vector3.one * 0.001f;

		return prev.position - joint.position;
	}

	public void Follow(JointProperties joint, Transform obj, bool keepRotation = false) {
		//Rotation;
		Vector3 dir = obj.position - joint.transform.position;

		if (!keepRotation) {
			joint.dummy.transform.LookAt(obj);
			joint.dummy.transform.Rotate(0, 270, 0);
		}

		float length = joint.length.magnitude;

		Vector3 lengthDir = dir.normalized * length;
		if (!keepRotation)
			joint.dummy.transform.position = obj.position + (lengthDir * -1);
		else {
			joint.dummy.transform.position = obj.position;
			joint.dummy.transform.rotation = obj.rotation;
		}

		//joint.transform.position = joint.dummy.transform.position;
		//joint.transform.rotation = joint.dummy.transform.rotation;
	}
}
