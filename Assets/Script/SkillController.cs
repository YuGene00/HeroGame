using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour {

	//passive level
	int passiveLevel;
	public int PassiveLevel {
		get { return passiveLevel; }
		set {
			passiveLevel = value;
			Player.Instance.RunPassive();
		}
	}

	//skill
	[SerializeField]
	Skill[] passiveSkills = null;
	[SerializeField]
	Skill uniqueSkill = null;
	[SerializeField]
	Skill ultimateSkill = null;

	void Awake() {
		InitializePassiveSkills();
		uniqueSkill.Initialize();
		ultimateSkill.Initialize();
	}

	void InitializePassiveSkills() {
		for (int i = 0; i < passiveSkills.Length; ++i) {
			passiveSkills[i].Initialize();
		}
	}

	public void RunPassive() {
		RunSkill(passiveSkills);
	}

	public void RunUnique() {
		RunSkill(uniqueSkill);
	}

	public void RunUltimate() {
		RunSkill(ultimateSkill);
	}
	
	#region void RunSkill(Skill[] skills/Skill skill)
	void RunSkill(Skill[] skills) {
		for (int i = 0; i < skills.Length; ++i) {
			skills[i].Run();
		}
	}

	void RunSkill(Skill skill) {
		skill.Run();
	}
	#endregion
}