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
	public CharacterMover.Direction ReachedBorder { get; set; }

	//Collider2D for parent's MoveCollider
	Collider2D moveCollider;

	//common constant for path calculate
	Vector2 halfSizeOfMoveCollider;

	//constant for paravola
	float cosPower;
	float halfBOfQuadraticFormula;
	Collider2D[] tileColliders;

	//variable for paravola related to jump power
	float aOfQuadraticFormula;

	//variable for vertical related to jump power
	float highestHeight;

	public void InitializeBy(Enemy target) {
		this.target = target;
		ReachedBorder = CharacterMover.Direction.NONE;
		moveCollider = this.target.transform.FindChild("MoveCollider").GetComponent<Collider2D>();
		InitializeConstantForPathCalcualte();
		InitializeVariableForPathCalcualteRelatedToJumpPower();
		Play = true;
		playWait = new WaitUntil(() => Play);
		CoroutineDelegate.Instance.StartCoroutine(RunAi());
	}

	public void InitializeConstantForPathCalcualte() {
		halfSizeOfMoveCollider = moveCollider.bounds.size * 0.5f;
		cosPower = Mathf.Pow(Mathf.Cos(CharacterMover.JUMPDEGREERADIAN), 2f);
		halfBOfQuadraticFormula = Mathf.Tan(CharacterMover.JUMPDEGREERADIAN) * 0.5f;
		InitializeTileColliders();
	}

	void InitializeTileColliders() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		tileColliders = new Collider2D[tiles.Length];
		for (int i = 0; i < tiles.Length; ++i) {
			tileColliders[i] = tiles[i].GetComponent<Collider2D>();
		}
	}

	public void InitializeVariableForPathCalcualteRelatedToJumpPower() {
		float v0Power = Mathf.Pow(target.CharacterMover.JumpPower.Value, 2f);
		float g = -Physics2D.gravity.y;
		aOfQuadraticFormula = -g / (2f * v0Power * cosPower);
		highestHeight = v0Power / (2f * g);
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
		Vector2 distanceToPlayer = Player.Instance.Position - target.Position;
		int jumpDirection = -1;
		if (0 < distanceToPlayer.y) {
			jumpDirection = GetPossibleJumpBy(distanceToPlayer);
		} else if (-halfSizeOfMoveCollider.y < distanceToPlayer.y) {
			jumpDirection = GetHorizontalJumpBy(distanceToPlayer);
		}

		if (jumpDirection == -1) {
			WalkBy(distanceToPlayer);
		} else {
			JumpTo((CharacterMover.Direction)jumpDirection);
		}
	}

	int GetPossibleJumpBy(Vector2 distanceToPlayer) {
		int jumpDirection = -1;
		if (Mathf.Abs(distanceToPlayer.x) <= halfSizeOfMoveCollider.x) {
			if (distanceToPlayer.x < 0) {
				jumpDirection = GetPossibleJumpAfterCheckFirstSecondThirdDirection(CharacterMover.Direction.NONE,
					CharacterMover.Direction.LEFT, CharacterMover.Direction.RIGHT);
			} else {
				jumpDirection = GetPossibleJumpAfterCheckFirstSecondThirdDirection(CharacterMover.Direction.NONE,
					CharacterMover.Direction.RIGHT, CharacterMover.Direction.LEFT);
			}
		} else {
			if (distanceToPlayer.x < 0) {
				jumpDirection = GetPossibleJumpAfterCheckFirstSecondThirdDirection(CharacterMover.Direction.LEFT,
					CharacterMover.Direction.RIGHT, CharacterMover.Direction.NONE);
			} else {
				jumpDirection = GetPossibleJumpAfterCheckFirstSecondThirdDirection(CharacterMover.Direction.RIGHT,
					CharacterMover.Direction.LEFT, CharacterMover.Direction.NONE);
			}
		}
		return jumpDirection;
	}

	int GetPossibleJumpAfterCheckFirstSecondThirdDirection(CharacterMover.Direction direction1, CharacterMover.Direction direction2, CharacterMover.Direction direction3) {
		if (CanGoToHigher(direction1)) {
			return (int)direction1;
		} else if (CanGoToHigher(direction2)) {
			return (int)direction2;
		} else if (CanGoToHigher(direction3)) {
			return (int)direction3;
		} else {
			return -1;
		}
	}

	bool CanGoToHorizontal(CharacterMover.Direction direction) {
		for (int i = 0; i < tileColliders.Length; ++i) {
			if (IsColliderInNearSpace(tileColliders[i], direction)) {
				return true;
			}
		}
		return false;
	}

	bool IsColliderInNearSpace(Collider2D collider, CharacterMover.Direction direction) {
		float tileY = collider.bounds.center.y + collider.bounds.size.y * 0.5f;
		float gapY = moveCollider.bounds.center.y - halfSizeOfMoveCollider.y - tileY;
		if (gapY < 0 || halfSizeOfMoveCollider.y <= gapY) {
			return false;
		}
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = collider.bounds.center.x - xOffset;
		float tileX2 = collider.bounds.center.x + xOffset;
		float spaceNearPlayerX1 = 0f;
		float spaceNearPlayerX2 = 0f;
		switch (direction) {
			case CharacterMover.Direction.LEFT:
				spaceNearPlayerX2 = moveCollider.bounds.center.x - halfSizeOfMoveCollider.x;
				spaceNearPlayerX1 = spaceNearPlayerX2 - moveCollider.bounds.size.x;
				break;
			case CharacterMover.Direction.RIGHT:
				spaceNearPlayerX1 = moveCollider.bounds.center.x + halfSizeOfMoveCollider.x;
				spaceNearPlayerX2 = spaceNearPlayerX1 + moveCollider.bounds.size.x;
				break;
		}
		return tileX1 < spaceNearPlayerX2 && spaceNearPlayerX1 < tileX2;
	}

	bool CanGoToHigher(CharacterMover.Direction direction) {
		for (int i = 0; i < tileColliders.Length; ++i) {
			if (IsColliderInJumpPath(tileColliders[i], direction)) {
				return true;
			}
		}
		return false;
	}

	bool IsColliderInJumpPath(Collider2D collider, CharacterMover.Direction direction) {
		Vector2 center = collider.bounds.center - moveCollider.bounds.center;
		float tileY = center.y + (collider.bounds.size.y + moveCollider.bounds.size.y) * 0.5f;
		if (tileY <= halfSizeOfMoveCollider.y) {
			return false;
		}
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = center.x - xOffset - halfSizeOfMoveCollider.x;
		float tileX2 = center.x + xOffset + halfSizeOfMoveCollider.x;
		switch (direction) {
			case CharacterMover.Direction.LEFT:
				return IsLineInParavolaAtY(-tileX2, -tileX1, tileY);
			case CharacterMover.Direction.RIGHT:
				return IsLineInParavolaAtY(tileX1, tileX2, tileY);
			default:
				return IsLineInVerticalAtY(tileX1, tileX2, tileY);
		}
	}

	bool IsLineInParavolaAtY(float lineX1, float lineX2, float y) {
		float determinationValue = Mathf.Pow(halfBOfQuadraticFormula, 2f) - aOfQuadraticFormula * -y;
		if (determinationValue <= 0) {
			return false;
		}

		float sqrtOfDeterminationValue = Mathf.Sqrt(determinationValue);
		float value = (-halfBOfQuadraticFormula - sqrtOfDeterminationValue) / aOfQuadraticFormula;
		return lineX1 <= value && value <= lineX2;
	}

	bool IsLineInVerticalAtY(float lineX1, float lineX2, float y) {
		return lineX1 < 0 && 0 < lineX2 && y < highestHeight;
	}

	void JumpTo(CharacterMover.Direction direction) {
		switch (direction) {
			case CharacterMover.Direction.NONE:
				target.Stop();
				break;
			default:
				target.WalkTo(direction);
				break;
		}
		target.Jump();
	}

	int GetHorizontalJumpBy(Vector2 distanceToPlayer) {
		int jumpDirection = -1;
		if (distanceToPlayer.x < 0) {
			if (ReachedBorder == CharacterMover.Direction.LEFT
				&& !CanGoToHorizontal(CharacterMover.Direction.LEFT)) {
				jumpDirection = (int)CharacterMover.Direction.LEFT;
			}
		} else {
			if (ReachedBorder == CharacterMover.Direction.RIGHT
				&& !CanGoToHorizontal(CharacterMover.Direction.RIGHT)) {
				jumpDirection = (int)CharacterMover.Direction.RIGHT;
			}
		}
		return jumpDirection;
	}

	void WalkBy(Vector2 distanceToPlayer) {
		if (distanceToPlayer.x < 0) {
			target.WalkTo(CharacterMover.Direction.LEFT);
		} else {
			target.WalkTo(CharacterMover.Direction.RIGHT);
		}
	}

	void Wonder() {
		CharacterMover.Direction direction = target.Direction;
		if (direction == ReachedBorder) {
			direction = GetReverseDirection(ReachedBorder);
		}
		target.WalkTo(direction);
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
}