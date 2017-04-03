using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(Animator))]
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