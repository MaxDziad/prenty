using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputProviderSystem : MonoBehaviour, IGameSystem, PlayerInputActionAsset.IGameplayActions
{
	public Action<Vector2> OnMovementInputEvent;
	public Action<Vector2> OnLookInputEvent;
	
	public Action<bool> OnFlashlightInputEvent;
	public Action OnDodgeInputEvent;

	private PlayerInputActionAsset _inputActionAsset;

	public void Initialize() { }

	public void OnSystemsInitialized()
	{
		if (GameInstance.Instance.TryGetSystem<InputSystem>(out var inputSystem))
		{
			_inputActionAsset = inputSystem.PlayerInputActions;
			_inputActionAsset.Gameplay.AddCallbacks(this);
			_inputActionAsset.Disable();
		}
	}

	public void OnSceneStart()
	{
		_inputActionAsset.Enable();
	}

	public void OnMovement(InputAction.CallbackContext context)
	{
		OnMovementInputEvent?.Invoke(context.ReadValue<Vector2>());
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		OnLookInputEvent?.Invoke(context.ReadValue<Vector2>());
	}

	public void OnFlashlight(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Started){
			OnFlashlightInputEvent?.Invoke(true);
		}
		else if(context.phase == InputActionPhase.Canceled){
			OnFlashlightInputEvent?.Invoke(false);
		}
	}

	public void OnDodge(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			OnDodgeInputEvent?.Invoke();
		}
	}

	public void Uninitialize()
	{
		_inputActionAsset = null;
	}
}
