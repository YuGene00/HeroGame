using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Stone : MonoBehaviour {

	//animation
	AnimationManager animationManager = new AnimationManager();

	void Awake() {
		animationManager.InitializeBy(GetComponent<Animator>());
	}

	public void Destroy() {
		animationManager.Animate(AnimationManager.AnimationType.DIE);
	}
}