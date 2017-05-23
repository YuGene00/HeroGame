using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkillManager))]
public class Player : Character {

	//singleton
	static Player instance = null;
	public static Player Instance { get { return instance; } }

	//skill
	SkillManager skillManager;
	public SkillManager SkillManager { get { return skillManager; } }

	//immortal
	bool isImmortal = false;
	const float immortalTime = 2.5f;
	WaitForSeconds immortalTimeWait = new WaitForSeconds(immortalTime);

	//KnockBack
	[SerializeField]
	Vector2 knockBackPower = Vector2.zero;
	const float knockBackTime = 0.2f;
	WaitForSeconds knockBackTimeWait = new WaitForSeconds(knockBackTime);

	new void Awake() {
		instance = this;
		base.Awake();
		skillManager = GetComponent<SkillManager>();
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
		isImmortal = true;
		yield return immortalTimeWait;
		isImmortal = false;
	}

	protected override void DeadAction() {
		EventManager.Instance.PlayerDie.Run();
	}
}