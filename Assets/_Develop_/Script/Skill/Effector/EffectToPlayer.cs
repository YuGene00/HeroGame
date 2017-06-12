using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectToPlayer : Effector {

	//effect type for play
	[SerializeField]
	EffectType effectType = EffectType.NONE;

	[SerializeField]
	bool IsDirectionRelatedToPlayer = true;

	public override void RunEffect() {
		Direction effectDirection = Direction.NONE;
		if (!IsDirectionRelatedToPlayer) {
			effectDirection = Player.Instance.Direction;
		}
		EffectManager.Instance.PlayEffect(new EffectData(effectType, Player.Instance.Position, effectDirection), Player.Instance.Trans);
	}
}