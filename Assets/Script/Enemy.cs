using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ai), typeof(Attacker))]
public class Enemy : Character {

	//AI
	Ai ai;

	//Attacker
	Attacker attacker;

	new void Awake() {
		base.Awake();
		ai = GetComponent<Ai>();
		attacker = GetComponent<Attacker>();
	}

	public new void ResetStat() {
		base.ResetStat();
		attacker.InitializeStat();
	}
}