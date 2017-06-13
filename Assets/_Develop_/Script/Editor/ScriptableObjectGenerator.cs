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
	const string scopeRoot = "Assets/Skill/_Develop_/Scope";
	const string aimingRoot = "Assets/Skill/_Develop_/Scope/Aiming";

	[MenuItem("ScriptableObj/Create")]
	static void CreateScriptableObj() {
		CreateSkill();
		CreateCondition();
		CreateEffector();
		CreateScope();
		CreateAiming();
	}

	static void CreateSkill() {
		AssetCreator<Skill>.CreateAssetAt(skillRoot);
		AssetCreator<AimingSkill>.CreateAssetAt(skillRoot);
	}

	static void CreateCondition() {
		AssetCreator<IsInScope>.CreateAssetAt(conditionRoot);
	}

	static void CreateEffector() {
		AssetCreator<AnimatePlayer>.CreateAssetAt(effectorRoot);
		AssetCreator<AnimatePlayerAsImmortal>.CreateAssetAt(effectorRoot);
		AssetCreator<DestroyClosestRock>.CreateAssetAt(effectorRoot);
		AssetCreator<EffectToPlayer>.CreateAssetAt(effectorRoot);
		AssetCreator<EffectAt>.CreateAssetAt(effectorRoot);
		AssetCreator<DamageAt>.CreateAssetAt(effectorRoot);
		AssetCreator<EndGame>.CreateAssetAt(effectorRoot);
	}

	static void CreateScope() {
		AssetCreator<InFrontOfPlayer>.CreateAssetAt(scopeRoot);
	}

	static void CreateAiming() {
		AssetCreator<RectAiming>.CreateAssetAt(aimingRoot);
	}

	class AssetCreator<T> where T : ScriptableObject {

		public static void CreateAssetAt(string root) {
			T asset = ScriptableObject.CreateInstance<T>();
			CreateAsset(root, asset);
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
}