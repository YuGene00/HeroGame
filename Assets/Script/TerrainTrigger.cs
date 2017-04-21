using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TerrainTrigger : MonoBehaviour {

	//terrain collider
	[SerializeField]
	Collider2D terrainCollider;

	//half size of terrain collider
	Vector2 halfTerrainColliderSize;

	//save velocity value
	static Vector2 originalVelocity;

	void Awake() {
		halfTerrainColliderSize = terrainCollider.bounds.size * 0.5f;
		SetIgnoreCollision();
	}

	void SetIgnoreCollision() {
		GameObject[] moveColliders = GameObject.FindGameObjectsWithTag("MoveCollider");
		for (int i = 0; i < moveColliders.Length; ++i) {
			Physics2D.IgnoreCollision(terrainCollider, moveColliders[i].GetComponent<Collider2D>());
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (IsVerticalCollisionBy(collider)) {
			Physics2D.IgnoreCollision(terrainCollider, collider, false);
		}
	}

	bool IsVerticalCollisionBy(Collider2D collider) {
		Vector3 revisedPosition = RevisePositionOf(collider);
		Vector2 betweenActual = revisedPosition - terrainCollider.bounds.center;
		if (betweenActual.y < 0) {
			return false;
		}
		float gradientActual = betweenActual.y / betweenActual.x;
		Vector2 betweenExpected = collider.bounds.size + terrainCollider.bounds.size;
		float gradientExpected = betweenExpected.y / betweenExpected.x;
		return Mathf.Abs(gradientExpected) <= Mathf.Abs(gradientActual);
	}

	Vector3 RevisePositionOf(Collider2D collider) {
		Vector2 velocity = collider.attachedRigidbody.velocity;
		if (velocity == Vector2.zero) {
			return collider.bounds.center;
		}
		Vector2 overlapedSpace = GetOverlapedSpace(collider);
		float velocityGradient = Mathf.Abs(velocity.y / velocity.x);
		float overlapedSpaceGradient = Mathf.Abs(overlapedSpace.y / overlapedSpace.x);
		Vector2 revisionOffset;
		if (velocityGradient < overlapedSpaceGradient) {
			revisionOffset.x = overlapedSpace.x;
			revisionOffset.y = (overlapedSpace.x * velocity.y) / velocity.x;
		} else {
			revisionOffset.x = (overlapedSpace.y * velocity.x) / velocity.y;
			revisionOffset.y = overlapedSpace.y;
		}
		return (Vector2)collider.bounds.center - revisionOffset;
	}

	Vector2 GetOverlapedSpace(Collider2D collider) {
		Vector2 overlapedSpace;
		Vector2 halfColliderSize = collider.bounds.size * 0.5f;
		if (terrainCollider.bounds.center.x < collider.bounds.center.x) {
			overlapedSpace.x = -(terrainCollider.bounds.center.x + halfTerrainColliderSize.x - (collider.bounds.center.x - halfColliderSize.x));
		} else {
			overlapedSpace.x = collider.bounds.center.x + halfColliderSize.x - (terrainCollider.bounds.center.x - halfTerrainColliderSize.x);
		}
		if (terrainCollider.bounds.center.y < collider.bounds.center.y) {
			overlapedSpace.y = -(terrainCollider.bounds.center.y + halfTerrainColliderSize.y - (collider.bounds.center.y - halfColliderSize.y));
		} else {
			overlapedSpace.y = collider.bounds.center.y + halfColliderSize.y - (terrainCollider.bounds.center.y - halfTerrainColliderSize.y);
		}

		return overlapedSpace;
	}

	void OnTriggerExit2D(Collider2D collider) {
		Physics2D.IgnoreCollision(terrainCollider, collider);
	}
}