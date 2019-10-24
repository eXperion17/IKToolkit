using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandHover : MonoBehaviour {
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private List<RaycastChecker> raycastCheckers;

	private RaycastHit currHit;
	private Vector3 prevPosition;
	[SerializeField]
	private Transform anchorPoint;

	private float transition;

	[SerializeField]
	private AnimationCurve transitionCurve;

	// Use this for initialization
	void Awake () {
		raycastCheckers.ForEach(x => x.AddListener(OnRayHit));
		currHit = new RaycastHit();
	}

	private void LateUpdate() {
		prevPosition = anchorPoint.position;
	}

	public void OnRayHit(RaycastHit hit) {
		currHit = hit;
	}

	private void OnAnimatorIK(int layerIndex) {
		if (currHit.point.magnitude < 1) return;

		var delta = anchorPoint.position - prevPosition;
		var invertedHit = currHit.normal.normalized;
		invertedHit = new Vector3(Mathf.Abs(invertedHit.x - 1), Mathf.Abs(invertedHit.y - 1), Mathf.Abs(invertedHit.z - 1));
		delta.Scale(invertedHit);

		_animator.SetIKPosition(AvatarIKGoal.RightHand, currHit.point + delta);

		if (Vector3.Distance(raycastCheckers[0].transform.position, currHit.point + delta) > 0.75f) {
			if (transition > 0)
				transition -= Time.deltaTime*5;
			//Debug.Log(transitionCurve.Evaluate(transition));
			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, transitionCurve.Evaluate(transition));
		} else {
			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			transition = 1;
		}
	}
}
