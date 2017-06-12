using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effector : ScriptableObject {

	public abstract void RunEffect();
}

public abstract class ScopingEffector : Effector {

	[SerializeField]
	protected Scope scope = null;
}