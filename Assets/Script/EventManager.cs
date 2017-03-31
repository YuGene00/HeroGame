using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	//singleton
	static EventManager instance = null;
	public static EventManager Instance { get { return instance; } }

	//event object
	RemainTimer remainTimer;
	public RemainTimer RemainTimer { get { return remainTimer; } }
	PlayerDie playerDie;
	public PlayerDie PlayerDie { get { return playerDie; } }
	Result result;
	public Result Result { get { return result; } }

	void Awake() {
		instance = this;
	}
}