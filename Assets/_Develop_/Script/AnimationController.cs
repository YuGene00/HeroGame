using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour {

	//Animator
	Animator animator;

	//current animation
	public AnimationType State { get; set; }
	public float Progress { get { return animator.GetCurrentAnimatorStateInfo(0).normalizedTime; } }

	//flag for locking control
	public bool IsLocked { get; set; }

	void Awake() {
		animator = GetComponent<Animator>();
		State = AnimationType.STAY;
		IsLocked = false;
	}

	public void Animate(AnimationType animationType) {
		if (IsLocked) {
			return;
		}

		switch (animationType) {
			case AnimationType.NONE:
				ResetAnimationBool();
				break;
			default:
				SetBool(animationType.ToString(), true);
				break;
		}
		State = animationType;
	}

	void SetBool(string name, bool value) {
		ResetAnimationBool();
		animator.SetBool(name, value);
	}

	void ResetAnimationBool() {
		AnimatorControllerParameter[] parameters = animator.parameters;
		for (int i = 0; i < parameters.Length; ++i) {
			animator.SetBool(parameters[i].name, false);
		}
	}
}