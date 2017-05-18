using UnityEditor;

[CustomEditor(typeof(HpManager))]
public class HpManagerEditor : EditorFrame {

	//editor target
	HpManager hpManager;

	//HP value
	SerializedProperty baseMaxHp;
	Formula<int> maxHpFormula;

	protected override void OnEnable() {
		hpManager = target as HpManager;
		InitializeHpValue();
	}

	void InitializeHpValue() {
		baseMaxHp = serializedObject.FindProperty("baseMaxHp");
		maxHpFormula = hpManager.MaxHp;
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