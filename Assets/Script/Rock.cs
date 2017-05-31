using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class Rock : MonoBehaviour {

	//animationController
	AnimationController animationController;

	void Awake() {
		animationController = GetComponent<AnimationController>();
	}

	public void Destroy() {
		animationController.Animate(AnimationType.DIE);
	}
}