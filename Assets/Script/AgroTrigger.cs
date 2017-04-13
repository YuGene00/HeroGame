using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AgroTrigger : MonoBehaviour {

	//parent's AI
	[SerializeField]
	Ai ai;

	void OnTriggerEnter(Collider collision) {
		ai.SetAgro(true);
	}

	void OnTriggerExit(Collider collision) {
		ai.SetAgro(false);
	}
}