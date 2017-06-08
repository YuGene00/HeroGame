using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRock : Skill {

	//RockIsInFrontOfPlayer
	IsRockInFrontOfPlayer checkRockCondition;

	public override void Initialize() {
		InitializeCheckRockCondition();
	}

	void InitializeCheckRockCondition() {
		for (int i = 0; i < conditions.Length; ++i) {
			checkRockCondition = conditions[i] as IsRockInFrontOfPlayer;
			if (checkRockCondition != null) {
				return;
			}
		}
	}

	protected override void RunEffect() {
		base.RunEffect();
		try {
			(checkRockCondition.ClosestRock.GetComponent<InteractiveTrigger>().MainScript as Rock).Destroy();
		} catch {
			return;
		}
	}
}