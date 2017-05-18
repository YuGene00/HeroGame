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

	public new void Damaged(int value) {
		//EventManager.Instance.Result.IncreaseHitCount();
		base.Damaged(value);
	}

	protected override void DeadAction() {
		EventManager.Instance.PlayerDie.Run();
	}
}