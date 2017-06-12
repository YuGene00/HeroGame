using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	//singleton
	static UIManager instance = null;
	public static UIManager Instance { get { return instance; } }

	//default UI object
	[SerializeField]
	GameObject defaultUi = null;
	public GameObject DefaultUi { get { return defaultUi; } }

	//Rect Aiming
	[SerializeField]
	GameObject rectAiming = null;
	public GameObject RectAiming { get { return rectAiming; } }

	void Awake() {
		instance = this;
	}
}