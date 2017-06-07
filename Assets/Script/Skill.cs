using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject {

	//Skill Info
	protected string skillName = null;
	public string Name { get { return skillName; } }
	protected string detail = null;
	public string Detail { get { return detail; } }

	//Skill Process
	[SerializeField]
	Condition[] conditions;
	[SerializeField]
	Effector[] effectors;

	public abstract void Initialize();

	public void Run() {
		if (CanRun()) {
			RunEffect();
		}
	}

	protected abstract bool CanRun();

	protected abstract void RunEffect();
}

public class BreakRock : Skill {

	//circle for check collider
	float xFromPlayer = 0f;
	float radius = 0f;

	protected override void Initialize() {
		skillName = "돌파괴";
		detail = "나 뿌순다 돌";
		InitializeCheckCircle();
	}

	void InitializeCheckCircle() {
		RockIsInFrontOfPlayer conditionForCheckCircle = ScriptableObject.CreateInstance<RockIsInFrontOfPlayer>();
		xFromPlayer = conditionForCheckCircle.XFromPlayer;
		radius = conditionForCheckCircle.Radius;
		ScriptableObject.Destroy(conditionForCheckCircle);
	}

	protected override bool CanRun() {
		//Physics2D.OverlapCircleAll
	}
}