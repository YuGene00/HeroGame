using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Ai : MonoBehaviour {

	//current action
	IAiAction action;

	//type of Actions
	Wonderer wonderer;
	PlayerTracer playerTracer;

	//flag for in border
	public Direction ReachedBorder { get { return wonderer.ReachedBorder; } set { wonderer.ReachedBorder = value; }	}

	//flag for play Ai
	public bool Play { get; set; }
	WaitUntil playWait;

	void Awake() {
		InitializeAction();
		Play = true;
		playWait = new WaitUntil(() => Play);
		StartCoroutine("RunAi");
	}

	void InitializeAction() {
		Enemy enemy = GetComponent<Enemy>();
		wonderer = Wonderer.CreateFor(enemy);
		playerTracer = PlayerTracer.CreateFor(enemy);
		action = wonderer;
	}

	IEnumerator RunAi() {
		while (true) {
			yield return playWait;
			action.Action();
		}
	}

	public void SetAgro(bool isAgro) {
		if (isAgro) {
			action = playerTracer;
		} else {
			action = wonderer;
		}
	}
}

public abstract class IAiAction {

	//Enemy
	protected Enemy enemy;

	public IAiAction(Enemy enemy) {
		this.enemy = enemy;
	}

	public abstract void Action();
}

public class Wonderer : IAiAction {

	//flag for in border
	public Direction ReachedBorder { get; set; }

	public static Wonderer CreateFor(Enemy enemy) {
		return new Wonderer(enemy);
	}

	Wonderer(Enemy enemy) : base(enemy) {
		ReachedBorder = Direction.NONE;
	}

	public override void Action() {
		Direction direction = enemy.Direction;
		if (direction == ReachedBorder) {
			direction = GetReverseDirection(ReachedBorder);
		}
		enemy.WalkTo(direction);
	}

	Direction GetReverseDirection(Direction direction) {
		switch (direction) {
			case Direction.LEFT:
				return Direction.RIGHT;
			default:
				return Direction.LEFT;
		}
	}
}

public class PlayerTracer : IAiAction {

	//CharacterMover
	CharacterMover characterMover;

	//type of move action
	struct MoveType {
		public MoveState moveState;
		public Direction direction;

		public MoveType(MoveState moveState, Direction direction) {
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
		public Direction direction;

		public PathType(HeightType heightType, Direction direction) {
			this.heightType = heightType;
			this.direction = direction;
		}
	}

	//directions for check
	readonly static Direction[] directions2 = new Direction[2];
	readonly static Direction[] directions3 = new Direction[3];

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
	static Collider2D[] tileColliders;

	//variable for paravola related to jump velocity
	float cosPower;
	float halfBOfQuadraticFormula;
	float aOfQuadraticFormula;

	//variable for vertical related to jump velocity
	float highestHeight;

	//variable for drop paravola related to speed
	float dropV0Power;

	public static PlayerTracer CreateFor(Enemy enemy) {
		return new PlayerTracer(enemy);
	}

	PlayerTracer(Enemy enemy) : base(enemy) {
		characterMover = enemy.GetComponent<CharacterMover>();
		InitializeConstantForPathCalcualte();
		InitializeVariableForPathCalcualteRelatedToJumpPower();
	}

	public static void InitializeTileColliders() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		tileColliders = new Collider2D[tiles.Length];
		for (int i = 0; i < tiles.Length; ++i) {
			tileColliders[i] = tiles[i].GetComponent<Collider2D>();
		}
	}

	public void InitializeConstantForPathCalcualte() {
		halfSizeOfMoveCollider = enemy.MoveCollider.bounds.size * 0.5f;
		quarterSizeOfMoveCollider = halfSizeOfMoveCollider * 0.5f;
	}

