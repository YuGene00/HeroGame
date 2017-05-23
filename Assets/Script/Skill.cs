using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill {

	//Skill Info
	string name = null;
	public string Name { get { return name; } }
	string detail = null;
	public string Detail { get { return detail; } }

	public void Run() {
		if (CanRun()) {
			RunEffect();
		}
	}

	protected abstract bool CanRun();

	protected abstract void RunEffect();
}