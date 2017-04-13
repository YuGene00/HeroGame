using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

	//play Ai flag
	public bool Play { get; set; }
	WaitUntil playWait;

	//check agro flag
	bool isAgro = false;

	//in border flag
	bool isInBorder = false;

	public void Initialize() {
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
		
	}

	public void SetAgro(bool isAgro) {
		this.isAgro = isAgro;
	}

	public void ReachToBorder() {
		isInBorder = true;
	}
}