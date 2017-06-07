using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject {

	public abstract bool IsTrue();
}

public class RockIsInFrontOfPlayer : Condition {

	//circle for check collider
	[SerializeField]
	float xFromPlayer = 0f;
	public float XFromPlayer { get { return xFromPlayer; } }
	[SerializeField]
	float radius = 0f;
	public float Radius { get { return radius; } }

	public override bool IsTrue() {
		if (Physics2D.OverlapCircle(GetCircleCenter(), radius) != null) {
			return true;
		} else {
			return false;
		}
	}

	Vector2 GetCircleCenter() {
		Vector2 checkCenter = Player.Instance.Position;
		if (Player.Instance.Direction == Direction.LEFT) {
			checkCenter.x -= xFromPlayer;
		} else {
			checkCenter.x += xFromPlayer;
		}
		checkCenter.y += radius * 0.5f;
		return checkCenter;
	}
}