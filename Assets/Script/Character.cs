using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Character : MonoBehaviour {

	//move
	Transform trans;
	[SerializeField]
	CharacterMover characterMover = new CharacterMover();
	public CharacterMover CharacterMover { get { return characterMover; } }

	//Collider2D for MoveCollider
	Collider2D moveCollider;
	public Collider2D MoveCollider { get { return moveCollider; } }
	float halfSizeOfMoveColliderY;

	//position
	public Vector2 Position { get { return new Vector2(trans.position.x, trans.position.y - halfSizeOfMoveColliderY); } }

	//direction
	public CharacterMover.Direction Direction {
		get {
			if (trans.localScale.x >= 0f) {
				return CharacterMover.Direction.LEFT;
			} else {
				return CharacterMover.Direction.RIGHT;
			}
		}
	}

	//animation
	AnimationManager animationManager = new AnimationManager();
	public AnimationManager AnimationManager { get { return animationManager; } }

	//HP
	[SerializeField]
	HpManager hpManager = new HpManager();
	public HpManager HpManager { get { return hpManager; } }

	protected void Awake() {
		trans = transform;
		characterMover.InitializeBy(GetComponent<Rigidbody2D>());
		moveCollider = trans.FindChild("MoveCollider").GetComponent<Collider2D>();
		halfSizeOfMoveColliderY = moveCollider.bounds.size.y * 0.5f;
		animationManager.InitializeBy(GetComponent<Animator>());
		hpManager.Initialize();
	}

	public void WalkTo(CharacterMover.Direction direction) {
		SetRotation(direction);
		characterMover.WalkTo(direction);
		AnimateByMoveState();
	}

	void SetRotation(CharacterMover.Direction direction) {
		if (Direction == direction) {
			return;
		}

		Vector3 scale = trans.localScale;
		switch (direction) {
			case CharacterMover.Direction.LEFT:
				scale.x = Mathf.Abs(scale.x);
				trans.localScale = scale;
				break;
			case CharacterMover.Direction.RIGHT:
				scale.x = -Mathf.Abs(scale.x);
				trans.localScale = scale;
				break;
		}
	}

	public void AnimateByMoveState() {
		AnimationManager.AnimationType animationType = AnimationManager.AnimationType.STAY;
		switch (characterMover.State) {
			case CharacterMover.MoveState.STAY:
				animationType = AnimationManager.AnimationType.STAY;
				break;
			case CharacterMover.MoveState.WALK:
				animationType = AnimationManager.AnimationType.WALK;
				break;
			case CharacterMover.MoveState.JUMP:
				animationType = AnimationManager.AnimationType.STAY;
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

	public void Animate(AnimationManager.AnimationType animation) {
		animationManager.Animate(animation);
	}

	public void ResetStat() {
		characterMover.InitializeStat();
		hpManager.InitializeStat();
	}

	public void Damaged(int value) {
		hpManager.Damaged(value);
		if (hpManager.CurrentHp <= 0) {
			animationManager.Animate(AnimationManager.AnimationType.DIE);
			DeadAction();
		}
	}

	protected virtual void DeadAction() { }

	public void Recovery(int value) {
		hpManager.Recovery(value);
	}
}