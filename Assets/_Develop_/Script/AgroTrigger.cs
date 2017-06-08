using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AgroTrigger : MonoBehaviour {

	//parent's Ai
	Ai aiOfParent;

	void Awake() {
		aiOfParent = transform.parent.GetComponent<Ai>();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		aiOfParent.SetAgro(true);
	}

	void OnTriggerExit2D(Collider2D collider) {
		aiOfParent.SetAgro(false);
	}
}