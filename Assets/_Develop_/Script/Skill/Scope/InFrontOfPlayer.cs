using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFrontOfPlayer : FixedScope {

	public override Rect ScopingArea {
		get {
			Vector2 minPoint = Player.Instance.Position;
			if (Player.Instance.Direction == Direction.LEFT) {
				minPoint.x -= scopeSize.x;
			}
			return new Rect(minPoint, scopeSize);
		}
	}
}