using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTrigger : MonoBehaviour {

	//parent's main script
	[SerializeField]
	MainScript mainScript = null;
	public MainScript MainScript { get { return mainScript; } }
}