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

	public void InitializeBy(Transform target) {
		InitializeStat();
		speed.SetBaseValue(baseSpeed);
		this.target = target;
	}

	public void InitializeStat() {
		speed.Clear();
	}

	public void MoveTo(Vector2 direction) {
		target.Translate(direction * speed.Value * Time.deltaTime);
	}
}