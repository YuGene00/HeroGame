using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

	//singleton
	static Player instance = null;
	public static Player Instance { get { return instance; } }
	
	//move
	SpriteRenderer spriteRenderer;
	[SerializeField]
	public CharacterMover characterMover;

	//animation
	AnimationManager animationManager;

	//skill
	SkillManager skillManager;

	//HP
	HPManager hpManager;

	void Awake() {
		instance = this;
		spriteRenderer = GetComponent<SpriteRenderer>();
		characterMover = CharacterMover.CreateByTarget(GetComponent<Rigidbody2D>());
		animationManager = AnimationManager.CreateByTarger(GetComponent<Animator>());
		skillManager = new SkillManager();
		hpManager = new HPManager();
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

	public void RunPassive() {
		skillManager.RunPassive();
	}

	public void RunUnique() {
		skillManager.RunUnique();
	}

	public void Damaged(int value) {
		hpManager.Damaged(value);
		EventManager.Instance.Result.IncreaseHitCount();
		if (hpManager.CurrentHp <= 0) {
			animationManager.Animate(AnimationManager.AnimationType.DIE);
			EventManager.Instance.PlayerDie.Run();
		}
	}

	public void Recovery(int value) {
		hpManager.Recovery(value);
	}
}