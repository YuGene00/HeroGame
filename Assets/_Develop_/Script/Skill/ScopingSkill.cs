using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopingSkill : Skill {

	Rect region;

	public void SetRegion(Rect region) {
		this.region = region;
	}
}