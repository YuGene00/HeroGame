using UnityEditor;

[CustomEditor(typeof(Attacker))]
public class AttackerEditor : EditorFrame {

	//editor target
	Attacker attacker;

	//Attack value
	SerializedProperty baseAtk;
	Formula<int> atkFormula;

	//Attack target Tag
	SerializedProperty targetTag;

	protected override void OnEnable() {
		attacker = target as Attacker;
		InitializeAtkValue();
	}

	void InitializeAtkValue() {
		baseAtk = serializedObject.FindProperty("baseAtk");
		atkFormula = attacker.Atk;
		atkFormula.SetBaseValue(baseAtk.intValue);
		targetTag = serializedObject.FindProperty("targetTag");
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
		targetTag.stringValue = EditorGUILayout.TextField("타겟 태그", targetTag.stringValue);
		--EditorGUI.indentLevel;
	}

	void AfterChangeAtk() {
		serializedObject.ApplyModifiedProperties();
		atkFormula.SetBaseValue(baseAtk.intValue);
	}
}