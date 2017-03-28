using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons {
	public class UIManager : MonoBehaviour {

		//singleton
		static UIManager instance = null;
		public static UIManager Instance { get { return instance; } }

		void Awake() {
			instance = this;
		}
	}
}