using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

	//singleton
	static EventManager instance = null;
	public static EventManager Instance { get { return instance; } }

	//event object
	InitializeStage initializeStage = new InitializeStage();
	public InitializeStage InitializeStage { get { return initializeStage; } }
	RunStage runStage = new RunStage();
	public RunStage RunStage { get { return runStage; } }
	RemainTime remainTime = new RemainTime();
	public RemainTime RemainTime { get { return remainTime; } }
	PlayerDie playerDie = new PlayerDie();
	public PlayerDie PlayerDie { get { return playerDie; } }
	Result result = new Result();
	public Result Result { get { return result; } }

	void Awake() {
		instance = this;
	}

	void Start() {
		if (SceneManager.GetActiveScene().name != "MapEditor") {
			initializeStage.LoadStage(DataSaver.Instance.CurrentStage);
		}
		initializeStage.Run();
	}
}