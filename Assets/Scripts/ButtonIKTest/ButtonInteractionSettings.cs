using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonInteractionSettings", menuName = "IK Toolkit/ButtonInteractionSettings")]
public class ButtonInteractionSettings : ScriptableObject {
	public AnimationCurve IKTransition;
	[Range(0.1f, 5)]
	public float IKTransitionTime;
	public bool disableInput;
}
