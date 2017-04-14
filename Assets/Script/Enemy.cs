using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	//AI
	Ai ai = new Ai();
	public Ai Ai { get { return ai; } }

	//Attacker
	[SerializeField]
	Attacker attacker = new Attacker();
	public Attacker Attacker { get { return attacker; } }

	new void Awake() {
		base.Awake();
		ai.InitializeBy(this);
		attacker.Initialize();
	}

	public new void ResetStat() {
		base.ResetStat();
		attacker.InitializeStat();
	}
}