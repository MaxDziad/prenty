using System;
using System.Collections;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private FlashlightPerceptor _flashlightPerceptor;

	[SerializeField]
	private float _maxDestroyTime = 1.5f;

	private float _currentTime;
	private Coroutine _currentHandle;

	public void OnInitialize()
	{
		_flashlightPerceptor.OnSeeingFlashlightEvent += OnSeeingFlashlight;
		_flashlightPerceptor.OnStopSeeingFlashlightEvent += OnStopSeeingFlashlight;
	}

	private void OnSeeingFlashlight(FlashlightPerceptible perceptible)
	{
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
			_currentTime += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}

	private IEnumerator RestoringRoutine()
	{
		while (_currentTime > 0)
		{
			_currentTime -= Time.deltaTime;
			yield return null;
		}

		_currentTime = 0;
	}

	private void OnStopSeeingFlashlight(FlashlightPerceptible perceptible)
	{
		StopCoroutine();
		_currentHandle = StartCoroutine(RestoringRoutine());
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
