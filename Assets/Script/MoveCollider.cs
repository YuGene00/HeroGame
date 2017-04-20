using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoveCollider : MonoBehaviour {

	//parent
	Character parent;

	//move collider
	Collider2D moveCollider;

	//count for foot contact
	int footContactCount = 0;

	void Awake() {
		parent = transform.parent.GetComponent<Character>();
		moveCollider = GetComponent<Collider2D>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision) {
		++footContactCount;
		if (parent.CharacterMover.State == CharacterMover.MoveState.JUMP) {
			parent.CharacterMover.SetInAir(false);
			parent.AnimateByMoveState();
		}
	}

	protected virtual void OnCollisionExit2D(Collision2D collision) {
		--footContactCount;
		if (footContactCount <= 0) {
			parent.CharacterMover.SetInAir(true);
			parent.AnimateByMoveState();
		}
	}
}