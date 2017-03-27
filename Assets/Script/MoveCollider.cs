using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class MoveCollider : MonoBehaviour {

	//parent's CharacterMover
	public CharacterMover characterMover;

	//parent's AnimationMgr
	public AnimationMgr AnimationMgr;

	void OnCollisionEnter(Collision other) {

	}

	void OnCollisionExit(Collision other) {

	}
}