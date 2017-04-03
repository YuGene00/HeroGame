using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackTrigger : MonoBehaviour {

	//parent's Attacker
	[SerializeField]
	Attacker attacker;

	void OnTriggerStay(Collider collision) {
		Player.Instance.Damaged(attacker.Atk.Value);
	}
}