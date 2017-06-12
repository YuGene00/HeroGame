using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInScope : ScopingCondition {

	[SerializeField]
	string targetTag = null;

	public override bool IsTrue() {
		Collider2D[] colliders = scope.GetCollidersInScope();
		for (int i = 0; i < colliders.Length; ++i) {
			if (colliders[i].CompareTag(targetTag)) {
				return true;
			}
		}
		return false;
	}
}