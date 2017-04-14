using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

	//target Enemy
	Enemy target;

	//flag for play Ai
	public bool Play { get; set; }
	WaitUntil playWait;

	//flag for check agro
	bool isAgro = false;

	//flag for in border
	bool isInBorder = false;

	public void InitializeBy(Enemy target) {
		this.target = target;
		Play = true;
		playWait = new WaitUntil(() => Play);
		CoroutineDelegate.Instance.StartCoroutine(RunAi());
	}

	IEnumerator RunAi() {
		while (true) {
			yield return playWait;
			if (isAgro) {
				TracePlayer();
			} else {
				Wonder();
			}
		}
	}

	void TracePlayer() {
		
	}

	void Wonder() {
		CharacterMover.Direction direction = target.Direction;
		if (isInBorder) {
			//direction = GetReverseDirection(direction);
			isInBorder = false;
		}
		target.WalkTo(direction);
	}

	CharacterMover.Direction GetReverseDirection(CharacterMover.Direction direction) {
		switch (direction) {
			case CharacterMover.Direction.LEFT:
				return CharacterMover.Direction.RIGHT;
			default:
				return CharacterMover.Direction.LEFT;
		}
	}

	public void SetAgro(bool isAgro) {
		this.isAgro = isAgro;
	}

	public void ReachToBorder() {
		isInBorder = true;
	}
}