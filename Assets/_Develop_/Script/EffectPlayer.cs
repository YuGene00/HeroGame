using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EffectPlayer : MonoBehaviour {

	//Animator
	Animator animator;

	//wait for animator
	WaitUntil[] enterWaits;
	WaitWhile[] exitWaits;

	//GameObject
	GameObject gameObj;

	void Awake() {
		animator = GetComponent<Animator>();
		InitializeWaits();
		gameObj = gameObject;
	}

	void InitializeWaits() {
		enterWaits = new WaitUntil[(int)EffectType.MAX];
		exitWaits = new WaitWhile[(int)EffectType.MAX];
		for (int i = (int)EffectType.NONE + 1; i < (int)EffectType.MAX; ++i) {
			string stateName = ((EffectType)i).ToString();
			enterWaits[i] = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));
			exitWaits[i] = new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));
		}
	}

	public void PlayEffect(EffectType effectType) {
		StartCoroutine(RunAndDestroy(effectType));
	}

	IEnumerator RunAndDestroy(EffectType effectType) {
		animator.SetTrigger(effectType.ToString());
		yield return enterWaits[(int)effectType];
		yield return exitWaits[(int)effectType];
		ObjectPool.Release(gameObj);
	}
}