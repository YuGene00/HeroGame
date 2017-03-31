using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager {

	//passive skills
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