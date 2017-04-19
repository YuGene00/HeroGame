using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoveCollider : MonoBehaviour {

	//parent
	Character parent;

	void Awake() {
		parent = transform.parent.GetComponent<Character>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision) {
		if (parent.CharacterMover.State == CharacterMover.MoveState.JUMP && hasFootContact(collision)) {
			parent.CharacterMover.SetInAir(false);
			parent.AnimateByMoveState();
		}
	}

	protected virtual void OnCollisionExit2D(Collision2D collision) {
		if (parent.CharacterMover.State != CharacterMover.MoveState.JUMP && !hasFootContact(collision)) {
			parent.CharacterMover.SetInAir(true);
			parent.AnimateByMoveState();
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