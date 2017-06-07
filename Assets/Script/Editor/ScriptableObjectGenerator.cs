using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectGenerator {

	//stringBuilder for create path;
	static StringBuilder pathString = new StringBuilder();

	//root for asset
	const string skillRoot = "Assets/Skill";

	[MenuItem("ScriptableObj/Create")]
	static void CreateScriptableObj() {
		CreateSkill();
	}

	static void CreateSkill() {
		BreakRock breakRock = ScriptableObject.CreateInstance<BreakRock>();
		AssetDatabase.CreateAsset(breakRock, CreatePath(skillRoot, breakRock));
	}

	static void CreateCondition() {

	}

	static string CreatePath(string root, Object file) {
		pathString.Length = 0;
		pathString.Append(root);
		pathString.Append("/");
		pathString.Append(file.GetType().ToString());
		pathString.Append(".asset");
		return pathString.ToString();
	}
}
