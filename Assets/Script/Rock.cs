using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class Rock : MonoBehaviour {

	//animationController
	AnimationController animationController;

	//wait for die
	WaitUntil dieWait;

	void Awake() {
		animationController = GetComponent<AnimationController>();
		dieWait = new WaitUntil(() => animationController.Progress >= 1f);
	}

	public void Destroy() {
		animationController.Animate(AnimationType.DIE);
	}
}