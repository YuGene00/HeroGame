using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Effector {

	public override void RunEffect() {
		Player.Instance.DelayStateOn();
		Player.Instance.IsImmortal = true;
		EventManager.Instance.Victory();
	}
}