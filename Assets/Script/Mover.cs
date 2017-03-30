using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover {

	//target Transform
	Transform target;

	//move speed
	Formula<float> speed;
	public Formula<float> Speed { get { return speed; } }

	Mover() { }

	public static Mover CreateByTarget(Transform target) {
		Mover mover = new Mover();
		mover.target = target;
		return mover;
	}

	public void MoveTo(Vector2 direction) {
		target.Translate(direction * speed.Value * Time.deltaTime);
	}
}