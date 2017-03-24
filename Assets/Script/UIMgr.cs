using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons {
	public class UIMgr : MonoBehaviour {

		//singleton
		static UIMgr instance = null;
		public static UIMgr Instance { get { return instance; } }

		void Awake() {
			instance = this;
		}
	}
}