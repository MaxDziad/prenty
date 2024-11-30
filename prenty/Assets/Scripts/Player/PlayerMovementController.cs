using UnityEngine;

public class PlayerMovementController : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private float _movementSpeed = 2f;

	private GameplayInputProviderSystem _gameplayInputSystem;

	private Vector2 _cachedMovementVector;

	public void OnInitialize()
	{
		if (GameInstance.Instance.TryGetSystem(out _gameplayInputSystem))
		{
			_gameplayInputSystem.OnMovementInputEvent += OnMovementInput;
		}
	}

	public void Update()
	{
		transform.position += new Vector3(_cachedMovementVector.x, _cachedMovementVector.y)
			* Time.deltaTime * _movementSpeed;
	}

	private void OnMovementInput(Vector2 vector)
	{
		_cachedMovementVector = vector;
	}

	private void OnDestroy()
	{
		if (_gameplayInputSystem != null)
		{
			_gameplayInputSystem.OnMovementInputEvent -= OnMovementInput;
		}
	}
}
