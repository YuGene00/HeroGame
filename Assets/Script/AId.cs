using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

	//check agro flag
	bool isAgro = false;

	//play AI flag
	public bool Play { get; set; }

	public void Initialize() {
		CoroutineDelegate.Instance.StartCoroutine(RunAi());
		Play = true;
	}

	IEnumerator RunAi() {
		while (true) {
			yield return null;
		}
	}



	public void SetAgro(bool isAgro) {
		this.isAgro = isAgro;
	}
}