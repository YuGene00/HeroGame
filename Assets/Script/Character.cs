﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMover), typeof(AnimationManager), typeof(HpManager))]
public class Character : MainScript {

	//Transform
	Transform trans;

	//CharacterMover
	protected CharacterMover characterMover;

	//Collider2D for MoveCollider
	Collider2D moveCollider;
	public Collider2D MoveCollider { get { return moveCollider; } }
	float halfSizeOfMoveColliderY;

	//position
	public Vector2 Position { get { return new Vector2(trans.position.x, trans.position.y - halfSizeOfMoveColliderY); } }

	//direction
	public Direction Direction {
		get {
			if (trans.localScale.x >= 0f) {
				return Direction.LEFT;
			} else {
				return Direction.RIGHT;
			}
		}
	}

	//AnimationManager
	AnimationManager animationManager;
	public AnimationType AnimationState { get { return animationManager.AnimationState; } }

	//HPManager
	HpManager hpManager;

	protected void Awake() {
		trans = transform;
		characterMover = GetComponent<CharacterMover>();
		moveCollider = trans.Find("MoveCollider").GetComponent<Collider2D>();
		halfSizeOfMoveColliderY = moveCollider.bounds.size.y * 0.5f;
		animationManager = GetComponent<AnimationManager>();
		hpManager = GetComponent<HpManager>();
	}

	public void WalkTo(Direction direction) {
		SetRotation(direction);
		characterMover.WalkTo(direction);
		AnimateByMoveState();
	}

	void SetRotation(Direction direction) {
		if (Direction == direction) {
			return;
		}

		Vector3 scale = trans.localScale;
		switch (direction) {
			case Direction.LEFT:
				scale.x = Mathf.Abs(scale.x);
				trans.localScale = scale;
				break;
			case Direction.RIGHT:
				scale.x = -Mathf.Abs(scale.x);
				trans.localScale = scale;
				break;
		}
	}

	public void AnimateByMoveState() {
		AnimationType animationType = AnimationType.STAY;
		switch (characterMover.State) {
			case MoveState.STAY:
				animationType = AnimationType.STAY;
				break;
			case MoveState.WALK:
				animationType = AnimationType.WALK;
				break;
			case MoveState.JUMP:
				animationType = AnimationType.JUMP;
				break;
		}
		Animate(animationType);
	}

	public void Jump() {
		characterMover.JumpTo(Direction);
	}

	public void Stop() {
		characterMover.Stop();
		AnimateByMoveState();
	}

	public virtual void Animate(AnimationType animation) {
		animationManager.Animate(animation);
	}

	public virtual void ResetStat() {
		characterMover.InitializeStat();
		hpManager.InitializeStat();
	}

	public virtual void Damaged(DamageData damageData) {
		animationManager.Animate(AnimationType.DAMAGED);
		hpManager.Damaged(damageData.value);
		if (hpManager.CurrentHp <= 0) {
			animationManager.Animate(AnimationType.DIE);
			DeadAction();
		}
	}

	protected virtual void DeadAction() { }

	public void Recovery(int value) {
		hpManager.Recovery(value);
	}
}