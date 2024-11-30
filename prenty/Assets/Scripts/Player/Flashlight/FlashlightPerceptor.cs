using System;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPerceptor : MonoBehaviour
{
	public event Action<FlashlightPerceptible> OnSeeingFlashlightEvent;
	public event Action<FlashlightPerceptible> OnStopSeeingFlashlightEvent;

	private HashSet<FlashlightPerceptible> _registeredPerceptibles = new();

	private void OnTriggerEnter(Collider other)
	{
		FlashlightPerceptible perceptible = other.GetComponent<FlashlightPerceptible>();

		if (perceptible != null)
		{
			_registeredPerceptibles.Add(perceptible);
			perceptible.OnDisableEvent += OnPeceptibleDisable;
			OnSeeingFlashlightEvent?.Invoke(perceptible);
		}
	}

	private void OnPeceptibleDisable(FlashlightPerceptible perceptible)
	{
		perceptible.OnDisableEvent -= OnPeceptibleDisable;
		RemovePerceptible(perceptible);
	}

	private void OnTriggerExit(Collider other)
	{
		FlashlightPerceptible perceptible = other.GetComponent<FlashlightPerceptible>();

		if (perceptible != null && _registeredPerceptibles.Contains(perceptible))
		{
			RemovePerceptible(perceptible);
		}
	}

	private void RemovePerceptible(FlashlightPerceptible perceptible)
	{
		_registeredPerceptibles.Remove(perceptible);
		OnStopSeeingFlashlightEvent?.Invoke(perceptible);
	}
}
