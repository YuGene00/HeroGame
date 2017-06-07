﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class Rock : MonoBehaviour {

	//animationController
	AnimationController animationController;

	//wait for die
	WaitUntil dieWait;

	//GameObject
	GameObject gameObj;

	void Awake() {
		animationController = GetComponent<AnimationController>();
		dieWait = new WaitUntil(() => animationController.Progress >= 1f);
		gameObj = gameObject;
	}

	public void Destroy() {
		StartCoroutine("RunDestroy");
	}

	IEnumerator RunDestroy() {
		animationController.Animate(AnimationType.DIE);
		yield return dieWait;
		ObjectPool.Release(gameObj);
	}
}