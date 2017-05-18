using UnityEditor;

[CustomEditor(typeof(CharacterMover))]
public class CharacterMoverEditor : MoverEditor {

	//editor target
	CharacterMover characterMover;

	//jump value
	SerializedProperty baseJumpPower;
	Formula<float> jumpFormula;
	SerializedProperty speedInfluenceRatioToJump;

	protected override void OnEnable() {
		base.OnEnable();
		characterMover = target as CharacterMover;
		InitializeCharacterMoverValue();
	}

	void InitializeCharacterMoverValue() {
		baseJumpPower = serializedObject.FindProperty("baseJumpPower");
		jumpFormula = characterMover.JumpPower;
		speedInfluenceRatioToJump = serializedObject.FindProperty("speedInfluenceRatioToJump");
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawJump, AfterChangeJump);
	}

	void DrawJump() {
		EditorGUILayout.LabelField("점프");
		++EditorGUI.indentLevel;
		baseJumpPower.floatValue = EditorGUILayout.FloatField("기본 점프 파워", baseJumpPower.floatValue);
		EditorGUILayout.LabelField("최종 점프 파워", jumpFormula.Value.ToString());
		speedInfluenceRatioToJump.floatValue = EditorGUILayout.FloatField("점프에 미치는 속도비", speedInfluenceRatioToJump.floatValue);
		--EditorGUI.indentLevel;
	}

	void AfterChangeJump() {
		serializedObject.ApplyModifiedProperties();
		jumpFormula.SetBaseValue(baseJumpPower.floatValue);
	}
}