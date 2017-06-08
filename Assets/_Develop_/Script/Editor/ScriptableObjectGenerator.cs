using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectGenerator {

	//stringBuilder for create path;
	static StringBuilder pathString = new StringBuilder();

	//root for asset
	const string skillRoot = "Assets/Skill/_Develop_/Skill";
	const string conditionRoot = "Assets/Skill/_Develop_/Condition";
	const string effectorRoot = "Assets/Skill/_Develop_/Effector";

	[MenuItem("ScriptableObj/Create")]
	static void CreateScriptableObj() {
		CreateSkill();
		CreateCondition();
		CreateEffector();
	}

	static void CreateSkill() {
		BreakRock breakRock = ScriptableObject.CreateInstance<BreakRock>();
		CreateAsset(skillRoot, breakRock);
	}

	static void CreateCondition() {
		IsRockInFrontOfPlayer rockIsInFrontOfPlayer = ScriptableObject.CreateInstance<IsRockInFrontOfPlayer>();
		CreateAsset(conditionRoot, rockIsInFrontOfPlayer);
	}

	static void CreateEffector() {
		AnimatePlayer animatePlayer = ScriptableObject.CreateInstance<AnimatePlayer>();
		CreateAsset(effectorRoot, animatePlayer);
		AnimatePlayerAsImmortal animatePlayerAsImmortal = ScriptableObject.CreateInstance<AnimatePlayerAsImmortal>();
		CreateAsset(effectorRoot, animatePlayerAsImmortal);
		EffectToPlayer effectToPlayer = ScriptableObject.CreateInstance<EffectToPlayer>();
		CreateAsset(effectorRoot, effectToPlayer);
	}

	static void CreateAsset(string root, Object asset) {
		AssetDatabase.CreateAsset(asset, CreatePath(root, asset));
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