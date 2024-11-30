using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(InputSystemUIInputModule))]
public class InputSystem : MonoBehaviour, IGameSystem
{
	private PlayerInput _playerInput;

	private PlayerInputActionAsset _actionAsset;
	private InputSystemUIInputModule _uiModule;
	public PlayerInputActionAsset PlayerInputActions => _actionAsset;

	public void Initialize()
	{
		_actionAsset = new PlayerInputActionAsset();
		_playerInput = GetComponent<PlayerInput>();
		_uiModule = GetComponent<InputSystemUIInputModule>();

		_playerInput.actions = _actionAsset.asset;
		_uiModule.actionsAsset = _actionAsset.asset;
	}

	public void Uninitialize()
	{
		_playerInput = null;
	}
}
