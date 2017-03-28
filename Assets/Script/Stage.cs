using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons {
	public class Stage : MonoBehaviour {

		//singleton
		static Stage instance = null;
		public static Stage Instance { get { return instance; } }

		void Awake() {
			instance = this;
		}

		public void LoadStage(int stageNo) {
			InitializeFromFile();
			InitializeCommon();
		}

		void InitializeFromFile() {

		}

		void InitializeCommon() {

		}

		public void Run() {
			EventManager.Instance.RemainTimer.Run();
		}

		public void Pause() {
			EventManager.Instance.RemainTimer.Pause();
		}
	}
}