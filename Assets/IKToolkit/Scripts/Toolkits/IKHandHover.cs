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
	[SerializeField]
	private Transform handTransform;
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
		if (attached) {
			if (hit.normal == currHit.normal && Vector3.Distance(hit.point, currHit.point) < 0.4f)
				return;
			else {
				attached = false;
				return;
			}
		}
			

		currHit = hit;
		attached = true;
		//Applying hand offset to prevent them from clipping
		currHit.point += (currHit.normal.normalized * handOffset);

		//Some offset on placement of the hand, making them more spread out or closer to eachother for more natural leaning
		var placementOffset = (Quaternion.AngleAxis(90, transform.up) * -currHit.normal.normalized) * placementDistance;
		if (IKGoal == AvatarIKGoal.LeftHand)
			placementOffset = -placementOffset;

		currHit.point += placementOffset;

		//Slow down the raycasts, we already made contact with a wall so we don't have to check as often
		raycastCheckers[0].ToggleSlowed(true);
	}

	private void OnAnimatorIK(int layerIndex) {
		//RaycastHit can't be checked if it's null, so this is a different option of doing so
		if (currHit.point.magnitude < 1) return;
		
		//To make the hands move with the character, we'll use the delta position of the character
		var delta = anchorPoint.position - prevPosition;
		var invertedHit = currHit.normal.normalized;
		bool oneAxis = (currHit.normal.magnitude == 1);
		//We take the hit.normal, turn it into a multiplier by subtracting 1 from the absolute value and multiply the delta with that value
		//This works perfectly for non-angled walls, as it perfectly correlates with the axis
		invertedHit = new Vector3(Mathf.Abs(Mathf.Abs(invertedHit.x) - 1), Mathf.Abs(Mathf.Abs(invertedHit.y) - 1), Mathf.Abs(Mathf.Abs(invertedHit.z) - 1));
		
		if (oneAxis) {
			delta.Scale(invertedHit);
			currHit.point += delta;
		} else {
			//This is a temporary solution, making extra raycasts if the walls are diagonal or curved
			//Ideally this would happen without constant raycasts, but unfortunately I couldn't find a solution
			RaycastHit hitTwo;
			if (Physics.Raycast(leanPoint.position, transform.forward, out hitTwo, 1f)) {
				currHit = hitTwo;
			}
			//This is basically a repeat of OnRayHit and declaring it as the currHit that the rest of the method uses
			currHit.point += (currHit.normal.normalized * handOffset);
			var placementOffset = (Quaternion.AngleAxis(90, transform.up) * -currHit.normal.normalized) * placementDistance;
			if (IKGoal == AvatarIKGoal.LeftHand)
				placementOffset = -placementOffset;
			currHit.point += placementOffset;
		}

		//Using the animator and Unity's humanoid IK system to define the positions
		_animator.SetIKPosition(IKGoal, Vector3.Lerp(handTransform.position, currHit.point, 1));
		_animator.SetIKHintPosition(IKHint, currHit.point + Vector3.down);

		//The AnimationCurve prevents jarring jumps, transition is added, subtracted and reset in below
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
				raycastCheckers[0].ToggleSlowed(false);
				raycastCheckers[0].Reset();
			}
			
		} else {
			if (transition > 0)
				transition -= Time.deltaTime * IKSpeed;

			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, transitionCurve.Evaluate(transition));
		}
	}
}
