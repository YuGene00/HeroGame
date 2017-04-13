using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	//singleton
	static Player instance = null;
	public static Player Instance { get { return instance; } }

	//skill
	SkillManager skillManager = new SkillManager();
	public SkillManager SkillManager { get { return skillManager; } }

	new void Awake() {
		instance = this;
		base.Awake();
	}

	public void RunPassive() {
		ResetStat();
		skillManager.RunPassive();
	}

	public void RunUnique() {
		skillManager.RunUnique();
	}

	public new void Damaged(int value) {
		EventManager.Instance.Result.IncreaseHitCount();
		base.Damaged(value);
	}

	protected override void DeadAction() {
		EventManager.Instance.PlayerDie.Run();
	}
}