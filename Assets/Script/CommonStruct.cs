using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//damage data
public struct DamageData {

	public int value;
	public Transform attacker;

	public DamageData(int value = 0, Transform attacker = null) {
		this.value = value;
		this.attacker = attacker;
	}
}