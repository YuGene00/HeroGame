using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager {

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
	Skill[] passiveSkills;
	Skill uniqueSkill;
	Skill ultimateSkill;

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
		
	}

	void RunSkill(Skill skill) {
		skill.Run();
	}
	#endregion
}