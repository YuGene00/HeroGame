using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAt : ScopingEffector {

	//target's tag
	[SerializeField]
	string targetTag = null;

	public override void RunEffect() {
		Collider2D[] targets = scope.GetCollidersInScope();
		foreach (Collider2D target in targets) {
			if (target.CompareTag(targetTag)) {
				(target.GetComponent<InteractiveTrigger>().MainScript as Character).Damaged(new DamageData(1));
			}
		}
	}
}