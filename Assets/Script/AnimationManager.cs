using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour {

	//Animator
	Animator animator;

	void Awake() {
		animator = GetComponent<Animator>();
	}

	public void Animate(AnimationType animationType) {
		ResetAnimationBool();
		switch (animationType) {
			case AnimationType.STAY:
				animator.SetBool("Stay", true);
				break;
			case AnimationType.WALK:
				animator.SetBool("Walk", true);
				break;
			case AnimationType.JUMP:
				//animator.SetBool("Jump", true);
				break;
			case AnimationType.DAMAGED:
				//animator.SetBool("Damaged", true);
				break;
			case AnimationType.DIE:
				//animator.SetBool("Die", true);
				break;
		}
	}

	void ResetAnimationBool() {
		animator.SetBool("Stay", false);
		animator.SetBool("Walk", false);
		//animator.SetBool("Jump", false);
		//animator.SetBool("Damaged", false);
		//animator.SetBool("Die", false);
	}
}