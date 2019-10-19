using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootInfo {
	public Transform feetOrigin;
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
				currInfo.baseOffset = hit.point - currInfo.feetOrigin.position;
			}
		}
	}

	private void FixedUpdate() {
		
	}

	private void OnAnimatorIK(int layerIndex) {
		RaycastHit hit;
		for (int i = 0; i < _feetInfo.Length; i++) {
			var currInfo = _feetInfo[i];
			var footPos = Vector3.zero;
			if (Physics.Raycast(currInfo.feetOrigin.position + currInfo.baseOffset + currInfo.rayOffset, Vector3.down, out hit, 5f)) {
				footPos = currInfo.feetOrigin.position + hit.distance * Vector3.down + currInfo.rayOffset;

				//hardcoded for now
				if (i == 0) {
					//Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
					_animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
					_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

				} else {
					_animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
					_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
				}
			}
		}
		
		//var leftRay = Physics.Raycast(_leftFoot.feetOrigin.position + _leftFoot.baseOffset, Vector3.down, out hit, 5f);
		//var footPosL = _leftFoot.feetOrigin.position + hit.distance * Vector3.down;

		//_animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosL);
		//_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1); 

	}
}
