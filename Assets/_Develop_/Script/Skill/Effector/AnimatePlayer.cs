using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayer : Effector {

	//animation type for play
	[SerializeField]
	AnimationType animationType = AnimationType.NONE;

	public override void RunEffect() {
		Player.Instance.Animate(animationType);
	}
}