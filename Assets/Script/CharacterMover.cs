using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover {

	//enum for move direction
	public enum Direction {
		LEFT, RIGHT
	}

	//move state
	public enum MoveState {
		STAY, WALK, JUMP
	}
	MoveState state;
	public MoveState State { get { return state; } }

	CharacterMover() {

	}

	public static CharacterMover CreateByTarget(Rigidbody target) {
		return null;
	}

	public void SetInAir(bool inAir) {

	}

	public void WalkTo(Direction direction) {

	}

	public void JumpTo(Direction direction) {

	}

	public void Stop() {

	}
}