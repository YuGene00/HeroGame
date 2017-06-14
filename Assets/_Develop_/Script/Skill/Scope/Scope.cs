using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scope : ScriptableObject {

	public abstract Rect ScopingArea { get; }
	public Collider2D[] GetCollidersInScope() {
		Rect scope = ScopingArea;
		return Physics2D.OverlapAreaAll(scope.min, scope.max);
	}
}

public abstract class FixedScope : Scope {

	[SerializeField]
	protected Vector2 scopeSize = Vector2.zero;
}

public abstract class AimingScope : Scope {

	//aiming Ui
	protected abstract GameObject AimingUi { get; }
	Transform aimingTrans = null;
	protected Transform AimingTrans {
		get {
			if (aimingTrans == null) {
				aimingTrans = AimingUi.transform;
			}
			return aimingTrans;
		}
	}

	//Main Camera
	Camera mainCamera = null;
	protected Camera MainCamera {
		get {
			if (mainCamera == null) {
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			}
			return mainCamera;
		}
	}

	//delegate for calling base
	public delegate void CallBaseEffect();

	public void StartAiming(CallBaseEffect callBaseEffect) {
		UIManager.Instance.DefaultUi.SetActive(false);
		AimingUi.SetActive(true);
		CoroutineDelegate.Instance.StartCoroutine(CallBaseAfterAiming(callBaseEffect));
	}

	IEnumerator CallBaseAfterAiming(CallBaseEffect callBaseEffect) {
		yield return CoroutineDelegate.Instance.StartCoroutine(RunAiming());
		callBaseEffect();
		//UIManager.Instance.DefaultUi.SetActive(true);
		AimingUi.SetActive(false);
	}

	protected abstract IEnumerator RunAiming();
}