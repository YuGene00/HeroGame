using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSupporter {

	//define of Touch Id
	public enum TouchType {
		NOT_VALID = -1,
		MOUSE = -2
	}

	public static ITouch GetTouchInPhase(TouchPhase touchPhase) {
		int touchId = (int)TouchType.NOT_VALID;
		for (int i = 0; i < Input.touchCount; ++i) {
			UnityEngine.Touch touch = Input.GetTouch(i);
			if (touch.phase == touchPhase) {
				touchId = touch.fingerId;
			}
		}
		if (touchId == (int)TouchType.NOT_VALID && IsMouseInPhase(touchPhase)) {
			touchId = (int)TouchType.MOUSE;
		}
		return new Touch(touchId);
	}

	static bool IsMouseInPhase(TouchPhase touchPhase) {
		switch (touchPhase) {
			case TouchPhase.Began:
				return Input.GetMouseButtonDown(0);
			case TouchPhase.Ended:
				return Input.GetMouseButtonUp(0);
		}
		return false;
	}

	public interface ITouch {

		bool IsValid { get; }
		Vector2 Position { get; }
		bool IsInPhase(TouchPhase touchPhase);
	}

	struct Touch : ITouch {

		//identification for touch
		int touchId;
		public bool IsValid { get { return touchId != (int)TouchType.NOT_VALID; } }

		//touch position
		public Vector2 Position {
			get {
				switch (touchId) {
					case (int)TouchType.NOT_VALID:
						return Vector2.zero;
					case (int)TouchType.MOUSE:
						return Input.mousePosition;
					default:
						return Input.GetTouch(touchId).position;
				}
			}
		}

		public Touch(int touchId) {
			this.touchId = touchId;
		}

		public bool IsInPhase(TouchPhase touchPhase) {
			switch (touchId) {
				case (int)TouchType.NOT_VALID:
					return false;
				case (int)TouchType.MOUSE:
					return IsMouseInPhase(touchPhase);
				default:
					return Input.GetTouch(touchId).phase == touchPhase;
			}
		}
	}
}