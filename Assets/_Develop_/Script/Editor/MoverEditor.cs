using UnityEditor;

[CustomEditor(typeof(Mover))]
public class MoverEditor : EditorFrame {

	//editor target
	Mover mover;

	//speed value
	SerializedProperty baseSpeed;
	Formula<float> speedFormula;

	protected override void OnEnable() {
		mover = target as Mover;
		InitializeMoverValue();
	}

	void InitializeMoverValue() {
		baseSpeed = serializedObject.FindProperty("baseSpeed");
		speedFormula = mover.Speed;
		speedFormula.SetBaseValue(baseSpeed.floatValue);
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawMove, AfterChangeMove);
	}

	void DrawMove() {
		EditorGUILayout.LabelField("이동");
		++EditorGUI.indentLevel;
		baseSpeed.floatValue = EditorGUILayout.FloatField("기본 이동 속도", baseSpeed.floatValue);
		EditorGUILayout.LabelField("최종 이동 속도", speedFormula.Value.ToString());
		--EditorGUI.indentLevel;
	}

	void AfterChangeMove() {
		serializedObject.ApplyModifiedProperties();
		speedFormula.SetBaseValue(baseSpeed.floatValue);
	}
}