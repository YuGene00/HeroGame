using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillController))]
public class SkillControllerEditor : EditorFrame {

	//editor target
	SkillController skillController;

	//skill value
	int passiveLevel;

	protected override void OnEnable() {
		skillController = target as SkillController;
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
		--EditorGUI.indentLevel;
	}

	void AfterChangeSkill() {
		skillController.PassiveLevel = passiveLevel;
	}
}