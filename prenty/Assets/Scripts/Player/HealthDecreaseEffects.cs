using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class HealthDecreaseEffects : MonoBehaviour
{
	[SerializeField]
	private PlayerHealth _playerHealth;

	[SerializeField]
	private CinemachineImpulseSource _impulseSource;

	[SerializeField]
	private Material _damageOverlayMaterial;

	[SerializeField]
	private float _maxBlendValue = 0.7f;

	[SerializeField]
	private float _blendToZeroTime = 1f;

	private Coroutine _handle;
	private float _currentTime;

	private void Start()
	{
		_playerHealth.OnPlayerTakeDamageEvent += OnPlayerTakeDamage;
	}

	private void OnPlayerTakeDamage()
	{
		_impulseSource.GenerateImpulse();
		_handle = StartCoroutine(BlendToZeroRoutine());
	}

	private IEnumerator BlendToZeroRoutine()
	{
		UpdateBlendValue(_maxBlendValue);
		_currentTime = _blendToZeroTime;

		while (_currentTime > 0)
		{
			_currentTime -= Time.deltaTime;
			UpdateBlendValue(_maxBlendValue * _currentTime / _blendToZeroTime);
			yield return null;
		}

		UpdateBlendValue(0);
	}

	private void UpdateBlendValue(float value)
	{
		_damageOverlayMaterial.SetFloat("_Blend", value);
	}

	private void OnDestroy()
	{
		if (_handle != null)
		{
			StopCoroutine(_handle);
		}

		if (_playerHealth != null)
		{
			_playerHealth.OnPlayerTakeDamageEvent -= OnPlayerTakeDamage;
		}
	}
}
