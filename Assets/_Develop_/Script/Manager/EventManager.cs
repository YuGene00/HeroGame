using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

	//singleton
	static EventManager instance = null;
	public static EventManager Instance { get { return instance; } }

	//event object
	//InitializeStage initializeStage = new InitializeStage();
	//public InitializeStage InitializeStage { get { return initializeStage; } }
	//RunStage runStage = new RunStage();
	//public RunStage RunStage { get { return runStage; } }
	//RemainTime remainTime = new RemainTime();
	//public RemainTime RemainTime { get { return remainTime; } }
	//PlayerDie playerDie = new PlayerDie();
	//public PlayerDie PlayerDie { get { return playerDie; } }
	//Result result = new Result();
	//public Result Result { get { return result; } }

	//Count;
	int killCount = 0;
	int hitCount = 0;

	//Next Stage
	[SerializeField]
	string nextStage = null;

	//wait after die
	WaitForSeconds endWait = new WaitForSeconds(2f);

	void Awake() {
		instance = this;
		PlayerTracer.InitializeTileColliders();
	}

	//void Start() {
	//	if (SceneManager.GetActiveScene().name != "MapEditor") {
	//		initializeStage.LoadStage(DataSaver.Instance.CurrentStage);
	//	}
	//	initializeStage.Run();
	//}

	public void SetPause(bool value) {
		UIManager.Instance.Pause.SetActive(value);
		if (value) {
			Time.timeScale = 0f;
		} else {
			Time.timeScale = 1f;
		}
	}

	public void IncreaseKillCount() {
		++killCount;
	}

	public void IncreaseHitCount() {
		++hitCount;
	}

	public void PlayerDie() {
		CoroutineDelegate.Instance.StartCoroutine(WaitAndSetPopUp(UIManager.Instance.Defeat));
	}

	IEnumerator WaitAndSetPopUp(GameObject popUp) {
		yield return endWait;
		Time.timeScale = 0f;
		popUp.SetActive(true);
	}

	public void Victory() {
		CoroutineDelegate.Instance.StartCoroutine(WaitAndSetPopUp(UIManager.Instance.Victory));
	}

	public void MoveToNext() {
		Time.timeScale = 1f;
		LoadScene(nextStage);
	}

	public void MoveToStage(string stage) {
		Time.timeScale = 1f;
		LoadScene(stage);
	}

	public void Restart() {
		Time.timeScale = 1f;
		LoadScene(SceneManager.GetActiveScene().name);
	}

	void LoadScene(string scene) {
		CoroutineDelegate.Instance.StopAllCoroutines();
		Time.timeScale = 1f;
		SceneManager.LoadScene(scene);
	}
}