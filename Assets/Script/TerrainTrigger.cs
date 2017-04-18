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
		Vector2 betweenExpected = (collider.bounds.size + terrainCollider.bounds.size) / 2f;
		float gradientExpected = betweenExpected.y / betweenExpected.x;
		Vector2 betweenActual = (collider.bounds.center - terrainCollider.bounds.center) / 2;
		float gradientActual = betweenActual.y / betweenActual.x;
		if (betweenActual.y >= 0 && Mathf.Abs(gradientExpected) <= Mathf.Abs(gradientActual)) {
			Physics2D.IgnoreCollision(terrainCollider, collider, false);
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		Physics2D.IgnoreCollision(terrainCollider, collider);
	}
}