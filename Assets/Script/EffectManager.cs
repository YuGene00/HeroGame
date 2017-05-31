using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	//singleton
	static EffectManager instance = null;
	public static EffectManager Instance { get { return instance; } }

	//effect object pool
	[SerializeField]
	GameObject effectPlayer = null;
	ObjectPool effectPool;

	void Awake() {
		instance = this;
		effectPool = ObjectPool.CreateFor(effectPlayer);
	}

	public void PlayEffect(EffectData effectData, Transform parent = null) {
		GameObject effectPlayer = effectPool.Retain(effectData.position);
		if (parent != null) {
			effectPlayer.transform.SetParent(parent);
		}
		SetEffectDirection(effectPlayer, effectData.direction);
		effectPlayer.GetComponent<EffectPlayer>().PlayEffect(effectData.effectType);
	}

	void SetEffectDirection(GameObject effectPlayer, Direction direction) {
		Transform effectTrans = effectPlayer.transform;
		Vector3 scale = effectTrans.localScale;
		switch (direction) {
			case Direction.RIGHT:
				scale.x = -Mathf.Abs(scale.x);
				break;
			default:
				scale.x = Mathf.Abs(scale.x);
				break;
		}
		effectTrans.localScale = scale;
	}
}