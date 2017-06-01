﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour {

	//Animator
	Animator animator;
	AnimatorStateInfo animatorStateInfo;

	//current animation
	public AnimationType State { get; set; }
	public float Progress { get { return animatorStateInfo.normalizedTime; } }

	void Awake() {
		animator = GetComponent<Animator>();
		animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		State = AnimationType.STAY;
	}

	public void Animate(AnimationType animationType) {
		switch (animationType) {
			case AnimationType.STAY:
				SetBool("Stay", true);
				break;
			case AnimationType.WALK:
				SetBool("Walk", true);
				break;
			case AnimationType.JUMP:
				SetBool("Jump", true);
				break;
			case AnimationType.DAMAGED:
				SetBool("Damaged", true);
				break;
			case AnimationType.DIE:
				SetBool("Die", true);
				break;
		}
		State = animationType;
	}

	void SetBool(string name, bool value) {
		ResetAnimationBool();
		animator.SetBool(name, value);
	}

	void ResetAnimationBool() {
		animator.SetBool("Stay", false);
		animator.SetBool("Walk", false);
		animator.SetBool("Jump", false);
		animator.SetBool("Damaged", false);
		animator.SetBool("Die", false);
	}
}