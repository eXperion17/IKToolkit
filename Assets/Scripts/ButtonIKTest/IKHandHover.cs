using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandHover : MonoBehaviour {
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private List<RaycastChecker> raycastCheckers;

	[SerializeField]
	private AvatarIKGoal IKGoal;
	private AvatarIKHint IKHint;

	private RaycastHit currHit;
	private Vector3 prevPosition;
	[SerializeField]
	private Transform anchorPoint;
	[SerializeField]
	private Transform leanPoint;
	[SerializeField, Range(0, 2)]
	private float maxReachRadius;
	[SerializeField, Range(1, 5)]
	private float IKSpeed;

	[SerializeField, Range(0, 0.2f)]
	private float handOffset;
	[SerializeField, Range(0, 1)]
	private float placementDistance;

	private float transition;

	private bool attached;

	[SerializeField]
	private AnimationCurve transitionCurve;

	// Use this for initialization
	void Awake () {
		raycastCheckers.ForEach(x => x.AddListener(OnRayHit));
		currHit = new RaycastHit();
		attached = false;

		if (IKGoal == AvatarIKGoal.LeftHand)
			IKHint = AvatarIKHint.LeftElbow;
		else
			IKHint = AvatarIKHint.RightElbow;
	}

	private void LateUpdate() {
		prevPosition = anchorPoint.position;
	}

	public void OnRayHit(RaycastHit hit) {
		currHit = hit;
		attached = true;
		//Applying hand offset to prevent them from clipping
		currHit.point += (currHit.normal.normalized * handOffset);

		var placementOffset = (Quaternion.AngleAxis(90, transform.up) * -currHit.normal.normalized) * placementDistance;
		if (IKGoal == AvatarIKGoal.LeftHand)
			placementOffset = -placementOffset;
		currHit.point += placementOffset;

		raycastCheckers[0].Toggle(false);
	}

	private void OnAnimatorIK(int layerIndex) {
		if (currHit.point.magnitude < 1) return;

		//we always need to define the IK goal
		var delta = anchorPoint.position - prevPosition;
		var invertedHit = currHit.normal.normalized;
		Debug.Log(invertedHit);
		invertedHit = new Vector3(Mathf.Abs(invertedHit.x - 1), Mathf.Abs(invertedHit.y - 1), Mathf.Abs(invertedHit.z - 1));
		delta.Scale(invertedHit);

		currHit.point += delta;


		_animator.SetIKPosition(IKGoal, currHit.point);
		_animator.SetIKHintPosition(IKHint, currHit.point + Vector3.down);
		_animator.SetIKHintPositionWeight(IKHint, transitionCurve.Evaluate(transition));
		_animator.SetIKPositionWeight(IKGoal, transitionCurve.Evaluate(transition));

		//Getting our left vector of the avatar/character
		var leftVector = Quaternion.AngleAxis(90, transform.up) * -currHit.normal.normalized;
		//Rotating the hit.normal so that it always faces forwards of the avatar.
		var newForward = Quaternion.AngleAxis(90, leftVector) * currHit.normal;

		_animator.SetIKRotation(IKGoal, Quaternion.LookRotation(newForward, currHit.normal));
		_animator.SetIKRotationWeight(IKGoal, transitionCurve.Evaluate(transition));

		if (attached) {
			if (transition < 1)
				transition += Time.deltaTime * IKSpeed;

			if (Vector3.Distance(leanPoint.position, currHit.point) > maxReachRadius) {
				attached = false;
				raycastCheckers[0].Toggle(true);
			}
		} else {
			if (transition > 0)
				transition -= Time.deltaTime * IKSpeed;

			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, transitionCurve.Evaluate(transition));
		}
	}
}
