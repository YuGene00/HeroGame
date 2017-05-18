using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ai))]
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

	public override void ResetStat() {
		base.ResetStat();
		attacker.InitializeStat();
	}
}