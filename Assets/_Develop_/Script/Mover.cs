using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour, IStat {

	//Rigidbody2D
	protected Rigidbody2D rigid;

	//move speed
	[SerializeField]
	float baseSpeed = 0f;
	Formula<float> speed = new Formula<float>();
	public Formula<float> Speed { get { return speed; } }

	protected void Awake() {
		rigid = GetComponent<Rigidbody2D>();
		InitializeStat();
	}

	public void InitializeStat() {
		speed.SetBaseValue(baseSpeed);
		speed.Clear();
	}

	public void MoveTo(Vector2 direction) {
		rigid.velocity = direction * speed.Value;
	}
}