//order in layer
public enum OrderInLayer {
	BACKGROUND = -3,
	TERRAIN = -2,
	PLAYER = -1,
	ENEMY = 0,
	EFFECT = 1
}

//move direction
public enum Direction {
	NONE, LEFT, RIGHT
}

//move state
public enum MoveState {
	STAY, WALK, JUMP
}

//animation type
public enum AnimationType {
	NONE, STAY, WALK, JUMP, DAMAGED, DIE
}

//effect type
public enum EffectType {
	WALK, JUMP, LAND
}