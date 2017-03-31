using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mover {

	//target Transform
	Transform target;

	//move speed
	[SerializeField]
	float baseSpeed;
	Formula<float> speed = new Formula<float>();
	public Formula<float> Speed { get { return speed; } }

	Mover() {
		speed.SetBaseValue(baseSpeed);
	}

	public static Mover CreateByTarget(Transform target) {
		Mover mover = new Mover();
		mover.target = target;
		return mover;
	}

	public void MoveTo(Vector2 direction) {
		target.Translate(direction * speed.Value * Time.deltaTime);
	}
}