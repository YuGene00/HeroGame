using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject {

	public abstract bool IsTrue();
}

public abstract class ScopingCondition : Condition {

	[SerializeField]
	protected FixedScope scope = null;
}