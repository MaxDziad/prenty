using System;
using UnityEngine;

public class TimerSystem : MonoBehaviour, IGameSystem
{
	private TimeSpan _timeSinceBegin = TimeSpan.Zero;

	public void OnSceneStart()
	{
		_timeSinceBegin = new TimeSpan(DateTime.Now.Ticks);
	}

	public TimeSpan GetTimeSpan()
	{
		return new TimeSpan(DateTime.Now.Ticks) - _timeSinceBegin;
	}

	public void Initialize() { }
	public void Uninitialize() { }
}
