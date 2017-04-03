using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterEditor : EditorFrame {

	//delegate for changeCheck
	protected delegate void DrawInspectorMethod();

	//editor target
	Character character;

	//character move value
	SerializedProperty baseSpeed;
	Formula<float> speedFormula;
	SerializedProperty baseJumpPower;
	Formula<float> jumpFormula;

	//HP value
	SerializedProperty baseMaxHp;
	Formula<int> maxHpFormula;

	protected override void OnEnable() {
		character = target as Character;
		InitializeCharacterMove();
		InitializeHp();
	}

	void InitializeCharacterMove() {
		CharacterMover characterMover = character.CharacterMover;
		baseSpeed = serializedObject.FindProperty("characterMover.mover.baseSpeed");
		speedFormula = characterMover.Mover.Speed;
		baseJumpPower = serializedObject.FindProperty("characterMover.baseJumpPower");
		jumpFormula = characterMover.JumpPower;
	}

	void InitializeHp() {
		baseMaxHp = serializedObject.FindProperty("hpManager.baseMaxHp");
		maxHpFormula = character.HpManager.MaxHp;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawAndApply(DrawCharacterMove, AfterChangeCharacterMove);
		DrawAndApply(DrawHp, AfterChangeHp);
	}

	void DrawCharacterMove() {
		EditorGUILayout.LabelField("이동");
		++EditorGUI.indentLevel;
		baseSpeed.floatValue = EditorGUILayout.FloatField("기본 이동 속도", baseSpeed.floatValue);
		EditorGUILayout.LabelField("최종 이동 속도", speedFormula.Value.ToString());
		baseJumpPower.floatValue = EditorGUILayout.FloatField("기본 점프 파워", baseJumpPower.floatValue);
		EditorGUILayout.LabelField("최종 점프 파워", jumpFormula.Value.ToString());
		--EditorGUI.indentLevel;
	}

	void AfterChangeCharacterMove() {
		serializedObject.ApplyModifiedProperties();
		speedFormula.SetBaseValue(baseSpeed.floatValue);
		jumpFormula.SetBaseValue(baseJumpPower.floatValue);
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