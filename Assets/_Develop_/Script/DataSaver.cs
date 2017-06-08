using UnityEngine;
using UnityEditor;

public class DataSaver : ScriptableObject {

	//path for asset
	const string path = "Assets/SaveFile.asset";

	//singleton
	static DataSaver instance = null;
	public static DataSaver Instance {
		get {
			if (!instance) {
				instance = AssetDatabase.LoadAssetAtPath<DataSaver>(path);
				if (!instance) {
					instance = CreateDataSaver();
				}
			}
			return instance;
		}
	}

	//current stage
	public int CurrentStage = 0;

	static DataSaver CreateDataSaver() {
		DataSaver dataSaver = CreateInstance<DataSaver>();
		AssetDatabase.CreateAsset(dataSaver, path);
		AssetDatabase.Refresh();
		return dataSaver;
	}
}