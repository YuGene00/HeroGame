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
			InitFromFile();
			InitCommon();
		}

		void InitFromFile() {

		}

		void InitCommon() {

		}

		public void Run() {
			EventMgr.Instance.RemainTimer.Run();
		}

		public void Pause() {
			EventMgr.Instance.RemainTimer.Pause();
		}
	}
}