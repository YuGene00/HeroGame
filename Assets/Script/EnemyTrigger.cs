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

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("BorderLeft")) {
			parent.Ai.ReachedBorder = CharacterMover.Direction.LEFT;
		} else if (collider.CompareTag("BorderRight")) {
			parent.Ai.ReachedBorder = CharacterMover.Direction.RIGHT;
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			Player.Instance.Damaged(parent.Attacker.Atk.Value);
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if ((collider.CompareTag("BorderLeft") && parent.Ai.ReachedBorder == CharacterMover.Direction.LEFT)
			|| (collider.CompareTag("BorderRight") && parent.Ai.ReachedBorder == CharacterMover.Direction.RIGHT)) {
			parent.Ai.ReachedBorder = CharacterMover.Direction.NONE;
		}
	}
}