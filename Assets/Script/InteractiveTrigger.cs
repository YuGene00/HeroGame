using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTrigger : MonoBehaviour {

	//parent's main script
	[SerializeField]
	MainScript mainScript;
	public MainScript MainScript { get { return mainScript; } }
}