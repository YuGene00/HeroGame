using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillController))]
public class SkillControllerEditor : EditorFrame {

	//editor target
	SkillController skillController;

	//skill value
	int passiveLevel;
	SerializedProperty passiveSkills;
	SerializedProperty uniqueSkill;
	SerializedProperty ultimateSkill;

	protected override void OnEnable() {
		skillController = target as SkillController;
		InitializeHpValue();
	}

	void InitializeHpValue() {
		passiveSkills = serializedObject.FindProperty("passiveSkills");
		uniqueSkill = serializedObject.FindProperty("uniqueSkill");
		ultimateSkill = serializedObject.FindProperty("ultimateSkill");
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawSkill, AfterChangeSkill);
	}

	void DrawSkill() {
		EditorGUILayout.LabelField("스킬");
		++EditorGUI.indentLevel;
		EditorGUI.BeginDisabledGroup(!Application.isPlaying);
		passiveLevel = EditorGUILayout.IntSlider("패시브 레벨", skillController.PassiveLevel, 0, 10);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.PropertyField(passiveSkills, true);
		EditorGUILayout.PropertyField(uniqueSkill, true);
		EditorGUILayout.PropertyField(ultimateSkill, true);
		--EditorGUI.indentLevel;
	}

	void AfterChangeSkill() {
		if (skillController.PassiveLevel != passiveLevel) {
			skillController.PassiveLevel = passiveLevel;
		}
		serializedObject.ApplyModifiedProperties();
	}
}