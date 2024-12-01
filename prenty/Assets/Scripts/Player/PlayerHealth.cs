using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour, ISceneObject
{
	public event Action OnPlayerTakeDamageEvent;
	public event Action OnPlayerDeathEvent;

	[SerializeField]
	private Light2D _flashlightCone;

	[SerializeField]
	private Light2D _flashlightInner;

	[SerializeField]
	private int _fullHealth = 3;

	private float _flashlightConeBaseO;
	private float _flashlightInnerBaseO;
	private float _flashlightConeBaseI;
	private float _flashlightInnerBaseI;

	private int _currentHealth;

	public void OnInitialize()
	{
		_currentHealth = _fullHealth;
		_flashlightConeBaseO = _flashlightCone.pointLightOuterRadius;
		_flashlightInnerBaseO = _flashlightInner.pointLightOuterRadius;
		_flashlightConeBaseI = _flashlightCone.pointLightInnerRadius;
		_flashlightInnerBaseI = _flashlightInner.pointLightInnerRadius;
	}

	void TakeDamage()
	{
		if (_currentHealth > 1)
		{
			_currentHealth--;
			ChangeIntensity(_currentHealth / (float)_fullHealth);
			OnPlayerTakeDamageEvent?.Invoke();
		}
		else
		{
			OnPlayerDeathEvent?.Invoke();
		}
	}

	private void ChangeIntensity(float newValue)
	{
		_flashlightCone.pointLightOuterRadius = newValue * _flashlightConeBaseO;
		_flashlightInner.pointLightOuterRadius = newValue * _flashlightInnerBaseO;
		_flashlightCone.pointLightInnerRadius = newValue * _flashlightConeBaseI;
		_flashlightInner.pointLightInnerRadius = newValue * _flashlightInnerBaseI;

	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			var spiderContext = collision.GetComponent<SpiderContext>();
			spiderContext.SpiderAgent.Destroy();
			TakeDamage();
		}
	}
}
