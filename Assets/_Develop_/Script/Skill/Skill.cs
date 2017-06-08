using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject {

	//Skill Info
	[SerializeField]
	string skillName = null;
	public string Name { get { return skillName; } }
	[SerializeField]
	string detail = null;
	public string Detail { get { return detail; } }

	//Skill Process
	[SerializeField]
	protected Condition[] conditions;
	[SerializeField]
	protected Effector[] effectors;

	public abstract void Initialize();

	public void Run() {
		if (CanRun()) {
			RunEffect();
		}
	}

	bool CanRun() {
		for (int i = 0; i < conditions.Length; ++i) {
			if (!conditions[i].IsTrue()) {
				return false;
			}
		}
		return true;
	}

	protected virtual void RunEffect() {
		for (int i = 0; i < effectors.Length; ++i) {
			effectors[i].RunEffect();
		}
	}
}