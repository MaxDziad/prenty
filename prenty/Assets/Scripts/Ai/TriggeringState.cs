using System;
using UnityEngine;

public class TriggeringState : AbstractAiState
{
	public event Action OnAlertFinishedEvent;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Material _normalMaterial;

	private readonly int _alertTrigger = Animator.StringToHash("Alert");

	public override void OnStart()
	{
		_agentSpider.Agent.isStopped = true;
		_animator.enabled = true;
		_animator.SetTrigger(_alertTrigger);
		_spriteRenderer.material = _normalMaterial;
	}

	public void SignalAlertFinish()
	{
		OnAlertFinishedEvent?.Invoke();
	}
}
