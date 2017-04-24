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

	//type of move action
	struct MoveType {
		public CharacterMover.MoveState moveState;
		public CharacterMover.Direction direction;

		public MoveType(CharacterMover.MoveState moveState, CharacterMover.Direction direction) {
			this.moveState = moveState;
			this.direction = direction;
		}
	}

	//delegate for GetMoveAfterCheckX/Y
	delegate MoveType GetMoveAfterCheck(Vector2 distanceToPlayer);

	//type of path calculate
	enum HeightType {
		LOWER, HORIZONTAL, HIGHER
	}
	struct PathType {
		public HeightType heightType;
		public CharacterMover.Direction direction;

		public PathType(HeightType heightType, CharacterMover.Direction direction) {
			this.heightType = heightType;
			this.direction = direction;
		}
	}

	//directions for check
	readonly static CharacterMover.Direction[] directions2 = new CharacterMover.Direction[2];
	readonly static CharacterMover.Direction[] directions3 = new CharacterMover.Direction[3];

	//tile line for path calculate
	struct Line {
		public float x1, x2;

		public Line(float x1, float x2) {
			this.x1 = x1;
			this.x2 = x2;
		}
	}

	//common constant for path calculate
	Vector2 halfSizeOfMoveCollider;
	Vector2 quarterSizeOfMoveCollider;

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
		InitializeConstantForPathCalcualte();
		InitializeVariableForPathCalcualteRelatedToJumpPower();
		Play = true;
		playWait = new WaitUntil(() => Play);
		CoroutineDelegate.Instance.StartCoroutine(RunAi());
	}

	public void InitializeConstantForPathCalcualte() {
		halfSizeOfMoveCollider = target.MoveCollider.bounds.size * 0.5f;
		quarterSizeOfMoveCollider = halfSizeOfMoveCollider * 0.5f;
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
		MoveType moveType = new MoveType(CharacterMover.MoveState.STAY, CharacterMover.Direction.NONE);
		GetMoveAfterCheck firstCheck;
		GetMoveAfterCheck secondCheck;

		if (Mathf.Abs(distanceToPlayer.x) <= distanceToPlayer.y) {
			firstCheck = GetMoveAfterCheckYBy;
			secondCheck = GetMoveAfterCheckXBy;
		} else {
			firstCheck = GetMoveAfterCheckXBy;
			secondCheck = GetMoveAfterCheckYBy;
		}

		moveType = firstCheck(distanceToPlayer);
		if (moveType.moveState == CharacterMover.MoveState.STAY) {
			moveType = secondCheck(distanceToPlayer);
		}

		MoveCharacterBy(moveType);
	}

	MoveType GetMoveAfterCheckYBy(Vector2 distanceToPlayer) {
		if (GetHeightBy(distanceToPlayer.y) == HeightType.HIGHER) {
			return GetHigherJumpBy(distanceToPlayer);
		} else {
			return new MoveType(CharacterMover.MoveState.STAY, CharacterMover.Direction.NONE);
		}
	}

	MoveType GetHigherJumpBy(Vector2 distanceToPlayer) {
		CharacterMover.Direction[] directions;
		if (Mathf.Abs(distanceToPlayer.x) <= halfSizeOfMoveCollider.x) {
			directions3[0] = CharacterMover.Direction.NONE;
			if (distanceToPlayer.x < 0) {
				directions3[1] = CharacterMover.Direction.LEFT;
				directions3[2] = CharacterMover.Direction.RIGHT;
				directions = directions3;
			} else {
				directions3[1] = CharacterMover.Direction.RIGHT;
				directions3[2] = CharacterMover.Direction.LEFT;
				directions = directions3;
			}
		} else {
			if (distanceToPlayer.x < 0) {
				directions2[0] = CharacterMover.Direction.LEFT;
				directions2[1] = CharacterMover.Direction.NONE;
				directions = directions2;
			} else {
				directions2[0] = CharacterMover.Direction.RIGHT;
				directions2[1] = CharacterMover.Direction.NONE;
				directions = directions2;
			}
		}
		return GetPossibleJumpAfterCheckDirections(directions);
	}

	MoveType GetPossibleJumpAfterCheckDirections(CharacterMover.Direction[] directions) {
		for (int i = 0; i < directions.Length; ++i) {
			if (CanJumpTo(new PathType(HeightType.HIGHER, directions[i]))) {
				return new MoveType(CharacterMover.MoveState.JUMP, directions[i]);
			}
		}
		return new MoveType(CharacterMover.MoveState.STAY, CharacterMover.Direction.NONE);
	}

	bool CanJumpTo(PathType pathType) {
		for (int i = 0; i < tileColliders.Length; ++i) {
			if (IsColliderInJumpPath(tileColliders[i], pathType)) {
				return true;
			}
		}
		return false;
	}

	bool IsColliderInJumpPath(Collider2D collider, PathType pathType) {
		Vector2 center = collider.bounds.center - target.MoveCollider.bounds.center;
		float tileY = center.y + (collider.bounds.size.y + target.MoveCollider.bounds.size.y) * 0.5f;
		if (GetHeightBy(tileY) != pathType.heightType) {
			return false;
		}
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = center.x - xOffset - quarterSizeOfMoveCollider.x;
		float tileX2 = center.x + xOffset + quarterSizeOfMoveCollider.x;
		switch (pathType.direction) {
			case CharacterMover.Direction.NONE:
				return IsLineInVerticalAtY(new Line(tileX1, tileX2), tileY);
			case CharacterMover.Direction.LEFT:
				return IsLineInParavolaAtY(new Line(-tileX2, -tileX1), tileY);
			case CharacterMover.Direction.RIGHT:
				return IsLineInParavolaAtY(new Line(tileX1, tileX2), tileY);
			default:
				return false;
		}
	}

	HeightType GetHeightBy(float y) {
		if (y <= -quarterSizeOfMoveCollider.y) {
			return HeightType.LOWER;
		} else if (y < quarterSizeOfMoveCollider.y) {
			return HeightType.HORIZONTAL;
		} else {
			return HeightType.HIGHER;
		}
	}

	bool IsLineInVerticalAtY(Line line, float y) {
		return line.x1 < 0 && 0 < line.x2 && y < highestHeight;
	}

	bool IsLineInParavolaAtY(Line line, float y) {
		float determinationValue = Mathf.Pow(halfBOfQuadraticFormula, 2f) - aOfQuadraticFormula * -y;
		if (determinationValue <= 0) {
			return false;
		}

		float sqrtOfDeterminationValue = Mathf.Sqrt(determinationValue);
		float value = (-halfBOfQuadraticFormula - sqrtOfDeterminationValue) / aOfQuadraticFormula;
		return line.x1 <= value && value <= line.x2;
	}

	MoveType GetMoveAfterCheckXBy(Vector2 distanceToPlayer) {
		MoveType moveType = new MoveType(CharacterMover.MoveState.STAY, CharacterMover.Direction.NONE);
		for (int i = (int)HeightType.HORIZONTAL; (int)HeightType.LOWER <= i; --i) {
			if (moveType.moveState == CharacterMover.MoveState.STAY
				&& i <= (int)GetHeightBy(distanceToPlayer.y)) {
				moveType = GetWalkOrJumpToHeightByDistanceToPlayer((HeightType)i, distanceToPlayer);
			}
		}
		return moveType;
	}

	MoveType GetWalkOrJumpToHeightByDistanceToPlayer(HeightType heightType, Vector2 distanceToPlayer) {
		MoveType moveType = new MoveType(CharacterMover.MoveState.STAY, CharacterMover.Direction.NONE);

		if (distanceToPlayer.x < 0) {
			moveType.direction = CharacterMover.Direction.LEFT;
		} else {
			moveType.direction = CharacterMover.Direction.RIGHT;
		}

		if (CanWalkTo(new PathType(heightType, moveType.direction))) {
			moveType.moveState = CharacterMover.MoveState.WALK;
		} else if (CanJumpTo(new PathType(heightType, moveType.direction))) {
			moveType.moveState = CharacterMover.MoveState.JUMP;
		}

		return moveType;
	}

	bool CanWalkTo(PathType pathType) {
		for (int i = 0; i < tileColliders.Length; ++i) {
			if (IsColliderInWalkPath(tileColliders[i], pathType)) {
				return true;
			}
		}
		return false;
	}

	bool IsColliderInWalkPath(Collider2D collider, PathType pathType) {
		float tileY = collider.bounds.center.y + collider.bounds.size.y * 0.5f;
		float gapY = target.MoveCollider.bounds.center.y - halfSizeOfMoveCollider.y - tileY;
		if (GetHeightBy(-gapY) != pathType.heightType) {
			return false;
		}
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = collider.bounds.center.x - xOffset;
		float tileX2 = collider.bounds.center.x + xOffset;
		float spaceNearPlayerX1 = 0f;
		float spaceNearPlayerX2 = 0f;
		switch (pathType.direction) {
			case CharacterMover.Direction.LEFT:
				spaceNearPlayerX2 = target.MoveCollider.bounds.center.x;// - halfSizeOfMoveCollider.x;
				spaceNearPlayerX1 = spaceNearPlayerX2 - target.MoveCollider.bounds.size.x;
				break;
			case CharacterMover.Direction.RIGHT:
				spaceNearPlayerX1 = target.MoveCollider.bounds.center.x;// + halfSizeOfMoveCollider.x;
				spaceNearPlayerX2 = spaceNearPlayerX1 + target.MoveCollider.bounds.size.x;
				break;
		}
		return tileX1 < spaceNearPlayerX2 && spaceNearPlayerX1 < tileX2;
	}

	void MoveCharacterBy(MoveType moveType) {
		if (moveType.moveState == CharacterMover.MoveState.WALK) {
			target.WalkTo(moveType.direction);
		} else if (moveType.moveState == CharacterMover.MoveState.JUMP) {
			JumpTo(moveType.direction);
		} else {
			target.Stop();
		}
	}

	void JumpTo(CharacterMover.Direction direction) {
		switch (direction) {
			case CharacterMover.Direction.NONE:
				target.Stop();
				break;
			case CharacterMover.Direction.LEFT:
				target.WalkTo(CharacterMover.Direction.LEFT);
				break;
			case CharacterMover.Direction.RIGHT:
				target.WalkTo(CharacterMover.Direction.RIGHT);
				break;
			default:
				return;
		}
		target.Jump();
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