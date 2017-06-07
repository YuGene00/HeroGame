using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EffectPlayer : MonoBehaviour {

	//Animator
	Animator animator;

	//wait for animator
	WaitUntil enterWalkWait;
	WaitWhile exitWalkWait;
	WaitUntil enterJumpWait;
	WaitWhile exitJumpWait;
	WaitUntil enterLandWait;
	WaitWhile exitLandWait;

	//GameObject
	GameObject gameObj;

	void Awake() {
		animator = GetComponent<Animator>();
		enterWalkWait = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"));
		exitWalkWait = new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"));
		enterJumpWait = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));
		exitJumpWait = new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));
		enterLandWait = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Land"));
		exitLandWait = new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Land"));
		gameObj = gameObject;
	}

	public void PlayEffect(EffectType effectType) {
		StartCoroutine(RunAndDestroy(effectType));
	}

	IEnumerator RunAndDestroy(EffectType effectType) {
		switch (effectType) {
			case EffectType.WALK:
				animator.SetTrigger("Walk");
				yield return enterWalkWait;
				yield return exitWalkWait;
				break;
			case EffectType.JUMP:
				animator.SetTrigger("Jump");
				yield return enterJumpWait;
				yield return exitJumpWait;
				break;
			case EffectType.LAND:
				animator.SetTrigger("Land");
				yield return enterLandWait;
				yield return exitLandWait;
				break;
		}
		ObjectPool.Release(gameObj);
	}
}