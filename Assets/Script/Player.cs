using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons {
	[RequireComponent(typeof(Animator))]
	public class Player : MonoBehaviour {

		//singleton
		static Player instance = null;
		public static Player Instance { get { return instance; } }

		void Awake() {
			instance = this;
		}

		
	}
}