using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class MoveCollider : MonoBehaviour {

	//parent's CharacterMover
	public CharacterMover characterMover;

	//parent's AnimationManager
	public AnimationManager animationManager;

	void OnCollisionEnter2D(Collision2D collision) {
		if (characterMover.State == CharacterMover.MoveState.JUMP) {
			characterMover.SetInAir(!hasFootContact(collision));
			animationManager.Animate(AnimationManager.AnimationType.STAY);
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (characterMover.State != CharacterMover.MoveState.JUMP) {
			characterMover.SetInAir(!hasFootContact(collision));
			animationManager.Animate(AnimationManager.AnimationType.JUMP);
		}
	}

	bool hasFootContact(Collision2D collision) {
		for (int i = 0; i < collision.contacts.Length; ++i) {
			if (collision.contacts[i].normal == Vector2.up) {
				return true;
			}
		}
		return false;
	}
}