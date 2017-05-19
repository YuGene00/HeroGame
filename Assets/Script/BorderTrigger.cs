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
			(collider.GetComponent<InteractiveTrigger>().MainScript as Enemy).ReachedBorder = borderType;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag("Enemy")) {
			Enemy enemy = collider.GetComponent<InteractiveTrigger>().MainScript as Enemy;
			if (enemy.ReachedBorder == borderType) {
				enemy.ReachedBorder = Direction.NONE;
			}
		}
	}
}