using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons {
	public class EventMgr : MonoBehaviour {

		//singleton
		static EventMgr instance = null;
		public static EventMgr Instance { get { return instance; } }

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
}