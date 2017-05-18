using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationManager))]
public class Stone : MonoBehaviour {

	//animation
	AnimationManager animationManager;

	void Awake() {
		animationManager = GetComponent<AnimationManager>();
	}

	public void Destroy() {
		animationManager.Animate(AnimationType.DIE);
	}
}