using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressAction : MonoBehaviour {
	private Animator _animator;
	private Transform _target;
	
	[SerializeField]
	private float motion;
	[SerializeField]
	public ButtonPressPhase currentPhase;

	private ButtonInteractionSettings _settings;

	public void Initialize(Animator ani, Transform target, ButtonInteractionSettings settings) {
		_animator = ani;
		_target = target;
		_settings = settings;
		
		motion = 0;
		currentPhase = ButtonPressPhase.MovingTo;
	}

	public void OnAnimatorIK(int layerIndex) {
		_animator.SetIKPosition(AvatarIKGoal.RightHand, _target.position);
		var ratio = _settings.IKTransition.keys[_settings.IKTransition.length - 1].time / _settings.IKTransitionTime;
		_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _settings.IKTransition.Evaluate(motion*ratio));
	}

	public void Update() {
		if (currentPhase == ButtonPressPhase.MovingTo) {
			motion += Time.deltaTime;
			if (motion > _settings.IKTransitionTime)
				currentPhase = ButtonPressPhase.MovingBack;
		} else if (currentPhase == ButtonPressPhase.MovingBack) {
			motion -= Time.deltaTime;
			if (motion < 0)
				Destroy(this);
			//currentPhase = ButtonPressPhase.Idle;
		}	
	}
}
