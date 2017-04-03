using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterMover {

	//target Rigidbody
	Rigidbody2D target;

	//mover
	[SerializeField]
	Mover mover = new Mover();
	public Mover Mover { get { return mover; } }

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

	//jump power
	[SerializeField]
	float baseJumpPower;
	Formula<float> jumpPower = new Formula<float>();
	public Formula<float> JumpPower { get { return jumpPower; } }

	public void InitializeBy(Rigidbody2D target) {
		InitializeStat();
		jumpPower.SetBaseValue(baseJumpPower);
		this.target = target;
		mover.InitializeBy(this.target.transform);
	}

	public void InitializeStat() {
		jumpPower.Clear();
		mover.InitializeStat();
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
		target.AddForce(vector2 * jumpPower.Value);
	}

	public void Stop() {
		state = MoveState.STAY;
	}
}