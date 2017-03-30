using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover {

	//target Rigidbody
	Rigidbody target;

	//mover
	Mover mover;

	//enum for move direction
	public enum Direction {
		LEFT, RIGHT
	}

	//move state
	public enum MoveState {
		STAY, WALK, JUMP
	}
	MoveState state = MoveState.STAY;
	public MoveState State { get { return state; } }

	CharacterMover() { }

	public static CharacterMover CreateByTarget(Rigidbody target) {
		CharacterMover characterMover = new CharacterMover();
		characterMover.target = target;
		characterMover.mover = Mover.CreateByTarget(target.transform);
		return characterMover;
	}

	public void SetInAir(bool inAir) {
		if (inAir) {
			state = MoveState.JUMP;
		} else {
			state = MoveState.STAY;
		}
	}

	public void WalkTo(Direction direction) {
		if (state == MoveState.JUMP) {
			return;
		}

		state = MoveState.WALK;
		mover.MoveTo(ConvertToVector2(direction));
	}

	Vector2 ConvertToVector2(Direction direction) {
		Vector2 vector2 = Vector2.zero;
		switch (direction) {
			case Direction.LEFT:
				vector2 = Vector2.left;
				break;
			case Direction.RIGHT:
				vector2 = Vector2.right;
				break;
		}
		return vector2;
	}

	public void JumpTo(Direction direction) {
		Vector2 vector2 = Vector2.up;
		if (state == MoveState.WALK) {
			vector2 += ConvertToVector2(direction);
		}
		target.AddForce(vector2);
	}

	public void Stop() {
		state = MoveState.STAY;
	}
}