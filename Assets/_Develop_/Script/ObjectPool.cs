using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {

	class ParentMonobehavior : MonoBehaviour { }

	//original GameObject
	GameObject origin;
	public GameObject Origin { get { return origin; } }

	//data structure
	int extendCount;
	Stack<GameObject> poolStack = new Stack<GameObject>();

	//parent for GameObject from ObjectPool
	Parent parent;
	struct Parent {
		public MonoBehaviour mono;
		public Transform trans;
	}

	public static ObjectPool CreateFor(GameObject gameObj, int initializeCount = 20, int extendCount = 10) {
		ObjectPool objPool = new ObjectPool();
		objPool.origin = gameObj;
		objPool.extendCount = extendCount;
		objPool.InitializeParent();
		objPool.AllocateMemoryTo(initializeCount);
		return objPool;
	}

	void InitializeParent() {
		GameObject parentObj = new GameObject(origin.name);
		parent.mono = parentObj.AddComponent<ParentMonobehavior>();
		parent.trans = parentObj.transform;
	}

	void AllocateMemoryTo(int allocateCount) {
		for (int i = 0; i < allocateCount; ++i) {
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
		if (poolStack.Count <= 0) {
			AllocateMemoryTo(extendCount);
		}
		GameObject gameObj = poolStack.Pop();
		SetObjectPositionAt(gameObj, position, rotation);
		gameObj.SetActive(true);
		return gameObj;
	}

	GameObject SetObjectPositionAt(GameObject gameObj, Vector3 position, Quaternion? rotation) {
		if (rotation == null) {
			rotation = Quaternion.identity;
		}
		Transform trans = gameObj.transform;
		trans.position = position;
		trans.rotation = rotation.Value;
		return gameObj;
	}

	public static void Release(GameObject gameObj) {
		Releaser releaser = gameObj.GetComponent<Releaser>();
		if (releaser) {
			releaser.Release();
		} else {
			MonoBehaviour.Destroy(gameObj);
		}
	}

	public static void Release(GameObject gameObj, float time) {
		Releaser releaser = gameObj.GetComponent<Releaser>();
		if (releaser) {
			releaser.Release(time);
		} else {
			MonoBehaviour.Destroy(gameObj, time);
		}
	}

	public void ReturnToPool(GameObject gameObj) {
		Transform trans = gameObj.transform;
		if (trans.parent != parent.trans) {
			trans.SetParent(parent.trans);
		}
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

	class Releaser : MonoBehaviour {

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