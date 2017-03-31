﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager {

	//max HP
	Formula<int> maxHp = new Formula<int>();
	public Formula<int> MaxHp { get { return maxHp; } }

	//current HP
	int currentHp;
	public int CurrentHp { get { return currentHp; } }

	//immortal
	bool immortal = false;
	const float immortalTime = 1f;
	WaitForSeconds immortalTimeWait = new WaitForSeconds(immortalTime);

	public HPManager() {
		currentHp = maxHp.Value;
	}

	public void Damaged(int value) {
		if (!immortal) {
			currentHp -= value;
			CoroutineDelegate.Instance.StartCoroutine(RunImmortal());
		}
	}

	IEnumerator RunImmortal() {
		immortal = true;
		yield return immortalTime;
		immortal = false;
	}

	public void Recovery(int value) {
		currentHp = Mathf.Min(currentHp + value, maxHp.Value);
	}
}