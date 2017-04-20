using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterMover {

	//target Rigidbody2D
	Rigidbody2D target;

	//mover
	[SerializeField]
	Mover mover = new Mover();
	public Mover Mover { get { return mover; } }

	//enum for move direction
	public enum Direction {
		NONE, LEFT, RIGHT
	}

	//move state
	public enum MoveState {
		STAY, WALK, JUMP
	}
	MoveState state = MoveState.JUMP;
	public MoveState State { get { return state; } }

	//jump power
	[SerializeField]
	float baseJumpPower;
	Formula<float> jumpPower = new Formula<float>();
	public Formula<float> JumpPower { get { return jumpPower; } }

	//variable for calculating jump vector
	const float JUMPDEGREE = 80f;
	public const float JUMPDEGREERADIAN = JUMPDEGREE * 3.141592653589f / 180f;
	float cos;
	float sin;

	public void InitializeBy(Rigidbody2D target) {
		InitializeStat();
		jumpPower.SetBaseValue(baseJumpPower);
		InitializeConstantForJumpVector();
		this.target = target;
		mover.InitializeBy(this.target.transform);
	}

	public void InitializeStat() {
		jumpPower.Clear();
		mover.InitializeStat();
	}

	void InitializeConstantForJumpVector() {
		cos = Mathf.Cos(JUMPDEGREERADIAN);
		sin = Mathf.Sin(JUMPDEGREERADIAN);
	}

	public void SetInAir(bool inAir) {
		if (inAir) {
			state = MoveState.JUMP;
		} else {
			target.velocity = Vector2.zero;
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
		switch (direction) {
			case Direction.LEFT:
				return Vector2.left;
			default:
				return Vector2.right;
		}
	}

	public void JumpTo(Direction direction) {
		if (state == MoveState.JUMP) {
			return;
		}
		target.velocity = CalculateJumpVector(direction);
	}

	Vector2 CalculateJumpVector(Direction direction) {
		Vector2 jumpVector;
		if (state == MoveState.STAY) {
			jumpVector = Vector2.up;
		} else {
			if (direction == Direction.LEFT) {
				jumpVector = new Vector2(-cos, sin);
			} else {
				jumpVector = new Vector2(cos, sin);
			}
		}
		return jumpVector * jumpPower.Value;
	}

	public void Stop() {
		if (state == MoveState.WALK) {
			state = MoveState.STAY;
		}
	}
}