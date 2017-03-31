using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor {

	//character move value
	SerializedProperty baseSpeed;
	SerializedProperty jumpPower;

	void OnEnable() {
		baseSpeed = serializedObject.FindProperty("characterMover.mover.baseSpeed");
		jumpPower = serializedObject.FindProperty("characterMover.jumpPower");
	}

	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();
		serializedObject.Update();
		DrawInspector();
		if (EditorGUI.EndChangeCheck()) {
			serializedObject.ApplyModifiedProperties();
			AfterChangeInspector();
		}
	}

	void DrawInspector() {
		baseSpeed.floatValue = EditorGUILayout.FloatField("기본 이동속도", baseSpeed.floatValue);
		EditorGUILayout.LabelField("최종 이동속도", (target as Player).characterMover.mover.Speed.Value.ToString());
		jumpPower.floatValue = EditorGUILayout.FloatField("점프 파워", jumpPower.floatValue);
	}

	void AfterChangeInspector() {
		(target as Player).characterMover.mover.Speed.SetBaseValue(baseSpeed.floatValue);
	}
}