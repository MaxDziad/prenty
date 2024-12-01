using UnityEngine;

public class MouseCursorController : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private float _cursorSpeed;

	[SerializeField]
	[Range(0f, 1f)]
	private float _xRangeThreshold = 0;

	[SerializeField]
	[Range(0f, 1f)]
	private float _yRangeThreshold = 0;

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
		var newTargetPosition = transform.position;
		newTargetPosition += new Vector3(vector.x, vector.y, 0) * Time.deltaTime * _cursorSpeed;
		SetCheckedNewPosition(newTargetPosition);
	}

	public void SetCheckedNewPosition(Vector3 newTargetPosition)
	{
		var screenPoint = Camera.main.WorldToViewportPoint(newTargetPosition);

		if (screenPoint.x >= 0 + _xRangeThreshold && screenPoint.x <= 1 - _xRangeThreshold)
		{
			transform.position = new Vector2(newTargetPosition.x, transform.position.y);
		}
		if (screenPoint.y >= 0 + _yRangeThreshold && screenPoint.y <= 1 - _yRangeThreshold)
		{
			transform.position = new Vector2(transform.position.x, newTargetPosition.y);
		}
	}

	private void OnDestroy()
	{
		if (_gameplayInputProviderSystem != null)
		{
			_gameplayInputProviderSystem.OnLookInputEvent -= OnLook;
		}
	}
}
