using System;
using UnityEngine;

public class FlashlightController : MonoBehaviour, ISceneObject
{
	public event Action<bool> OnFlashlightChangedEvent;

	[SerializeField]
	private GameObject _flashlight;

	private GameplayInputProviderSystem _gameplayInputProviderSystem;

	public bool IsFlashlightOn { get; private set; }

	public void OnInitialize()
	{
		if (GameInstance.Instance.TryGetSystem(out _gameplayInputProviderSystem))
		{
			_gameplayInputProviderSystem.OnFlashlightInputEvent += OnFlashlight;
		}
	}

	public void OnFlashlight(bool isOn)
	{
		IsFlashlightOn = isOn;
		_flashlight.SetActive(IsFlashlightOn);
		OnFlashlightChangedEvent?.Invoke(IsFlashlightOn);
	}
}
