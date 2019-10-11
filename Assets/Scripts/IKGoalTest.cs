using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKGoalTest : MonoBehaviour {

	public Animator animator;
	public Transform target;
	
	// Update is called once per frame
	void OnAnimatorIK () {
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
		animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
		animator.SetIKRotation(AvatarIKGoal.RightHand, target.rotation);
	}
}
