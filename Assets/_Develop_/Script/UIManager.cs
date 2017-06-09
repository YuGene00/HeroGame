using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	//singleton
	static UIManager instance = null;
	public static UIManager Instance { get { return instance; } }

	//Main Camera
	[SerializeField]
	Camera mainCamera = null;

	//default UI object
	[SerializeField]
	GameObject defaultUi = null;

	//rectArea
	[SerializeField]
	GameObject rectArea = null;
	Transform rectAreaTrans;
	Collider2D rectAreaCollider;
	public Collider2D RectAreaCollider { get { return rectAreaCollider; } }

	void Awake() {
		instance = this;
		rectAreaTrans = rectArea.transform;
		rectAreaCollider = rectArea.GetComponent<Collider2D>();
	}

	public void SetDefaultUiActive(bool value) {
		defaultUi.SetActive(value);
	}

	public void SetRectAreaUiActive(bool value) {
		rectArea.SetActive(value);
	}

	public void StartTouchTracingByRectArea() {
		StartCoroutine("RunTouchTracingByRectArea");
	}

	IEnumerator RunTouchTracingByRectArea() {
		while (Input.GetMouseButtonDown(0)) {
			rectAreaTrans.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			yield return null;
		}
	}
}