using UnityEditor;

public class ThornbushEditor : EditorFrame {

	//editor target
	Thornbush thornbush;

	//Attack value
	SerializedProperty baseAtk;
	Formula<int> atkFormula;

	protected override void OnEnable() {
		InitializeAtk();
	}

	void InitializeAtk() {
		baseAtk = serializedObject.FindProperty("Attacker.baseAtk");
		atkFormula = thornbush.Attacker.Atk;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawAtk, AfterChangeAtk);
	}

	void DrawAtk() {
		EditorGUILayout.LabelField("공격력");
		++EditorGUI.indentLevel;
		baseAtk.intValue = EditorGUILayout.IntField("기본 공격력", baseAtk.intValue);
		EditorGUILayout.LabelField("최종 공격력", atkFormula.Value.ToString());
		--EditorGUI.indentLevel;
	}

	void AfterChangeAtk() {
		serializedObject.ApplyModifiedProperties();
		atkFormula.SetBaseValue(baseAtk.intValue);
	}
}