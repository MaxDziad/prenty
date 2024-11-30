using System;
using UnityEngine;

public class FlashlightPerceptible : MonoBehaviour, ISceneObject
{
	public event Action<FlashlightPerceptible> OnDisableEvent;

	[SerializeField]
	private FlashlightController _flashlightController;

	public void OnInitialize()
	{
		_flashlightController.OnFlashlightChangedEvent += OnFlashlightChanged;
	}

	private void OnFlashlightChanged(bool isActive)
	{
		gameObject.SetActive(isActive);
	}

	private void OnDisable()
	{
		OnDisableEvent?.Invoke(this);
	}

	private void OnDestroy()
	{
		if (_flashlightController != null)
		{
			_flashlightController.OnFlashlightChangedEvent -= OnFlashlightChanged;
		}
	}
}
