using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAt : ScopingEffector {

	//effect type
	[SerializeField]
	EffectType effectType = EffectType.NONE;

	//effect position in scope
	[SerializeField]
	Vector2 positionInScope = Vector2.zero;

	public override void RunEffect() {
		Vector2 posOffset = new Vector2(scope.ScopingArea.size.x * positionInScope.x, scope.ScopingArea.size.y * positionInScope.y);
		Vector2 effectPos = scope.ScopingArea.min + posOffset;
		EffectManager.Instance.PlayEffect(new EffectData(effectType, effectPos, Direction.NONE));
	}
}