using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ai))]
public class Enemy : Character {

	//AI
	Ai ai;

	//flag for in border
	public Direction ReachedBorder { get { return ai.ReachedBorder; } set { ai.ReachedBorder = value; } }

	new void Awake() {
		base.Awake();
		ai = GetComponent<Ai>();
	}
}