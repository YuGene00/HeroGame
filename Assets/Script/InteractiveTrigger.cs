using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTrigger : MonoBehaviour {

	//parent's main script
	MainScript mainScript;
	public MainScript MainScript { get { return mainScript; } }

	void Awake() {
		mainScript = transform.parent.GetComponent<MainScript>();
	}
}