using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	//delegate for key function
	delegate void KeyFunction();

	//wait for keycode
	WaitUntil escapeWait = new WaitUntil(() => Input.GetKey(KeyCode.Escape));

#if UNITY_STANDALONE_WIN
	//wait for window keycode
	WaitUntil leftArrowWait = new WaitUntil(() => Input.GetKey(KeyCode.LeftArrow));
	WaitUntil rightArrowWait = new WaitUntil(() => Input.GetKey(KeyCode.RightArrow));
	WaitUntil leftArrowUpWait = new WaitUntil(() => Input.GetKeyUp(KeyCode.LeftArrow));
	WaitUntil rightArrowUpWait = new WaitUntil(() => Input.GetKeyUp(KeyCode.RightArrow));
	WaitUntil spaceWait = new WaitUntil(() => Input.GetKey(KeyCode.Space));
#endif

	//flag for use controller
	static bool isUsingController = false;
	static TouchSupporter.ITouch controllerTouch;
	WaitUntil useControllerWait = new WaitUntil(() => isUsingController);
	WaitUntil controllerButtonUpWait = new WaitUntil(() => controllerTouch.IsInPhase(TouchPhase.Ended));

	//middle area of controller
	[SerializeField]
	RectTransform middleAreaOfController = null;
	float middleLeftEdge;
	float middleRightEdge;

	//flag for hold unique
	static bool isHoldingSkill = false;
	static TouchSupporter.ITouch holdingSkillTouch;
	float yBeforeHolding;
	WaitUntil holdingSkillWait = new WaitUntil(() => isHoldingSkill);
	WaitWhile holdingEndWait = new WaitWhile(() => isHoldingSkill);
	WaitForSeconds holdingTimeWait = new WaitForSeconds(0.3f);
	WaitUntil unholdingWait = new WaitUntil(() => holdingSkillTouch.IsInPhase(TouchPhase.Ended));
	readonly float slideDistance = Screen.height * 0.2f;

	void Awake() {
		CoroutineDelegate.Instance.StartCoroutine(BindFunctionToKeyWait(Application.Quit, escapeWait));
#if UNITY_STANDALONE_WIN
		InitializeWindowKeyBinding();
#endif
		StartControllerCoroutines();
		StartHoldingSkillCoroutines();
	}

	IEnumerator BindFunctionToKeyWait(KeyFunction keyFunction, WaitUntil keyWait) {
		while (true) {
			yield return keyWait;
			keyFunction();
		}
	}

#if UNITY_STANDALONE_WIN
	void InitializeWindowKeyBinding() {
		CoroutineDelegate.Instance.StartCoroutine(BindFunctionToKeyWait(LeftMove, leftArrowWait));
		CoroutineDelegate.Instance.StartCoroutine(BindFunctionToKeyWait(RightMove, rightArrowWait));
		CoroutineDelegate.Instance.StartCoroutine(BindFunctionToKeyWait(Stop, leftArrowUpWait));
		CoroutineDelegate.Instance.StartCoroutine(BindFunctionToKeyWait(Stop, rightArrowUpWait));
		CoroutineDelegate.Instance.StartCoroutine(BindFunctionToKeyWait(Jump, spaceWait));
	}

	void LeftMove() {
		MoveCharacter(Direction.LEFT);
	}

	void RightMove() {
		MoveCharacter(Direction.RIGHT);
	}

	void Stop() {
		MoveCharacter(Direction.NONE);
	}
#endif

	void StartControllerCoroutines() {
		CoroutineDelegate.Instance.StartCoroutine(RunController());
		CoroutineDelegate.Instance.StartCoroutine(StopController());
	}

	void StartHoldingSkillCoroutines() {
		CoroutineDelegate.Instance.StartCoroutine(RunHoldingSkill());
		CoroutineDelegate.Instance.StartCoroutine(RunHoldingTimeOut());
		CoroutineDelegate.Instance.StartCoroutine(RunUnholdingSkill());
	}

	void Start() {
		InitializeMiddleEdge();
	}

	IEnumerator RunController() {
		while (true) {
			yield return useControllerWait;
			if (!isUsingController) {
				continue;
			}
			MoveCharacter(GetControllerDirection());
		}
	}

	Direction GetControllerDirection() {
		if (controllerTouch.Position.x < middleLeftEdge) {
			return Direction.LEFT;
		} else if (middleRightEdge < controllerTouch.Position.x) {
			return Direction.RIGHT;
		} else {
			return Direction.NONE;
		}
	}

	IEnumerator StopController() {
		while (true) {
			yield return useControllerWait;
			yield return controllerButtonUpWait;
			isUsingController = false;
			MoveCharacter(Direction.NONE);
		}
	}

	void MoveCharacter(Direction direction) {
		UIManager.Instance.SetControllerDirection(direction);
		if (direction == Direction.NONE) {
			Player.Instance.Stop();
		} else {
			Player.Instance.WalkTo(direction);
		}
	}

	IEnumerator RunHoldingSkill() {
		while (true) {
			yield return holdingSkillWait;
			if (!isHoldingSkill) {
				continue;
			}
			yBeforeHolding = holdingSkillTouch.Position.y;
			yield return holdingEndWait;
		}
	}

	IEnumerator RunHoldingTimeOut() {
		while (true) {
			yield return holdingSkillWait;
			yield return holdingTimeWait;
			if (!isHoldingSkill) {
				continue;
			}
			isHoldingSkill = false;
			CalculateSlide();
		}
	}

	IEnumerator RunUnholdingSkill() {
		while (true) {
			yield return holdingSkillWait;
			yield return unholdingWait;
			if (!isHoldingSkill) {
				continue;
			}
			isHoldingSkill = false;
			CalculateSlide();
		}
	}

	void CalculateSlide() {
		if (slideDistance < holdingSkillTouch.Position.y - yBeforeHolding) {
			UIManager.Instance.SwapToUltimate();
		} else {
			Player.Instance.RunUnique();
		}
	}

	void InitializeMiddleEdge() {
		if (middleAreaOfController == null) {
			return;
		}

		float edgeOffset = middleAreaOfController.sizeDelta.x * 0.5f;
		float middleAreaX = middleAreaOfController.position.x;
		middleLeftEdge = middleAreaX - edgeOffset;
		middleRightEdge = middleAreaX + edgeOffset;
	}

	public void UseController() {
		if (isUsingController) {
			return;
		}

		controllerTouch = TouchSupporter.GetTouchInPhase(TouchPhase.Began);
		isUsingController = true;
	}

	public void HoldSkill() {
		if (isHoldingSkill) {
			return;
		}

		holdingSkillTouch = TouchSupporter.GetTouchInPhase(TouchPhase.Began);
		isHoldingSkill = true;
	}

	public void Jump() {
		Player.Instance.Jump();
	}

	public void RunUltimate() {
		Player.Instance.RunUltimate();
	}
}