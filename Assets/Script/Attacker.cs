using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attacker {

	//attack value
	[SerializeField]
	int baseAtk;
	Formula<int> atk = new Formula<int>();
	public Formula<int> Atk { get { return atk; } }

	public void Initialize() {
		InitializeStat();
		atk.SetBaseValue(baseAtk);
	}

	public void InitializeStat() {
		atk.Clear();
	}
}