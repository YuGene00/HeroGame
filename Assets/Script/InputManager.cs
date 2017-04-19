using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

#if UNITY_STANDALONE_WIN
	//delegate for key function
	delegate void KeyFunction();

	//wait for keycode
	WaitUntil escapeWait = new WaitUntil(() => (Input.GetKey(KeyCode.Escape)));
	WaitUntil leftArrowWait = new WaitUntil(() => (Input.GetKey(KeyCode.LeftArrow)));
	WaitUntil leftArrowUpWait = new WaitUntil(() => (Input.GetKeyUp(KeyCode.LeftArrow)));
	WaitUntil rightArrowWait = new WaitUntil(() => (Input.GetKey(KeyCode.RightArrow)));
	WaitUntil rightArrowUpWait = new WaitUntil(() => (Input.GetKeyUp(KeyCode.RightArrow)));
	WaitUntil spaceWait = new WaitUntil(() => (Input.GetKey(KeyCode.Space)));

	void Awake() {
		StartCoroutine(BindFunctionToKeyWait(Application.Quit, escapeWait));
		StartCoroutine(BindFunctionToKeyWait(LeftArrow, leftArrowWait));
		StartCoroutine(BindFunctionToKeyWait(LeftArrowUp, leftArrowUpWait));
		StartCoroutine(BindFunctionToKeyWait(RightArrow, rightArrowWait));
		StartCoroutine(BindFunctionToKeyWait(RightArrowUp, rightArrowUpWait));
		StartCoroutine(BindFunctionToKeyWait(Space, spaceWait));
	}

	IEnumerator BindFunctionToKeyWait(KeyFunction keyFunction, WaitUntil keyWait) {
		while (true) {
			yield return keyWait;
			keyFunction();
		}
	}
#endif

	void LeftArrow() {
		Player.Instance.WalkTo(CharacterMover.Direction.LEFT);
	}

	void LeftArrowUp() {
		Player.Instance.Stop();
	}

	void RightArrow() {
		Player.Instance.WalkTo(CharacterMover.Direction.RIGHT);
	}

	void RightArrowUp() {
		Player.Instance.Stop();
	}

	void Space() {
		Player.Instance.Jump();
	}
}