using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRockInFrontOfPlayer : Condition {

	//circle for check collider
	[SerializeField]
	float radius = 0f;

	//closest rock
	Collider2D closestRock;
	public Collider2D ClosestRock { get { return closestRock; } }

	public override bool IsTrue() {
		SetClosestRock();
		if (closestRock != null) {
			return true;
		} else {
			return false;
		}
	}

	void SetClosestRock() {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(GetCircleCenter(), radius);
		closestRock = null;
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
	}

	Vector2 GetCircleCenter() {
		Vector2 checkCenter = Player.Instance.Position;
		float halfRadius = radius * 0.5f;
		if (Player.Instance.Direction == Direction.LEFT) {
			checkCenter.x -= halfRadius;
		} else {
			checkCenter.x += halfRadius;
		}
		checkCenter.y += halfRadius;
		return checkCenter;
	}
}