using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : Mover {

	//move state
	MoveState state = MoveState.JUMP;
	public MoveState State { get { return state; } }

	//jump power
	[SerializeField]
	float baseJumpPower = 0f;
	Formula<float> jumpPower = new Formula<float>();
	public Formula<float> JumpPower { get { return jumpPower; } }
	[SerializeField]
	float speedInfluenceRatioToJump = 1;

	//variable for calculating jump vector
	Formula<float> jumpVectorX = new Formula<float>();

	//flag for locking control
	public bool IsLocked { get; set; }

	new void Awake() {
		base.Awake();
		InitializeStat();
		InitializeConstantForJumpVector();
		IsLocked = false;
	}

	public new void InitializeStat() {
		base.InitializeStat();
		jumpPower.SetBaseValue(baseJumpPower);
		jumpPower.Clear();
	}

	void InitializeConstantForJumpVector() {
		jumpVectorX.SetBaseValue(Speed);
		jumpVectorX.CreateMultiplication(speedInfluenceRatioToJump);
	}

	public void SetInAir(bool inAir) {
		if (inAir) {
			state = MoveState.JUMP;
		} else {
			state = MoveState.STAY;
			rigid.velocity = Vector2.zero;
		}
	}

	public void WalkTo(Direction direction) {
		if (IsLocked || state == MoveState.JUMP) {
			return;
		}

		state = MoveState.WALK;
		MoveTo(GetVectorBy(direction));
	}

	Vector2 GetVectorBy(Direction direction) {
		switch (direction) {
			case Direction.LEFT:
				return Vector2.left;
			default:
				return Vector2.right;
		}
	}

	public void JumpTo(Direction direction) {
		if (IsLocked || state == MoveState.JUMP) {
			return;
		}

		rigid.velocity = GetJumpVectorBy(direction);
	}

	public Vector2 GetJumpVectorBy(Direction direction) {
		Vector2 jumpVector = new Vector2(0f, jumpPower.Value);
		if (state != MoveState.STAY) {
			jumpVector.x = jumpVectorX.Value;
			if (direction == Direction.LEFT) {
				jumpVector.x = -jumpVector.x;
			}
		}
		return jumpVector;
	}

	public void Stop() {
		if (IsLocked || state != MoveState.WALK) {
			return;
		}

		state = MoveState.STAY;
		rigid.velocity = Vector2.zero;
	}
}