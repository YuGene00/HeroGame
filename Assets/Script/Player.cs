using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkillManager), typeof(SpriteRenderer))]
public class Player : Character {

	//singleton
	static Player instance = null;
	public static Player Instance { get { return instance; } }

	//skill
	SkillManager skillManager;
	public SkillManager SkillManager { get { return skillManager; } }

	//immortal
	bool isImmortal = false;
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
	Color blankColor;

	//KnockBack
	[SerializeField]
	Vector2 knockBackPower = Vector2.zero;
	const float knockBackTime = 0.25f;
	WaitForSeconds knockBackTimeWait = new WaitForSeconds(knockBackTime);

	new void Awake() {
		instance = this;
		base.Awake();
		skillManager = GetComponent<SkillManager>();
		repeatImmortalUnitCount = (int)(immortalTime / unitImmortalTime);
		repeatBlankUnitCount = (int)(immortalBlankTime / unitImmortalTime);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void RunPassive() {
		ResetStat();
		skillManager.RunPassive();
	}

	public void RunUnique() {
		skillManager.RunUnique();
	}

	public override void Animate(AnimationType animation) {
		if (characterMover.IsMoveControlBanned) {
			return;
		}

		base.Animate(animation);
	}

	public override void Damaged(DamageData damageData) {
		if (isImmortal) {
			return;
		}

		//EventManager.Instance.Result.IncreaseHitCount();
		KnockBack(damageData.attacker);
		base.Damaged(damageData);
		StartCoroutine("RunImmortal");
		Debug.Log("Ouch!");
	}

	void KnockBack(Transform attacker) {
		Vector2 knockBackDirection = knockBackPower;
		if (Position.x < attacker.position.x) {
			knockBackDirection.x = -knockBackDirection.x;
		}
		StartCoroutine("RunMoveBanForDamaged");
		characterMover.MoveTo(knockBackDirection);
	}

	IEnumerator RunMoveBanForDamaged() {
		characterMover.IsMoveControlBanned = true;
		yield return knockBackTimeWait;
		characterMover.IsMoveControlBanned = false;
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
		EventManager.Instance.PlayerDie.Run();
	}
}