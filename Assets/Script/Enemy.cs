using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	//AI
	Ai ai = new Ai();

	//Attacker
	[SerializeField]
	Attacker attacker = new Attacker();
	public Attacker Attacker { get { return attacker; } }

	new void Awake() {
		base.Awake();
		attacker.Initialize();
	}

	public new void ResetStat() {
		base.ResetStat();
		attacker.InitializeStat();
	}
}