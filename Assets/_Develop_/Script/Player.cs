using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkillController), typeof(SpriteRenderer))]
public class Player : Character {

	//singleton
	static Player instance = null;
	public static Player Instance { get { return instance; } }

	//skill
	SkillController skillController;
	public SkillController SkillController { get { return skillController; } }

	//immortal
	[SerializeField]
	float immortalTime = 0f;
	const float unitImmortalTime = 0.1f;
	WaitForSeconds unitImmortalTimeWait = new WaitForSeconds(unitImmortalTime);
	int repeatImmortalUnitCount;
	[SerializeField]
	float immortalBlankTime = 0f;
	int repeatBlankUnitCount;

	//damaged blank
	SpriteRenderer spriteRenderer;
	[SerializeField]
	Color blankColor = new Color(1f, 1f, 1f, 0.5f);

	//KnockBack
	[SerializeField]
	Vector2 knockBackPower = Vector2.zero;
	const float knockBackTime = 0.5f;
	WaitForSeconds knockBackTimeWait = new WaitForSeconds(knockBackTime);

	//Wait for animation end
	WaitUntil animationEndWait;
	delegate void MethodAfterAnimation();

	new void Awake() {
		instance = this;
		base.Awake();
		skillController = GetComponent<SkillController>();
		repeatImmortalUnitCount = (int)(immortalTime / unitImmortalTime);
		repeatBlankUnitCount = (int)(immortalBlankTime / unitImmortalTime);
		spriteRenderer = GetComponent<SpriteRenderer>();
		animationEndWait = new WaitUntil(() => animationController.Progress >= 1f);
	}

	public void RunPassive() {
		ResetStat();
		skillController.RunPassive();
	}

	public void RunUnique() {
		skillController.RunUnique();
	}

	public void RunUltimate() {
		skillController.RunUltimate();
	}

	IEnumerator RunCallAfterFinishAnimation(MethodAfterAnimation method) {
		yield return null;
		yield return animationEndWait;
		method();
	}

	public override void Damaged(DamageData damageData) {
		if (isImmortal) {
			return;
		}

		EventManager.Instance.IncreaseHitCount();
		base.Damaged(damageData);
		if (hpController.CurrentHp > 0) {
			KnockBack(damageData.attacker);
			CoroutineDelegate.Instance.StartCoroutine(RunImmortal());
		}
	}

	void KnockBack(Transform attacker) {
		Vector2 knockBackDirection = knockBackPower;
		if (Position.x < attacker.position.x) {
			knockBackDirection.x = -knockBackDirection.x;
		}
		GiveDelay(knockBackTimeWait);
		characterMover.MoveTo(knockBackDirection);
	}

	IEnumerator RunImmortal() {
		Color originColor = spriteRenderer.color;
		int blankCount = repeatBlankUnitCount;
		isImmortal = true;
		for (int immortalCount = 0; immortalCount < repeatImmortalUnitCount; ++immortalCount) {
			if (repeatBlankUnitCount <= blankCount) {
				if (spriteRenderer.color != originColor) {
					spriteRenderer.color = originColor;
				} else {
					spriteRenderer.color = blankColor;
				}
				blankCount = 0;
			} else {
				++blankCount;
			}
			yield return unitImmortalTimeWait;
		}
		spriteRenderer.color = originColor;
		isImmortal = false;
	}

	protected override void DeadAction() {
		DelayStateOn();
		CallAfterFinishAnimation(EventManager.Instance.PlayerDie);
	}

	void CallAfterFinishAnimation(MethodAfterAnimation method) {
		CoroutineDelegate.Instance.StartCoroutine(RunCallAfterFinishAnimation(method));
	}

	public void AnimateAsImmortal(AnimationType animationType) {
		animationController.Animate(animationType);
		GiveDelay(animationEndWait);
		isImmortal = true;
		CallAfterFinishAnimation(() => isImmortal = false);
	}
}