using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover {

	//move speed
	Formula speed;
	public Formula Speed { get { return speed; } }

	Mover() {

	}

	public static Mover CreateByTarget(Transform target) {
		return null;
	}

	public void MoveTo(Vector2 direction) {

	}
}