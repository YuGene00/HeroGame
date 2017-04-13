using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyTrigger : MonoBehaviour {

	//parent's Ai
	[SerializeField]
	Ai ai;

	//parent's Attacker
	[SerializeField]
	Attacker attacker;

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Border")) {
			ai.ReachToBorder();
		}
	}

	void OnTriggerStay2D(Collider2D collision) {
		Player.Instance.Damaged(attacker.Atk.Value);
	}
}