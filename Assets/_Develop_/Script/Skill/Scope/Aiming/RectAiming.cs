using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectAiming : AimingScope {

	//Aiming Ui
	protected override GameObject AimingUi { get { return UIManager.Instance.RectAiming; } }

	//rectArea
	Collider2D rectAreaCollider = null;
	Collider2D RectAreaCollider {
		get {
			if (rectAreaCollider == null) {
				rectAreaCollider = AimingUi.GetComponent<Collider2D>();
			}
			return rectAreaCollider;
		}
	}

	//scope
	public override Rect ScopingArea {
		get {
			Vector2 regionSize = RectAreaCollider.bounds.size;
			Vector2 regionPos = (Vector2)RectAreaCollider.bounds.center - regionSize * 0.5f;
			return new Rect(regionPos, regionSize);
		}
	}

	protected override IEnumerator RunAiming() {
		TouchSupporter.ITouch aimingTouch = TouchSupporter.GetTouchInPhase(TouchPhase.Began);
		while (!aimingTouch.IsInPhase(TouchPhase.Ended)) {
			AimingTrans.position = (Vector2)MainCamera.ScreenToWorldPoint(aimingTouch.Position);
			yield return null;
		}
	}
}