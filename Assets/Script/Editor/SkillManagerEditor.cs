using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillManager))]
public class SkillManagerEditor : EditorFrame {

	//editor target
	SkillManager skillManager;

	//skill value
	int passiveLevel;

	protected override void OnEnable() {
		skillManager = target as SkillManager;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawSkill, AfterChangeSkill);
	}

	void DrawSkill() {
		EditorGUILayout.LabelField("스킬");
		++EditorGUI.indentLevel;
		EditorGUI.BeginDisabledGroup(!Application.isPlaying);
		passiveLevel = EditorGUILayout.IntSlider("패시브 레벨", skillManager.PassiveLevel, 0, 10);
		EditorGUI.EndDisabledGroup();
		--EditorGUI.indentLevel;
	}

	void AfterChangeSkill() {
		skillManager.PassiveLevel = passiveLevel;
	}
}