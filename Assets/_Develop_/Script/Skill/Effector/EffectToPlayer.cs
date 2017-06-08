using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectToPlayer : Effector {

	//effect type for play
	[SerializeField]
	EffectType effectType = EffectType.NONE;

	public override void RunEffect() {
		EffectManager.Instance.PlayEffect(new EffectData(effectType, Player.Instance.Position, Player.Instance.Direction), Player.Instance.Trans);
	}
}