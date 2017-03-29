using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover {

	//target transform
	Transform target;

	//move speed
	Formula speed;
	public Formula Speed { get { return speed; } }

	Mover();

	public static Mover CreateByTarget(Transform target) {
		Mover mover = new Mover();
		mover.target = target;
		return mover;
	}

	public void MoveTo(Vector2 direction) {

	}
}