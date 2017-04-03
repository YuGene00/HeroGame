using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : CharacterEditor {

	//editor target
	Player player;

	//skill value
	int passiveLevel;

	protected override void OnEnable() {
		base.OnEnable();
		player = target as Player;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawSkill, AfterChangeSkill);
	}

	void DrawSkill() {
		EditorGUILayout.LabelField("스킬");
		++EditorGUI.indentLevel;
		EditorGUI.BeginDisabledGroup(!Application.isPlaying);
		passiveLevel = EditorGUILayout.IntSlider("패시브 레벨", player.SkillManager.PassiveLevel, 0, 10);
		EditorGUI.EndDisabledGroup();
		--EditorGUI.indentLevel;
	}

	void AfterChangeSkill() {
		player.SkillManager.PassiveLevel = passiveLevel;
	}
}