using System.Collections;
using UnityEngine;

public class PlayerDashController : MonoBehaviour, ISceneObject
{
	private GameplayInputProviderSystem _gameplayInputProviderSystem;

	[SerializeField]
	private PlayerMovementController _playerMovementController;

	[SerializeField]
	private PlayerAnimationController _playerAnimationController;

	[SerializeField]
	private float _dashDuration = 0.3f;

	[SerializeField]
	private float _dashSpeed = 20f;

	[SerializeField]
	private float _dashCooldown = 4f;

	private bool _isDashing;
	private bool _isOnCooldown;
	private float _cooldownTimer;
	private float _dashTimer;

	private Vector2 _cachedInput;
	private Coroutine _coroutineHandle;

	public void OnInitialize()
	{
		if (GameInstance.Instance.TryGetSystem(out _gameplayInputProviderSystem))
		{
			_gameplayInputProviderSystem.OnMovementInputEvent += OnMovementInput;
			_gameplayInputProviderSystem.OnDodgeInputEvent += OnDodgeInput;
		}
	}

	private void OnMovementInput(Vector2 vector)
	{
		_cachedInput = vector.normalized;
	}

	private void OnDodgeInput()
	{
		if (!_isDashing && !_isOnCooldown && _cachedInput != Vector2.zero)
		{
			_coroutineHandle = StartCoroutine(DashRoutine());
		}
	}

	private IEnumerator DashRoutine()
	{
		_isDashing = true;
		_dashTimer = 0;
		Vector3 cachedInput = _cachedInput;
		_playerAnimationController.UpdateIsDashingInfo(true);

		while (_dashTimer < _dashDuration)
		{
			var deltaTime = Time.deltaTime;
			_dashTimer += deltaTime;
			_playerMovementController.AddAdditionalMotion(cachedInput * deltaTime * _dashSpeed);
			yield return null;
		}

		_playerAnimationController.UpdateIsDashingInfo(false);
		_isDashing = false;
		yield return CooldownRoutine();
	}

	private IEnumerator CooldownRoutine()
	{
		_isOnCooldown = true;
		_cooldownTimer = 0;

		while (_cooldownTimer < _dashCooldown)
		{
			_cooldownTimer += Time.deltaTime;
			yield return null;
		}

		_isOnCooldown = false;
	}

	private void OnDestroy()
	{
		if (_coroutineHandle != null)
		{
			StopCoroutine(_coroutineHandle);
		}

		if (_gameplayInputProviderSystem != null)
		{
			_gameplayInputProviderSystem.OnDodgeInputEvent -= OnDodgeInput;
			_gameplayInputProviderSystem = null;
		}
	}
}
