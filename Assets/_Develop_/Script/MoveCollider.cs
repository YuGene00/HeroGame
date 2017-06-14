using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoveCollider : MonoBehaviour {

	//parent
	Character parent;

	//parent's CharacterMover
	CharacterMover characterMoverOfParent;

	//paren's rigidbody
	Rigidbody2D rigidOfParent;

	//count for foot contact
	int footContactCount = 0;

	void Awake() {
		parent = transform.parent.GetComponent<Character>();
		characterMoverOfParent = parent.GetComponent<CharacterMover>();
		rigidOfParent = parent.GetComponent<Rigidbody2D>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision) {
		++footContactCount;
		if (characterMoverOfParent.State == MoveState.JUMP && rigidOfParent.velocity.y <= 0f) {
			characterMoverOfParent.SetInAir(false);
			EffectManager.Instance.PlayEffect(new EffectData(EffectType.LAND, parent.Position));
			parent.AnimateByMoveState();
		}
	}

	protected virtual void OnCollisionExit2D(Collision2D collision) {
		--footContactCount;
		if (footContactCount <= 0) {
			characterMoverOfParent.SetInAir(true);
			parent.AnimateByMoveState();
		}
	}
}