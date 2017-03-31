using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager {

	//target Animator
	Animator target;

	public enum AnimationType {
		STAY, WALK, JUMP, DIE
	}

	AnimationManager() { }

	public static AnimationManager CreateByTarger(Animator target) {
		AnimationManager animationManager = new AnimationManager();
		animationManager.target = target;
		return animationManager;
	}

	public void Animate(AnimationType animationType) {

	}
}