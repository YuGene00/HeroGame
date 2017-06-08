using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMover), typeof(AnimationController), typeof(HpController))]
public class Character : MainScript {

	//Transform
	Transform trans;
	public Transform Trans { get { return trans; } }

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

	//AnimationController
	protected AnimationController animationController;
	public AnimationType AnimationState { get { return animationController.State; } }

	//HPController
	protected HpController hpController;

	//immortal
	protected bool isImmortal = false;

	protected void Awake() {
		trans = transform;
		characterMover = GetComponent<CharacterMover>();
		moveCollider = trans.Find("MoveCollider").GetComponent<Collider2D>();
		halfSizeOfMoveColliderY = moveCollider.bounds.size.y * 0.5f;
		animationController = GetComponent<AnimationController>();
		hpController = GetComponent<HpController>();
	}

	void OnEnable() {
		hpController.Recovery(hpController.MaxHp.Value);
		isImmortal = false;
	}

	public void WalkTo(Direction direction) {
		SetDirection(direction);
		characterMover.WalkTo(direction);
		AnimateByMoveState();
	}

	void SetDirection(Direction direction) {
		if (Direction == direction) {
			return;
		}

		Vector3 scale = trans.localScale;
		switch (direction) {
			case Direction.LEFT:
				scale.x = Mathf.Abs(scale.x);
				break;
			case Direction.RIGHT:
				scale.x = -Mathf.Abs(scale.x);
				break;
		}
		trans.localScale = scale;
	}

	public void AnimateByMoveState() {
		AnimationType animationType = AnimationType.STAY;
		switch (characterMover.State) {
			case MoveState.STAY:
				animationType = AnimationType.STAY;
				break;
			case MoveState.WALK:
				PlayEffectIfFirstWalk();
				animationType = AnimationType.WALK;
				break;
			case MoveState.JUMP:
				animationType = AnimationType.JUMP;
				break;
		}
		Animate(animationType);
	}

	void PlayEffectIfFirstWalk() {
		if (animationController.State != AnimationType.STAY) {
			return;
		}

		EffectManager.Instance.PlayEffect(new EffectData(EffectType.WALK, Position, Direction));
	}

	public void Animate(AnimationType animation) {
		animationController.Animate(animation);
	}

	public void Jump() {
		if (animationController.State == AnimationType.JUMP) {
			return;
		}

		EffectManager.Instance.PlayEffect(new EffectData(EffectType.JUMP, Position));
		characterMover.JumpTo(Direction);
		characterMover.SetInAir(true);
		AnimateByMoveState();
	}

	public void Stop() {
		characterMover.Stop();
		AnimateByMoveState();
	}

	public virtual void ResetStat() {
		characterMover.InitializeStat();
		hpController.InitializeStat();
	}

	public virtual void Damaged(DamageData damageData) {
		if (isImmortal) {
			return;
		}

		animationController.Animate(AnimationType.DAMAGED);
		hpController.Damaged(damageData.value);
		if (hpController.CurrentHp <= 0) {
			isImmortal = true;
			animationController.Animate(AnimationType.DIE);
			DeadAction();
		}
	}

	protected virtual void DeadAction() { }

	public void Recovery(int value) {
		hpController.Recovery(value);
	}

	#region public void GiveDelay(YieldInstruction/CustomYieldInstruction wait)
	public void GiveDelay(YieldInstruction wait) {
		StopDelay();
		StartCoroutine("RunDelay", wait);
	}

	public void GiveDelay(CustomYieldInstruction wait) {
		StopDelay();
		StartCoroutine("RunCustomDelay", wait);
	}
	#endregion

	void StopDelay() {
		StopCoroutine("RunDelay");
		StopCoroutine("RunCustomDelay");
	}

	IEnumerator RunDelay(YieldInstruction wait) {
		DelayStateOn();
		yield return wait;
		DelayStateOff();
	}

	IEnumerator RunCustomDelay(CustomYieldInstruction wait) {
		DelayStateOn();
		yield return wait;
		DelayStateOff();
	}

	void DelayStateOn() {
		characterMover.IsLocked = true;
		animationController.IsLocked = true;
	}

	void DelayStateOff() {
		characterMover.IsLocked = false;
		animationController.IsLocked = false;
		AnimateByMoveState();
	}

	public void RemoveDelay() {
		StopCoroutine("RunDelay");
		DelayStateOff();
	}
}