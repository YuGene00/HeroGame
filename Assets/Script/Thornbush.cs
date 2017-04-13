using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Thornbush : MonoBehaviour {

	//Attacker
	[SerializeField]
	Attacker attacker = new Attacker();
	public Attacker Attacker { get { return attacker; } }

	void Awake() {
		attacker.Initialize();
	}
}