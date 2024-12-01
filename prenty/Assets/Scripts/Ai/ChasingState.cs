using System;
using System.Collections;
using UnityEngine;

public class ChasingState : AbstractAiState
{
	public event Action OnChaseFinishedEvent;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private float _speed = 3f;

	[SerializeField]
	private float _acceleraation = 8f;

	[SerializeField]
	private float _chaseCheckTime = 3f;

	[SerializeField]
	private float _fadeDuration = 1.0f;

	private float _currentTime;
	private Coroutine _chaseHandle;
	private Transform _targetTransform;

	public override void OnStart()
	{
		_agentSpider.Agent.speed = _speed;
		_agentSpider.Agent.acceleration = _acceleraation;
		_agentSpider.Agent.isStopped = false;

		_currentTime = 0f;
		_chaseHandle = StartCoroutine(ChaseCheckTime());
	}

	public void UpdateTargetTransform(Transform targetTransform)
	{
		_targetTransform = targetTransform;
	}

	private IEnumerator ChaseCheckTime()
	{
		while (_currentTime < _chaseCheckTime)
		{
			_currentTime += Time.deltaTime;
			yield return null;
		}

		if (_agentSpider.FlashlightPerceptible != null)
		{
			_targetTransform = _agentSpider.FlashlightPerceptible.FlashlightSource.transform;
			_currentTime = 0f;
			yield return ChaseCheckTime();
		}
		else
		{
			_targetTransform = null;
			OnChaseFinishedEvent?.Invoke();
		}
	}

	public override void OnUpdate()
	{
		if (_agentSpider.Agent.velocity != Vector3.zero)
		{
			_agentSpider.transform.up = _agentSpider.Agent.velocity;
		}

		if (_targetTransform != null)
		{
			_agentSpider.Agent.SetDestination(_targetTransform.position);
		}
	}

	public override void OnFinish()
	{
		StartCoroutine(FadeMaterial());
		StopCoroutine(_chaseHandle);
		_animator.enabled = false;

	}

	private IEnumerator FadeMaterial()
	{
		Debug.Log("Fading starrted");
		Renderer renderer = _agentSpider.GetComponentInChildren<Renderer>();
		Material material = renderer.material;

		float elapsedTime = 0f;
		Color color = material.color;

		while (elapsedTime < _fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Lerp(1, 0, elapsedTime);
			material.color = color;
			yield return null;
		}
		Debug.Log("Fading ended");
	}
}
