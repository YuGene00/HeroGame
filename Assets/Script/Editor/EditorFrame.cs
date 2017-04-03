using UnityEditor;

public abstract class EditorFrame : Editor {

	//delegate for changeCheck
	protected delegate void DrawInspectorMethod();

	protected abstract void OnEnable();

	public override void OnInspectorGUI() {
		serializedObject.Update();
	}

	protected void DrawAndApply(DrawInspectorMethod drawMethod, DrawInspectorMethod afterChange) {
		EditorGUI.BeginChangeCheck();
		drawMethod();
		if (EditorGUI.EndChangeCheck()) {
			afterChange();
		}
	}
}