using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderTrigger : MonoBehaviour {

	Direction borderType = Direction.NONE;

	void Awake() {
		if (CompareTag("BorderLeft")) {
			borderType = Direction.LEFT;
		} else if (CompareTag("BorderRight")) {
			borderType = Direction.RIGHT;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("Enemy")) {
			collider.GetComponent<InteractiveTrigger>().MainScript.GetComponent<Ai>().ReachedBorder = borderType;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag("Enemy")) {
			Ai aIEnemy = collider.GetComponent<InteractiveTrigger>().MainScript.GetComponent<Ai>();
			if (aIEnemy.ReachedBorder == borderType) {
				aIEnemy.ReachedBorder = Direction.NONE;
			}
		}
	}
}
