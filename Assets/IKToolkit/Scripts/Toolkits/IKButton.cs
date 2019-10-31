using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ButtonPressPhase {
	Idle,
	MovingTo,
	Pressing,
	MovingBack
}

public class IKButton : MonoBehaviour {
	[SerializeField]
	public GameObject pressTarget;
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	public ButtonPressPhase currentPhase;
	[SerializeField]
	private float interactionRadius;

	[SerializeField]
	private ButtonInteractionSettings settings;

	public UnityEvent onActionStartEvent;
	public UnityEvent onButtonPressEvent;
	public UnityEvent onActionEndEvent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		switch (currentPhase) {
			case ButtonPressPhase.Idle:
				if (PlayerInRange() && Input.GetKeyDown(KeyCode.E)) {
					PressButton();
				}
				break;
		}
		
	}

	private void PressButton() {
		currentPhase = ButtonPressPhase.MovingTo;

		if (_animator.gameObject.GetComponent<ButtonPressAction>() == null) {
			var action = _animator.gameObject.AddComponent<ButtonPressAction>();
			action.Initialize(_animator, pressTarget.transform, settings);

			onActionStart();
			Invoke("OnButtonPress", settings.IKTransitionTime);
			Invoke("OnActionEnd", settings.IKTransitionTime*2);
		}
	}
	private void onActionStart() {
		onActionStartEvent.Invoke();
	}

	private void OnButtonPress() {
		onButtonPressEvent.Invoke();
		//pressTarget.GetComponent<MeshRenderer>().enabled = false;
	}

	private void OnActionEnd() {
		currentPhase = ButtonPressPhase.Idle;
		onActionEndEvent.Invoke();

		if (settings.disableInput) {
			//renable it again
		}
	}

	private bool PlayerInRange() {
		return Vector3.Distance(_animator.gameObject.transform.position, pressTarget.transform.position) < interactionRadius;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(pressTarget.transform.position, interactionRadius);
	}
}
