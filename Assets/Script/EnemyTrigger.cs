using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyTrigger : MonoBehaviour {

	//parent
	Enemy parent;

	void Awake() {
		parent = transform.parent.GetComponent<Enemy>();
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Border")) {
			parent.Ai.ReachToBorder();
		}
	}

	void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			Player.Instance.Damaged(parent.Attacker.Atk.Value);
		}
	}
}