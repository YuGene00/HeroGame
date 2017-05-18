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
	const float immortalTime = 1f;
	WaitForSeconds immortalTimeWait = new WaitForSeconds(immortalTime);

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

	public override void Damaged(int value) {
		if (isImmortal) {
			return;
		}
		//EventManager.Instance.Result.IncreaseHitCount();
		base.Damaged(value);
		CoroutineDelegate.Instance.StartCoroutine(RunImmortal());
		Debug.Log("Ouch!");
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