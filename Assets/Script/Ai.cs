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
	float cos;
	float linearCoefficient;
	Collider2D[] tileColliders;

	//variable for paravola
	float quadraticCoefficient;

	public void InitializeBy(Enemy target) {
		this.target = target;
		moveCollider = this.target.transform.FindChild("MoveCollider").GetComponent<Collider2D>();
		InitializeConstantForParavola();
		InitializeVariableForParavola();
		Play = true;
		playWait = new WaitUntil(() => Play);
		CoroutineDelegate.Instance.StartCoroutine(RunAi());
	}

	public void InitializeConstantForParavola() {
		const float radian = 45f * 3.141592653589f / 180f;
		cos = Mathf.Cos(radian);
		linearCoefficient = Mathf.Tan(radian);
		InitializeTileColliders();
	}

	void InitializeTileColliders() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		tileColliders = new Collider2D[tiles.Length];
		for (int i = 0; i < tiles.Length; ++i) {
			tileColliders[i] = tiles[i].GetComponent<Collider2D>();
		}
	}

	public void InitializeVariableForParavola() {
		float v0 = target.CharacterMover.JumpPower.Value;
		float g = -Physics2D.gravity.y;
		quadraticCoefficient = g / (2 * Mathf.Pow(v0, 2f) * Mathf.Pow(cos, 2f));
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
		for (int i = 0; i < tileColliders.Length; ++i) {
			Vector2 center = tileColliders[i].bounds.center - moveCollider.bounds.center;
			float xOffset = tileColliders[i].bounds.size.x / 2f;
			float moveColliderHalfWidth = moveCollider.bounds.size.x / 2f;
			float tileX1 = center.x - xOffset - moveColliderHalfWidth;
			float tileX2 = center.x + xOffset + moveColliderHalfWidth;
			float tileY = center.y + moveCollider.bounds.size.y / 2f;
			switch (direction) {
				case CharacterMover.Direction.LEFT:
					tileX1 = -tileX1;
					tileX2 = -tileX2;
					DoesLineMeetParavolaInY(tileX1, tileX2, tileY);
					break;
				case CharacterMover.Direction.RIGHT:
					DoesLineMeetParavolaInY(tileX1, tileX2, tileY);
					break;
				case CharacterMover.Direction.NONE:

					break;
			}
		}
		return false;
	}

	bool DoesLineMeetParavolaInY(float lineX1, float lineX2, float y) {
		float value1, value2;
		value1 = Mathf.Sqrt((Mathf.Pow(linearCoefficient, 2f) - 4 * quadraticCoefficient * y) / 4 * Mathf.Pow(quadraticCoefficient, 2f));
		value2 = -value1;
		float extraTerm = linearCoefficient / (2 * quadraticCoefficient);
		value1 += extraTerm;
		value2 += extraTerm;
		return lineX1 <= value1 && value1 <= lineX2;
	}

	void Wonder() {
		CharacterMover.Direction direction = target.Direction;
		if (isInBorder) {
			direction = GetReverseDirection(direction);
			isInBorder = false;
		}
		target.WalkTo(direction);
		CanGoToHigher(direction);
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