	public void InitializeVariableForPathCalcualteRelatedToJumpPower() {
		Vector2 jumpVector = characterMover.GetJumpVectorBy(Direction.RIGHT);
		float v0Power = jumpVector.sqrMagnitude;
		cosPower = Mathf.Pow(jumpVector.x / Mathf.Sqrt(v0Power), 2f);
		halfBOfQuadraticFormula = jumpVector.y / jumpVector.x * 0.5f;
		float g = -Physics2D.gravity.y;
		aOfQuadraticFormula = -g / (2f * v0Power * cosPower);
		highestHeight = v0Power / (2f * g);
		dropV0Power = Mathf.Pow(characterMover.Speed.Value, 2f);
	}

	public override void Action() {
		Vector2 distanceToPlayer = Player.Instance.Position - enemy.Position;
		MoveType moveType = new MoveType(MoveState.STAY, Direction.NONE);
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
		if (moveType.moveState == MoveState.STAY) {
			moveType = secondCheck(distanceToPlayer);
		}

		MoveCharacterBy(moveType);
	}

	MoveType GetMoveAfterCheckYBy(Vector2 distanceToPlayer) {
		if (GetHeightBy(distanceToPlayer.y) == HeightType.HIGHER) {
			return GetHigherJumpBy(distanceToPlayer);
		} else {
			return new MoveType(MoveState.STAY, Direction.NONE);
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

	MoveType GetHigherJumpBy(Vector2 distanceToPlayer) {
		Direction[] directions;
		if (Mathf.Abs(distanceToPlayer.x) <= halfSizeOfMoveCollider.x) {
			directions3[0] = Direction.NONE;
			if (distanceToPlayer.x < 0) {
				directions3[1] = Direction.LEFT;
				directions3[2] = Direction.RIGHT;
				directions = directions3;
			} else {
				directions3[1] = Direction.RIGHT;
				directions3[2] = Direction.LEFT;
				directions = directions3;
			}
		} else {
			if (distanceToPlayer.x < 0) {
				directions2[0] = Direction.LEFT;
				directions2[1] = Direction.NONE;
				directions = directions2;
			} else {
				directions2[0] = Direction.RIGHT;
				directions2[1] = Direction.NONE;
				directions = directions2;
			}
		}
		return GetPossibleJumpAfterCheckDirections(directions);
	}

	MoveType GetPossibleJumpAfterCheckDirections(Direction[] directions) {
		for (int i = 0; i < directions.Length; ++i) {
			if (CanJumpTo(new PathType(HeightType.HIGHER, directions[i]))) {
				return new MoveType(MoveState.JUMP, directions[i]);
			}
		}
		return new MoveType(MoveState.STAY, Direction.NONE);
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
		Vector2 center = collider.bounds.center - enemy.MoveCollider.bounds.center;
		float tileY = center.y + (collider.bounds.size.y + enemy.MoveCollider.bounds.size.y) * 0.5f;
		if (GetHeightBy(tileY) != pathType.heightType) {
			return false;
		}
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = center.x - xOffset - quarterSizeOfMoveCollider.x;
		float tileX2 = center.x + xOffset + quarterSizeOfMoveCollider.x;
		switch (pathType.direction) {
			case Direction.NONE:
				return IsLineInVerticalAtY(new Line(tileX1, tileX2), tileY);
			case Direction.LEFT:
				return IsLineInParavolaAtY(new Line(-tileX2, -tileX1), tileY);
			case Direction.RIGHT:
				return IsLineInParavolaAtY(new Line(tileX1, tileX2), tileY);
			default:
				return false;
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
		MoveType moveType = new MoveType(MoveState.STAY, Direction.NONE);
		for (int i = (int)HeightType.HORIZONTAL; (int)HeightType.LOWER <= i; --i) {
			if (moveType.moveState == MoveState.STAY
				&& i <= (int)GetHeightBy(distanceToPlayer.y)) {
				moveType = GetWalkOrJumpToHeightByDistanceToPlayer((HeightType)i, distanceToPlayer);
			}
		}
		return moveType;
	}

	MoveType GetWalkOrJumpToHeightByDistanceToPlayer(HeightType heightType, Vector2 distanceToPlayer) {
		MoveType moveType = new MoveType(MoveState.STAY, Direction.NONE);

		if (distanceToPlayer.x < 0) {
			moveType.direction = Direction.LEFT;
		} else {
			moveType.direction = Direction.RIGHT;
		}

		if (CanWalkTo(new PathType(heightType, moveType.direction))) {
			moveType.moveState = MoveState.WALK;
		} else if (CanJumpTo(new PathType(heightType, moveType.direction))) {
			moveType.moveState = MoveState.JUMP;
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
		float gapY = enemy.MoveCollider.bounds.center.y - halfSizeOfMoveCollider.y - tileY;
		if (GetHeightBy(-gapY) != pathType.heightType) {
			return false;
		}
		switch (pathType.heightType) {
			case HeightType.HORIZONTAL:
				return IsColliderInHorizontalWalkPath(collider, pathType.direction);
			case HeightType.LOWER:
				return IsColliderInLowerWalkPath(collider, pathType.direction);
			default:
				return false;
		}
	}

	bool IsColliderInHorizontalWalkPath(Collider2D collider, Direction direction) {
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = collider.bounds.center.x - xOffset;
		float tileX2 = collider.bounds.center.x + xOffset;
		float spaceNearPlayerX1 = 0f;
		float spaceNearPlayerX2 = 0f;
		switch (direction) {
			case Direction.LEFT:
				spaceNearPlayerX2 = enemy.MoveCollider.bounds.center.x;// - halfSizeOfMoveCollider.x;
				spaceNearPlayerX1 = spaceNearPlayerX2 - enemy.MoveCollider.bounds.size.x;
				break;
			case Direction.RIGHT:
				spaceNearPlayerX1 = enemy.MoveCollider.bounds.center.x;// + halfSizeOfMoveCollider.x;
				spaceNearPlayerX2 = spaceNearPlayerX1 + enemy.MoveCollider.bounds.size.x;
				break;
			default:
				return false;
		}
		return tileX1 < spaceNearPlayerX2 && spaceNearPlayerX1 < tileX2;
	}

	bool IsColliderInLowerWalkPath(Collider2D collider, Direction direction) {
		Vector2 center = collider.bounds.center - enemy.MoveCollider.bounds.center;
		float tileY = center.y + (collider.bounds.size.y + enemy.MoveCollider.bounds.size.y) * 0.5f;
		float xOffset = collider.bounds.size.x * 0.5f;
		float tileX1 = center.x - xOffset - quarterSizeOfMoveCollider.x;
		float tileX2 = center.x + xOffset + quarterSizeOfMoveCollider.x;
		switch (direction) {
			case Direction.LEFT:
				return IsLineInDropParavolaAtY(new Line(-tileX2, -tileX1), tileY);
			case Direction.RIGHT:
				return IsLineInDropParavolaAtY(new Line(tileX1, tileX2), tileY);
			default:
				return false;
		}
	}

	bool IsLineInDropParavolaAtY(Line line, float y) {
		float value = Mathf.Sqrt(y * 2f * dropV0Power / Physics2D.gravity.y);
		return line.x1 <= value && value <= line.x2;
	}

	void MoveCharacterBy(MoveType moveType) {
		if (moveType.moveState == MoveState.WALK) {
			enemy.WalkTo(moveType.direction);
		} else if (moveType.moveState == MoveState.JUMP) {
			JumpTo(moveType.direction);
		} else {
			enemy.Stop();
		}
	}

	void JumpTo(Direction direction) {
		switch (direction) {
			case Direction.NONE:
				enemy.Stop();
				break;
			case Direction.LEFT:
				enemy.WalkTo(Direction.LEFT);
				break;
			case Direction.RIGHT:
				enemy.WalkTo(Direction.RIGHT);
				break;
			default:
				return;
		}
		enemy.Jump();
	}
}