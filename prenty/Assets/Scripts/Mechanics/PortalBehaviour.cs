using System;
using System.Collections;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
	public event Action OnPortalDestructionEvent;
	public event Action<PortalDestructionStage> OnPortalStageChangedEvent;

	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private Transform _raycastSource;

	[SerializeField]
	private FlashlightPerceptor _flashlightPerceptor;

	[SerializeField]
	private float _maxDestroyTime = 1.5f;

	[SerializeField]
	private float _damagedTreshold = 0.33f;

	[SerializeField]
	private float _almostDestructedTreshold = 0.66f;

	private float _currentTime;
	private Coroutine _currentHandle;
	private PortalDestructionStage _destructionStage = PortalDestructionStage.None;
	private Transform _pawnTransform;

	public PortalDestructionStage DestructionStage
	{
		get => _destructionStage;
		set
		{
			if (_destructionStage != value)
			{
				_destructionStage = value;
				OnPortalStageChangedEvent?.Invoke(value);
			}
		}
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}

	public void Start()
	{
		_flashlightPerceptor.OnSeeingFlashlightEvent += OnSeeingFlashlight;
		_flashlightPerceptor.OnStopSeeingFlashlightEvent += OnStopSeeingFlashlight;
	}

	private void OnSeeingFlashlight(FlashlightPerceptible perceptible)
	{
		_pawnTransform = perceptible.FlashlightSource.transform;
		StopCoroutine();
		_currentHandle = StartCoroutine(DestroyingRoutine());



	}



	private void StopCoroutine()
	{
		if (_currentHandle != null)
		{
			StopCoroutine(_currentHandle);
		}
	}

	private IEnumerator DestroyingRoutine()
	{
		while (_currentTime < _maxDestroyTime)
		{
			if (Physics.Linecast(_raycastSource.position, _pawnTransform.position, _layerMask,
				QueryTriggerInteraction.Ignore))
			{
				_currentTime += Time.deltaTime;
				TrySignalStageChange(_currentTime / _maxDestroyTime);
			}
			yield return null;
		}

		OnPortalDestructionEvent?.Invoke();
	}

	private void TrySignalStageChange(float progress)
	{
		switch (DestructionStage)
		{
			case PortalDestructionStage.None:
				if (progress >= _damagedTreshold)
				{
					DestructionStage = PortalDestructionStage.Damaged;
				}
				break;
			case PortalDestructionStage.Damaged:
				if (progress >= _almostDestructedTreshold)
				{
					DestructionStage = PortalDestructionStage.AlmostDestructed;
				}
				else if (progress < _damagedTreshold)
				{
					DestructionStage = PortalDestructionStage.None;
				}
				break;
			case PortalDestructionStage.AlmostDestructed:
				if (progress < _almostDestructedTreshold)
				{
					DestructionStage = PortalDestructionStage.Damaged;
				}
				break;
		}
	}

	private IEnumerator RestoringRoutine()
	{
		while (_currentTime > 0)
		{
			_currentTime -= Time.deltaTime;
			TrySignalStageChange(_currentTime / _maxDestroyTime);
			yield return null;
		}

		_currentTime = 0;
	}

	private void OnStopSeeingFlashlight(FlashlightPerceptible perceptible)
	{
		StopCoroutine();
		_currentHandle = StartCoroutine(RestoringRoutine());
		_pawnTransform = null;
	}

	private void OnDestroy()
	{
		if (_flashlightPerceptor != null)
		{
			_flashlightPerceptor.OnSeeingFlashlightEvent -= OnSeeingFlashlight;
			_flashlightPerceptor.OnStopSeeingFlashlightEvent -= OnStopSeeingFlashlight;
		}
	}
}
