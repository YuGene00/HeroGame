using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingSkill : Skill {

	//AimingScope
	[SerializeField]
	AimingScope aiming = null;

	protected override void RunEffect() {
		aiming.StartAiming(base.RunEffect);
	}
}