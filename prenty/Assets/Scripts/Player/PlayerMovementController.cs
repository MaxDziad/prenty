using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private PlayerAnimationController _animationController;

	[SerializeField]
	private float _movementSpeed = 2f;

	private GameplayInputProviderSystem _gameplayInputSystem;
	private CharacterController _characterController;
	private bool _isInitialized;

	private Vector2 _cachedMovementVector;
	private Vector3 _motionVector;

	public void OnInitialize()
	{
		if (GameInstance.Instance.TryGetSystem(out _gameplayInputSystem))
		{
			_gameplayInputSystem.OnMovementInputEvent += OnMovementInput;
		}

		_characterController = GetComponent<CharacterController>();
		_isInitialized = true;
	}

	public void Update()
	{
		if (_isInitialized)
		{
			_motionVector = new Vector3(_cachedMovementVector.x, _cachedMovementVector.y);
			_motionVector *= Time.deltaTime * _movementSpeed;
			_characterController.Move(_motionVector);
			transform.position = new Vector2(transform.position.x, transform.position.y);
			_animationController.UpdateMovementInfo(_motionVector != Vector3.zero);
		}
	}

	private void OnMovementInput(Vector2 vector)
	{
		_cachedMovementVector = vector;
	}

	private void OnDestroy()
	{
		_isInitialized = false;

		if (_gameplayInputSystem != null)
		{
			_gameplayInputSystem.OnMovementInputEvent -= OnMovementInput;
		}
	}
}
