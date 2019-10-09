using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointProperties {
	public Transform transform;
	public Vector3 length;
	public JointProperties(Transform obj, Vector3 length) {
		this.transform = obj;
		this.length = length;
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


	// Use this for initialization
	void Start () {
		joints = new List<JointProperties>();

		CreateJoints();
	}

	void Update() {
		//Follow
		/*
		for (int i = 0; i < joints.Count; i++) {
			JointProperties currJoint = joints[i];
			if (i == 0) {
				Follow(currJoint, target);
			} else {
				Follow(currJoint, joints[i - 1].transform);
			}
		}*/

		Follow(joints[1], joints[0].transform);
		Follow(joints[0], target);

		joints[1].transform.position = baseAnchor.transform.position;

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
		//Needs a backup incase transform assignment results in an endless loop
		while (currentJoint != null) {
			joints.Add(new JointProperties(currentJoint, GetJointLength(currentJoint, previousJoint)));

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
			joint.transform.LookAt(obj);
			joint.transform.Rotate(0, 270, 0);
		}

		float length = joint.length.magnitude;

		Vector3 lengthDir = dir.normalized * length;
		if (!keepRotation)
			joint.transform.position = obj.position + (lengthDir * -1);
		else {
			joint.transform.position = obj.position;
			joint.transform.rotation = obj.rotation;
		}
	}
}
