using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour, IStat {

	//max HP
	[SerializeField]
	int baseMaxHp = 0;
	Formula<int> maxHp = new Formula<int>();
	public Formula<int> MaxHp { get { return maxHp; } }

	//current HP
	int currentHp;
	public int CurrentHp { get { return currentHp; } }

	void Awake() {
		InitializeStat();
		currentHp = maxHp.Value;
	}

	public void InitializeStat() {
		maxHp.SetBaseValue(baseMaxHp);
		maxHp.Clear();
	}

	public void Damaged(int value) {
		currentHp -= value;
	}

	public void Recovery(int value) {
		currentHp = Mathf.Min(currentHp + value, maxHp.Value);
	}
}