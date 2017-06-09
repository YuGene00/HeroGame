using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : Effector {

	//wait for touch up
	WaitUntil touchUpWait = new WaitUntil(() => Input.GetMouseButtonUp(0));

	//skill after aiming
	[SerializeField]
	ScopingSkill skillAfterAiming = null;

	public override void RunEffect() {
		UIManager.Instance.SetDefaultUiActive(false);
		UIManager.Instance.SetRectAreaUiActive(true);
		UIManager.Instance.StartTouchTracingByRectArea();
	}

	IEnumerator RunSkillAfterTouchUp() {
		yield return touchUpWait;
		Vector2 regionSize = UIManager.Instance.RectAreaCollider.bounds.size;
		Vector2 regionPos = (Vector2)UIManager.Instance.RectAreaCollider.bounds.center - regionSize * 0.5f;
		skillAfterAiming.SetRegion(new Rect(regionPos, regionSize));
		skillAfterAiming.Run();
	}
}