using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AgroTrigger : MonoBehaviour {

	//parent
	Enemy parent;

	void Awake() {
		parent = transform.parent.GetComponent<Enemy>();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		parent.Ai.SetAgro(true);
	}

	void OnTriggerExit2D(Collider2D collider) {
		parent.Ai.SetAgro(false);
	}
}