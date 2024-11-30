using UnityEngine;

public class MouseCursorController : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private float _cursorSpeed;

	private GameplayInputProviderSystem _gameplayInputProviderSystem;

	public void OnInitialize()
	{
		transform.SetParent(null);

		if (GameInstance.Instance.TryGetSystem(out _gameplayInputProviderSystem))
		{
			_gameplayInputProviderSystem.OnLookInputEvent += OnLook;
		}
	}

	private void OnLook(Vector2 vector)
	{
		transform.position += new Vector3(vector.x, vector.y, 0) * Time.deltaTime * _cursorSpeed;
	}

	private void OnDestroy()
	{
		if (_gameplayInputProviderSystem != null)
		{
			_gameplayInputProviderSystem.OnLookInputEvent -= OnLook;
		}
	}
}
