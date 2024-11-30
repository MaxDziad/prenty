using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputProviderSystem : MonoBehaviour, IGameSystem, PlayerInputActionAsset.IGameplayActions
{
	public Action<Vector2> OnMovementInputEvent;

	private PlayerInputActionAsset _inputActionAsset;

	public void Initialize() { }

	public void OnSystemsInitialized()
	{
		if (GameInstance.Instance.TryGetSystem<InputSystem>(out var inputSystem))
		{
			_inputActionAsset = inputSystem.PlayerInputActions;
			_inputActionAsset.Gameplay.AddCallbacks(this);
		}
	}

	public void OnMovement(InputAction.CallbackContext context)
	{
		OnMovementInputEvent?.Invoke(context.ReadValue<Vector2>());
	}

	public void Uninitialize()
	{
		_inputActionAsset = null;
	}
}
