using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyTrigger : MonoBehaviour {

	//parent
	Enemy parent;

	//parent's Ai
	Ai aiOfParent;

	//parent's Attacker
	Attacker attackerOfParent;

	void Awake() {
		parent = transform.parent.GetComponent<Enemy>();
		aiOfParent = parent.GetComponent<Ai>();
		attackerOfParent = parent.GetComponent<Attacker>();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("BorderLeft")) {
			aiOfParent.ReachedBorder = Direction.LEFT;
		} else if (collider.CompareTag("BorderRight")) {
			aiOfParent.ReachedBorder = Direction.RIGHT;
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			Player.Instance.Damaged(attackerOfParent.Atk.Value);
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if ((collider.CompareTag("BorderLeft") && aiOfParent.ReachedBorder == Direction.LEFT)
			|| (collider.CompareTag("BorderRight") && aiOfParent.ReachedBorder == Direction.RIGHT)) {
			aiOfParent.ReachedBorder = Direction.NONE;
		}
	}
}