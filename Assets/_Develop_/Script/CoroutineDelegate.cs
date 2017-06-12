using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineDelegate : MonoBehaviour {

	//singleton
	static CoroutineDelegate instance = null;
	public static CoroutineDelegate Instance {
		get {
			if (!instance) {
				GameObject gameObj = new GameObject("CoroutineDelegate");
				instance = gameObj.AddComponent<CoroutineDelegate>();
				DontDestroyOnLoad(instance);
			}
			return instance;
		}
	}

	/*void Awake() {
		DontDestroyOnLoad(this);
	}*/

	public new void StopCoroutine(Coroutine routine) {
		if (routine == null) {
			return;
		}

		base.StopCoroutine(routine);
	}

	class MicroCoroutine {

		//list of coroutine
		List<IEnumerator> coroutines = new List<IEnumerator>();

		public void AddCoroutine(IEnumerator enumerator) {
			coroutines.Add(enumerator);
		}

		public void Run() {
			for (int i = 0; i < coroutines.Count; ++i) {
				if (!coroutines[i].MoveNext()) {
					coroutines.RemoveAt(i);
					continue;
				}
			}
		}
	}
}