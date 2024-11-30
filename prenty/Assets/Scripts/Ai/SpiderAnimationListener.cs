using UnityEngine;

public class SpiderAnimationListener : MonoBehaviour
{
	[SerializeField]
	private TriggeringState _triggeringState;

	// Animation Event Finished
	public void AlertFinished()
	{
		_triggeringState.SignalAlertFinish();
	}
}
