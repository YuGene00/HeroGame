using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager {

	//target Animator
	Animator target;

	public enum AnimationType {
		STAY, WALK, JUMP, DIE
	}

	public void InitializeBy(Animator target) {
		this.target = target;
	}

	public void Animate(AnimationType animationType) {
		InitializeAnimationBool();
		switch (animationType) {
			case AnimationType.STAY:
				target.SetBool("Stay", true);
				break;
			case AnimationType.WALK:
				target.SetBool("Walk", true);
				break;
		}
	}

	void InitializeAnimationBool() {
		target.SetBool("Stay", false);
		target.SetBool("Walk", false);
	}
}