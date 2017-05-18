using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpManager : MonoBehaviour, IStat {

	//max HP
	[SerializeField]
	int baseMaxHp;
	Formula<int> maxHp = new Formula<int>();
	public Formula<int> MaxHp { get { return maxHp; } }

	//current HP
	int currentHp;
	public int CurrentHp { get { return currentHp; } }

	//immortal
	bool immortal = false;
	const float immortalTime = 1f;
	WaitForSeconds immortalTimeWait = new WaitForSeconds(immortalTime);

	void Awake() {
		InitializeStat();
		currentHp = maxHp.Value;
	}

	public void InitializeStat() {
		maxHp.SetBaseValue(baseMaxHp);
		maxHp.Clear();
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