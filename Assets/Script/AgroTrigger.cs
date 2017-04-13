using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AgroTrigger : MonoBehaviour {

	//parent's AI
	[SerializeField]
	Ai ai;

	void OnTriggerEnter2D(Collider2D collision) {
		ai.SetAgro(true);
	}

	void OnTriggerExit2D(Collider2D collision) {
		ai.SetAgro(false);
	}
}