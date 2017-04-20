using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TerrainTrigger : MonoBehaviour {

	//terrain collider
	[SerializeField]
	Collider2D terrainCollider;

	void Awake() {
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
		Vector2 betweenExpected = (collider.bounds.size + terrainCollider.bounds.size) * 0.5f;
		float gradientExpected = betweenExpected.y / betweenExpected.x;
		Vector2 betweenActual = (collider.bounds.center - terrainCollider.bounds.center) * 0.5f;
		float gradientActual = betweenActual.y / betweenActual.x;
		return betweenActual.y >= 0 && Mathf.Abs(gradientExpected) <= Mathf.Abs(gradientActual);
	}

	void OnTriggerExit2D(Collider2D collider) {
		Physics2D.IgnoreCollision(terrainCollider, collider);
	}
}