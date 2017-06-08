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
	WaitUntil rightArrowWait = new WaitUntil(() => (Input.GetKey(KeyCode.RightArrow)));
	WaitUntil leftArrowUpWait = new WaitUntil(() => (Input.GetKeyUp(KeyCode.LeftArrow)));
	WaitUntil rightArrowUpWait = new WaitUntil(() => (Input.GetKeyUp(KeyCode.RightArrow)));
	WaitUntil spaceWait = new WaitUntil(() => (Input.GetKey(KeyCode.Space)));
	WaitUntil zWait = new WaitUntil(() => (Input.GetKey(KeyCode.Z)));

	void Awake() {
		StartCoroutine(BindFunctionToKeyWait(Application.Quit, escapeWait));
		StartCoroutine(BindFunctionToKeyWait(LeftMove, leftArrowWait));
		StartCoroutine(BindFunctionToKeyWait(RightMove, rightArrowWait));
		StartCoroutine(BindFunctionToKeyWait(Stop, leftArrowUpWait));
		StartCoroutine(BindFunctionToKeyWait(Stop, rightArrowUpWait));
		StartCoroutine(BindFunctionToKeyWait(Jump, spaceWait));
		StartCoroutine(BindFunctionToKeyWait(RunUnique, zWait));
	}

	IEnumerator BindFunctionToKeyWait(KeyFunction keyFunction, WaitUntil keyWait) {
		while (true) {
			yield return keyWait;
			keyFunction();
		}
	}
#endif

	void LeftMove() {
		Player.Instance.WalkTo(Direction.LEFT);
	}

	void RightMove() {
		Player.Instance.WalkTo(Direction.RIGHT);
	}

	void Stop() {
		Player.Instance.Stop();
	}

	void Jump() {
		Player.Instance.Jump();
	}

	void RunUnique() {
		Player.Instance.RunUnique();
	}
}