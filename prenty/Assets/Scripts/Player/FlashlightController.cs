using System;
using UnityEngine;

public class FlashlightController : MonoBehaviour, ISceneObject
{

    [SerializeField] private GameObject _flashlight;
    private GameplayInputProviderSystem _gameplayInputProviderSystem;


    public void OnInitialize()
    {
        
        if (GameInstance.Instance.TryGetSystem(out _gameplayInputProviderSystem))
        {
            _gameplayInputProviderSystem.OnFlashlightInputEvent += OnFlashlight;
        }
        
    }

    public void OnFlashlight(Boolean isOn)
    {
        if (isOn) _flashlight.SetActive(true);
        else _flashlight.SetActive(false);
    }
}
