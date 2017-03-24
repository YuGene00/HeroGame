using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {

	//original GameObject
	GameObject origin;

	//data structure
	int unitCount;
	Stack<GameObject> poolStack = new Stack<GameObject>();
	
	//parent for GameObject from ObjectPool
	Parent parent;
	struct Parent {
		public MonoBehaviour mono;
		public Transform trans;
	}

	public static ObjectPool CreateFor(GameObject gameObj, int unitCount = 20) {
		ObjectPool objPool = new ObjectPool();
		objPool.origin = gameObj;
		objPool.unitCount = unitCount;
		objPool.InitParent();
		objPool.AllocateMemory();
		return objPool;
	}

	void InitParent() {
		GameObject parentObj = new GameObject(origin.name);
		parent.mono = parentObj.GetComponent<MonoBehaviour>();
		parent.trans = parentObj.transform;
	}

	void AllocateMemory() {
		for (int i = 0; i < unitCount; ++i) {
			PushNewObject();
		}
	}

	void PushNewObject() {
		GameObject gameObj = CreateObject();
		gameObj.SetActive(false);
		poolStack.Push(gameObj);
	}

	GameObject CreateObject() {
		GameObject gameObj = Object.Instantiate(origin);
		gameObj.transform.SetParent(parent.trans);
		gameObj.AddComponent<Releaser>().ObjPool = this;
		return gameObj;
	}

	public GameObject Retain(Vector3 position = default(Vector3), Quaternion? rotation = null) {
		if (rotation == null) {
			rotation = Quaternion.identity;
		}
		GameObject gameObj = RetainObjectTo(position, rotation.Value);
		gameObj.SetActive(true);
		return gameObj;
	}

	GameObject RetainObjectTo(Vector3 position, Quaternion rotation) {
		if (poolStack.Count <= 0) {
			AllocateMemory();
		}
		GameObject gameObj = poolStack.Pop();
		Transform trans = gameObj.transform;
		trans.position = position;
		trans.rotation = rotation;
		return gameObj;
	}

	public static void Release(GameObject gameObj) {
		gameObj.GetComponent<Releaser>().Release();
	}

	public static void Release(GameObject gameObj, float time) {
		gameObj.GetComponent<Releaser>().Release(time);
	}

	public void ReturnToPool(GameObject gameObj) {
		gameObj.SetActive(false);
		poolStack.Push(gameObj);
	}

	public void ReturnToPool(GameObject gameObj, float time) {
		parent.mono.StartCoroutine(ReturnAfterTime(gameObj, time));
	}

	IEnumerator ReturnAfterTime(GameObject gameObj, float time) {
		yield return new WaitForSeconds(time);
		ReturnToPool(gameObj);
	}

	public class Releaser : MonoBehaviour {

		//gameObject which has this releaser
		GameObject gameObj;

		//objectPool which has this gameObjet
		ObjectPool objPool;
		public ObjectPool ObjPool { set { objPool = value; } }

		private void Awake() {
			gameObj = gameObject;
		}

		public void Release() {
			objPool.ReturnToPool(gameObj);
		}

		public void Release(float time) {
			objPool.ReturnToPool(gameObj, time);
		}
	}
}