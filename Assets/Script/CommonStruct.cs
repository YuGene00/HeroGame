using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//damage data
public struct DamageData {

	public int value;
	public Transform attacker;

	public DamageData(int value = 0, Transform attacker = null) {
		this.value = value;
		this.attacker = attacker;
	}
}

//effect data
public struct EffectData {

	public EffectType effectType;
	public Vector2 position;
	public Direction direction;

	public EffectData(EffectType effectType, Vector2 position = default(Vector2), Direction direction = Direction.NONE) {
		this.effectType = effectType;
		this.position = position;
		this.direction = direction;
	}
}