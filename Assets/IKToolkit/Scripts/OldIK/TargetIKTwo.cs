using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIKTwo : MonoBehaviour {

	[SerializeField]
	private Transform baseJoint;
	[SerializeField]
	private Transform endJoint;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private Transform baseAnchor;

	[SerializeField]
	private GameObject testPrefab;

	//Visible in inspector only for debugging
	[SerializeField]
	private List<JointProperties> joints;

	// Use this for initialization
	void Start() {
		joints = new List<JointProperties>();

		CreateJoints();
	}

	// Update is called once per frame
	void Update () {

		Follow(joints[0], target, true);
		for (int i = 1; i < joints.Count; i++) {
			Follow(joints[i], joints[i-1].dummy.transform);
		}


		joints[joints.Count - 1].dummy.transform.position = baseAnchor.position;

		for (int i = joints.Count - 2; i >= 0; i--) {
			joints[i].dummy.transform.position = joints[i + 1].dummy.transform.position + joints[i + 1].direction;
			Debug.DrawLine(joints[i].dummy.transform.position, joints[i + 1].dummy.transform.position + joints[i + 1].direction, Color.red);
		}

		joints.ForEach(x => x.ApplyLocation());
	}

	private void CreateJoints() {
		Transform currentJoint = endJoint;
		Transform previousJoint = null;


		//if anchor is the same as the base joint
		if (baseAnchor.Equals(baseJoint)) {
			var newAnchor = Instantiate(testPrefab, baseAnchor.parent);
			newAnchor.transform.position = baseAnchor.position;
			newAnchor.transform.rotation = baseAnchor.rotation;
			baseAnchor = newAnchor.transform;
		}

		int id = 0;
		//Needs a backup incase transform assignment results in an endless loop
		while (currentJoint != null) {
			var dummy = Instantiate(testPrefab, baseAnchor);
			dummy.transform.parent = baseAnchor;
			dummy.name = "dummy " + id++;
			if (id == 1)
					dummy.GetComponent<MeshRenderer>().material.color = Color.red;

			joints.Add(new JointProperties(dummy, currentJoint, GetJointLength(currentJoint, previousJoint)));

			previousJoint = currentJoint;
			currentJoint = currentJoint.parent;

			if (previousJoint == baseJoint)
				currentJoint = null;
		}

		

	}

	private Vector3 GetJointLength(Transform joint, Transform prev) {
		if (joint == endJoint)
			return Vector3.one * 0.02f;

		return prev.position - joint.position;
	}

	public void Follow(JointProperties joint, Transform obj, bool keepRotation = false) {
		//Rotation;
		Vector3 dir = obj.position - joint.dummy.transform.position;

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

		joint.direction = lengthDir;

		//joint.transform.position = joint.dummy.transform.position;
		//joint.transform.rotation = joint.dummy.transform.rotation;
	}
}
