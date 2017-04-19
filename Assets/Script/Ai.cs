using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

	//target Enemy
	Enemy target;

	//flag for play Ai
	public bool Play { get; set; }
	WaitUntil playWait;

	//flag for check agro
	bool isAgro = false;

	//flag for in border
	bool isInBorder = false;

	//Collider2D for parent's MoveCollider
	Collider2D moveCollider;

	//constant for paravola
	float cosPower;
	float halfBOfQuadraticFormula;
	float mass;
	Collider2D[] tileColliders;

	//variable for paravola related to jump power
	float aOfQuadraticFormula;

	public void InitializeBy(Enemy target) {
		this.target = target;
		moveCollider = this.target.transform.FindChild("MoveCollider").GetComponent<Collider2D>();
		InitializeConstantForParavola();
		InitializeVariableForParavolaRelatedToJumpPower();
		Play = true;
		playWait = new WaitUntil(() => Play);
		CoroutineDelegate.Instance.StartCoroutine(RunAi());
	}

	public void InitializeConstantForParavola() {
		const float radian = 45f * 3.141592653589f / 180f;
		cosPower = Mathf.Pow(Mathf.Cos(radian), 2f);
		halfBOfQuadraticFormula = Mathf.Tan(radian) / 2;
		mass = target.GetComponent<Rigidbody2D>().mass;
		InitializeTileColliders();
	}

	void InitializeTileColliders() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		tileColliders = new Collider2D[tiles.Length];
		for (int i = 0; i < tiles.Length; ++i) {
			tileColliders[i] = tiles[i].GetComponent<Collider2D>();
		}
	}

	public void InitializeVariableForParavolaRelatedToJumpPower() {
		float v0 = target.CharacterMover.JumpPower.Value / mass;
		float g = Physics2D.gravity.y;
		aOfQuadraticFormula = g / (2 * Mathf.Pow(v0, 2f) * cosPower);
	}

	IEnumerator RunAi() {
		while (true) {
			yield return playWait;
			if (isAgro) {
				TracePlayer();
			} else {
				Wonder();
			}
		}
	}

	void TracePlayer() {
		
	}

	bool CanGoToHigher(CharacterMover.Direction direction) {
		float v = target.GetComponent<Rigidbody2D>().velocity.magnitude;
		for (int i = 0; i < tileColliders.Length; ++i) {
			Vector2 center = tileColliders[i].bounds.center - moveCollider.bounds.center;
			float xOffset = tileColliders[i].bounds.size.x / 2f;
			float moveColliderHalfWidth = moveCollider.bounds.size.x / 2f;
			float tileX1 = center.x - xOffset - moveColliderHalfWidth;
			float tileX2 = center.x + xOffset + moveColliderHalfWidth;
			float tileY = center.y + (tileColliders[i].bounds.size.y + moveCollider.bounds.size.y) / 2f;
			switch (direction) {
				case CharacterMover.Direction.LEFT:
					return DoesLineMeetParavolaAtY(-tileX2, -tileX1, tileY);
				case CharacterMover.Direction.RIGHT:
					return DoesLineMeetParavolaAtY(tileX1, tileX2, tileY);
				case CharacterMover.Direction.NONE:

					break;
			}
		}
		return false;
	}

	bool DoesLineMeetParavolaAtY(float lineX1, float lineX2, float y) {
		y = 0;
		float determinationValue = Mathf.Pow(halfBOfQuadraticFormula, 2f) - aOfQuadraticFormula * -y;
		if (determinationValue <= 0) {
			return false;
		}

		float sqrtOfDeterminationValue = Mathf.Sqrt(determinationValue);
		float value = (-halfBOfQuadraticFormula - sqrtOfDeterminationValue) / aOfQuadraticFormula;
		return lineX1 <= value && value <= lineX2;
	}

	void Wonder() {
		CharacterMover.Direction direction = target.Direction;
		if (isInBorder) {
			direction = GetReverseDirection(direction);
			isInBorder = false;
		}
		target.WalkTo(direction);
		if (CanGoToHigher(direction)) {
			return;
		}
		target.Jump();
	}

	CharacterMover.Direction GetReverseDirection(CharacterMover.Direction direction) {
		switch (direction) {
			case CharacterMover.Direction.LEFT:
				return CharacterMover.Direction.RIGHT;
			default:
				return CharacterMover.Direction.LEFT;
		}
	}

	public void SetAgro(bool isAgro) {
		this.isAgro = isAgro;
	}

	public void ReachToBorder() {
		isInBorder = true;
	}
}