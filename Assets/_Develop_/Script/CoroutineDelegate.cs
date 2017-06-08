using UnityEngine;

public class CoroutineDelegate : MonoBehaviour {

	//singleton
	static CoroutineDelegate instance = null;
	public static CoroutineDelegate Instance {
		get {
			if (!instance) {
				GameObject gameObj = new GameObject("CoroutineDelegate");
				instance = gameObj.AddComponent<CoroutineDelegate>();
			}
			return instance;
		}
	}

	void Awake() {
		DontDestroyOnLoad(this);
	}
}