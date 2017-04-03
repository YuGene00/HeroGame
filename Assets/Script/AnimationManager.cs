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

	}
}