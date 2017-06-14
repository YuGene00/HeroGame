using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	//singleton
	static UIManager instance = null;
	public static UIManager Instance { get { return instance; } }

	//default UI object
	[SerializeField]
	GameObject defaultUi = null;
	public GameObject DefaultUi { get { return defaultUi; } }

	//Stage Info
	[SerializeField]
	Text stageInfo = null;

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

	//controller
	[SerializeField]
	RectTransform controller = null;
	[SerializeField]
	RectTransform controllbar = null;
	float moveOffset;

	//Unique, Ultimate
	[SerializeField]
	GameObject uniqueSkill = null;
	[SerializeField]
	GameObject ultimateSkill = null;

	void Awake() {
		instance = this;
		stageInfo.text = SceneManager.GetActiveScene().name;
		moveOffset = (controllbar.sizeDelta.x - controller.sizeDelta.x) * 0.5f;
	}

	public void SetLifeHeart(int value) {
		for (int i = 0; i < lifeHeart.Length; ++i) {
			lifeHeart[i].SetActive(i < value);
		}
	}

	public void SetControllerDirection(Direction direction) {
		Vector2 controllerPos = Vector2.zero;
		switch (direction) {
			case Direction.NONE:
				controllerPos.x = 0f;
				break;
			case Direction.LEFT:
				controllerPos.x = -moveOffset;
				break;
			case Direction.RIGHT:
				controllerPos.x = moveOffset;
				break;
		}
		controller.anchoredPosition = controllerPos;
	}

	public void SwapToUltimate() {
		uniqueSkill.SetActive(false);
		ultimateSkill.SetActive(true);
	}
}