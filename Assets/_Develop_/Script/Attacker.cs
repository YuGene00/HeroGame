﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour, IStat {

	//attack value
	[SerializeField]
	int baseAtk = 0;
	Formula<int> atk = new Formula<int>();
	public Formula<int> Atk { get { return atk; } }

	//attack target Tag
	[SerializeField]
	string targetTag = null;

	//Transform for DamageData
	Transform trans;

	void Awake() {
		InitializeStat();
		trans = transform;
	}

	public void InitializeStat() {
		atk.SetBaseValue(baseAtk);
		atk.Clear();
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.CompareTag(targetTag)) {
			Character target = collider.GetComponent<InteractiveTrigger>().MainScript as Character;
			if (target == null) {
				return;
			}
			target.Damaged(new DamageData(atk.Value, trans));
		}
	}
}