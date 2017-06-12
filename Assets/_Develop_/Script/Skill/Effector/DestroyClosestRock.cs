using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyClosestRock : ScopingEffector {

	public override void RunEffect() {
		try {
			(FindClosestRock().GetComponent<InteractiveTrigger>().MainScript as Rock).Destroy();
		} catch {
			return;
		}
	}

	Collider2D FindClosestRock() {
		Collider2D[] colliders = scope.GetCollidersInScope();
		Collider2D closestRock = null;
		float minDistance = -1f;
		for (int i = 0; i < colliders.Length; ++i) {
			if (colliders[i].CompareTag("Rock")) {
				float sqrDistance = ((Vector2)colliders[i].transform.position - Player.Instance.Position).sqrMagnitude;
				if (minDistance == -1f || sqrDistance < minDistance) {
					closestRock = colliders[i];
					minDistance = sqrDistance;
				}
			}
		}
		return closestRock;
	}
}