using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour, IStat {

	//attack value
	[SerializeField]
	int baseAtk;
	Formula<int> atk = new Formula<int>();
	public Formula<int> Atk { get { return atk; } }

	void Awake() {
		InitializeStat();
	}

	public void InitializeStat() {
		atk.SetBaseValue(baseAtk);
		atk.Clear();
	}
}