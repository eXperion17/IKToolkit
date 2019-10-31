﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootInfo {
	public Transform feetOrigin;
	[HideInInspector]
	public Vector3 baseOffset;
	public Vector3 rayOffset = Vector3.up;
}

public class IKFoot : MonoBehaviour {
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private FootInfo _rightFoot;
	[SerializeField]
	private FootInfo _leftFoot;

	[SerializeField]
	private AnimationClip[] applyIKList;

	private FootInfo[] _feetInfo;

	private void Awake() {
		_feetInfo = new FootInfo[2];
		_feetInfo[0] = _rightFoot;
		_feetInfo[1] = _leftFoot;

		//for loop
		RaycastHit hit;
		for (int i = 0; i < _feetInfo.Length; i++) {
			var currInfo = _feetInfo[i];
			if (Physics.Raycast(currInfo.feetOrigin.position, Vector3.down, out hit, 5f)) {
				//Before we do anything, we'll have to know the offset to the ground.
				//This is used to determine how far off the feet have to be (since the pivot usually isn't on the ground)
				currInfo.baseOffset = hit.point - currInfo.feetOrigin.position;
			}
		}
	}

	public bool RequiresFootIK(AnimationClip clip) {
		var contains = false;

		foreach (var otherClip in applyIKList) {
			if (otherClip.Equals(clip)) {
				contains = true;
			}
		}

		return contains;
	}

	private void OnAnimatorIK(int layerIndex) {
		//With an include-type list, we check if the most prominent clip is in the list, if so, apply the IK.
		//Unfortunately this doesn't yet work with walking animations and such.
		if ((_animator.GetCurrentAnimatorClipInfoCount(0) > 0) && !RequiresFootIK(_animator.GetCurrentAnimatorClipInfo(0)[0].clip))
			return;
		
		RaycastHit hit;
		for (int i = 0; i < _feetInfo.Length; i++) {
			var currInfo = _feetInfo[i];
			var footPos = Vector3.zero;
			if (Physics.Raycast(currInfo.feetOrigin.position + currInfo.baseOffset + currInfo.rayOffset, Vector3.down, out hit, 5f)) {
				footPos = currInfo.feetOrigin.position + hit.distance * Vector3.down + currInfo.rayOffset;

				//hardcoded for now as the IK systems only work with two feet anyway
				if (i == 0) {
					_animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
					_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
					
					/*
					//Getting our left vector of the avatar/character
					var leftVector = Quaternion.AngleAxis(90, transform.up) * transform.forward;
					//Rotating the hit.normal so that it always faces forwards of the avatar.
					var newForward = Quaternion.AngleAxis(90, leftVector) * hit.normal;
					Debug.DrawLine(hit.point, hit.point + newForward, Color.green);

					//Interpolates the newForward (tilted up/down) with a simple LookRotation with the forward (for tilted sideways).
					//Both shine in their particular area, so ideally 0.5f would be replaced by variable that changes depending on the angle.
					//var interpolatedRotation =	Quaternion.Slerp(	Quaternion.LookRotation(newForward, hit.normal),
					//							Quaternion.LookRotation(transform.forward, hit.normal), 0.5f);
					*/

					//Upon some research, this line does exactly the same as the commented code above in just one method. Source: https://github.com/arkms/IKFoot_Floor-Unity
					//In retrospect I could've done some more research before trying to tackle it on my own, however I have learned a decent amount doing it like this 
					Quaternion interpolatedRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);

					_animator.SetIKRotation(AvatarIKGoal.RightFoot, interpolatedRotation);
					_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

				} else {
					_animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
					_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

					var interpolatedRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
					_animator.SetIKRotation(AvatarIKGoal.LeftFoot, interpolatedRotation);
					
					_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
				}
			}
		}

	}
}