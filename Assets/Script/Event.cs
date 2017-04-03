using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent {

	void Run();
}

public class InitializeStage : IEvent {

	public void LoadStage(int stageNo) {

	}

	public void Run() {

	}
}

public class RunStage : IEvent {

	public void Run() {

	}

	public void Pause() {

	}
}

public class RemainTime : IEvent {

	//remain time
	float time;
	public float Time { get { return time; } }

	public void Run() {

	}

	public void Pause() {

	}
}

public class PlayerDie : IEvent {

	public void Run() {

	}
}

public class Result : IEvent {

	public void Run() {

	}

	public void IncreaseKillCount() {

	}

	public void IncreaseHitCount() {

	}
}