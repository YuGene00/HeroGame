using UnityEditor;

[CustomEditor(typeof(HpController))]
public class HpControllerEditor : EditorFrame {

	//editor target
	HpController hpController;

	//HP value
	SerializedProperty baseMaxHp;
	Formula<int> maxHpFormula;

	protected override void OnEnable() {
		hpController = target as HpController;
		InitializeHpValue();
	}

	void InitializeHpValue() {
		baseMaxHp = serializedObject.FindProperty("baseMaxHp");
		maxHpFormula = hpController.MaxHp;
		maxHpFormula.SetBaseValue(baseMaxHp.intValue);
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawHp, AfterChangeHp);
	}

	void DrawHp() {
		EditorGUILayout.LabelField("HP");
		++EditorGUI.indentLevel;
		baseMaxHp.intValue = EditorGUILayout.IntField("기본 최대 체력", baseMaxHp.intValue);
		EditorGUILayout.LabelField("최종 최대 체력", maxHpFormula.Value.ToString());
		--EditorGUI.indentLevel;
	}

	void AfterChangeHp() {
		serializedObject.ApplyModifiedProperties();
		maxHpFormula.SetBaseValue(baseMaxHp.intValue);
	}
}