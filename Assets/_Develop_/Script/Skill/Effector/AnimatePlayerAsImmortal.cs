using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayerAsImmortal : Effector {

	//animation type for play
	[SerializeField]
	AnimationType animationType = AnimationType.NONE;

	public override void RunEffect() {
		Player.Instance.AnimateAsImmortal(animationType);
	}
}