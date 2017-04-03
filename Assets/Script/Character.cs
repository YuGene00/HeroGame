using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Character : MonoBehaviour {

	//move
	SpriteRenderer spriteRenderer;
	[SerializeField]
	CharacterMover characterMover = new CharacterMover();
	public CharacterMover CharacterMover { get { return characterMover; } }

	//animation
	AnimationManager animationManager = new AnimationManager();

	//HP
	[SerializeField]
	HpManager hpManager = new HpManager();
	public HpManager HpManager { get { return hpManager; } }

	protected void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		characterMover.InitializeBy(GetComponent<Rigidbody2D>());
		animationManager.InitializeBy(GetComponent<Animator>());
		hpManager.Initialize();
	}

	public void WalkTo(CharacterMover.Direction direction) {
		spriteRenderer.flipX = !spriteRenderer.flipX;
		characterMover.WalkTo(direction);
		if (characterMover.State != CharacterMover.MoveState.JUMP) {
			animationManager.Animate(AnimationManager.AnimationType.WALK);
		}
	}

	public void JumpTo(CharacterMover.Direction direction) {
		characterMover.JumpTo(direction);
	}

	public void Stop() {
		characterMover.Stop();
	}

	public void Animate(AnimationManager.AnimationType animation) {
		animationManager.Animate(animation);
	}

	public void ResetStat() {
		characterMover.InitializeStat();
		hpManager.InitializeStat();
	}

	public virtual void Damaged(int value) {
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