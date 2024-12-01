using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiAgentSpider : MonoBehaviour, ISceneObject
{
	public event Action<AiAgentSpider> OnSpiderDestroyEvent;

	[SerializeField]
	private FlashlightPerceptor _flashlightPerceptor;

	[SerializeField]
	private ChillingState _chillingState;

	[SerializeField]
	private TriggeringState _triggeringState;

	[SerializeField]
	private ChasingState _chasingState;

	private AbstractAiState _currentState;
	private NavMeshAgent _navmeshAgent;
	private FlashlightPerceptible _flashlightPerceptible;
	private bool _isReady;

	public FlashlightPerceptible FlashlightPerceptible => _flashlightPerceptible;
	public NavMeshAgent Agent => _navmeshAgent;

	public void Spawn()
	{
		OnInitialize();
		OnGameStart();
	}

	public void OnInitialize()
	{
		_navmeshAgent = GetComponent<NavMeshAgent>();
		_currentState = _chillingState;
		_flashlightPerceptor.OnSeeingFlashlightEvent += OnSeeingFlashlight;
		_flashlightPerceptor.OnStopSeeingFlashlightEvent += OnStopSeeingFlashlight;
	}

	public void SetBrainActive(bool isActive)
	{
		_isReady = isActive;
		Agent.isStopped = !isActive;
	}

	private void OnSeeingFlashlight(FlashlightPerceptible perceptible)
	{
		if (_flashlightPerceptible == null)
		{
			_flashlightPerceptible = perceptible;
		}
	}

	private void OnStopSeeingFlashlight(FlashlightPerceptible perceptible)
	{
		if (_flashlightPerceptible == perceptible)
		{
			_flashlightPerceptible = null;
		}
	}

	public void OnGameStart()
	{
		if (_navmeshAgent != null)
		{
			_navmeshAgent.updateRotation = false;
			_navmeshAgent.updateUpAxis = false;
			_navmeshAgent.enabled = true;
			_currentState.OnStart();
			_isReady = true;
		}
	}

	public void Update()
	{
		if (_isReady && _currentState != null)
		{
			_currentState.OnUpdate();
			CheckTransitions();
		}
	}

	private void CheckTransitions()
	{
		if (_currentState.SpiderState == SpiderState.Chill && FlashlightPerceptible != null)
		{
			_chasingState.UpdateTargetTransform(FlashlightPerceptible.FlashlightSource.transform);
			_currentState.OnFinish();
			_currentState = _triggeringState;
			_currentState.OnStart();
			_triggeringState.OnAlertFinishedEvent += OnAlertFinished;
		}
	}

	private void OnAlertFinished()
	{
		_triggeringState.OnAlertFinishedEvent -= OnAlertFinished;
		_currentState.OnFinish();

		_chasingState.OnChaseFinishedEvent += OnChaseFinished;
		_currentState = _chasingState;
		_currentState.OnStart();
	}

	private void OnChaseFinished()
	{
		_chasingState.OnChaseFinishedEvent -= OnChaseFinished;
		_currentState.OnFinish();

		_currentState = _chillingState;
		_currentState.OnStart();
	}

	public void Destroy()
	{
		OnSpiderDestroyEvent?.Invoke(this);

		if (_flashlightPerceptor != null)
		{
			_flashlightPerceptor.OnSeeingFlashlightEvent -= OnSeeingFlashlight;
			_flashlightPerceptor.OnStopSeeingFlashlightEvent -= OnStopSeeingFlashlight;
		}

		Destroy(gameObject);
	}
}
