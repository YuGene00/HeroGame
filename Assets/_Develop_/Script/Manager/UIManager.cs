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

	//Life heart
	[SerializeField]
	GameObject[] lifeHeart = null;

	//Pop up
	[SerializeField]
	GameObject pause = null;
	public GameObject Pause { get { return pause; } }
	[SerializeField]
	GameObject victory = null;
	public GameObject Victory { get { return victory; } }
	[SerializeField]
	GameObject defeat = null;
	public GameObject Defeat { get { return defeat; } }

	void Awake() {
		instance = this;
	}

	public void SetLifeHeart(int value) {
		for (int i = 0; i < lifeHeart.Length; ++i) {
			lifeHeart[i].SetActive(i < value);
		}
	}
}