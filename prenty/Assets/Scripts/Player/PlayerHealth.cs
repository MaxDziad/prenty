using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Light2D _flashlightCone;
    [SerializeField] private Light2D _flashlightInner;
    [SerializeField] private int _fullHealth = 3;

    private float _flashlightConeBaseO;
    private float _flashlightInnerBaseO;
    private float _flashlightConeBaseI;
    private float _flashlightInnerBaseI;
    
    private int _currentHealth;
    public UnityEvent OnGameOver;
    
    void Start()
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
            ChangeIntensity((float)_currentHealth/(float)_fullHealth);
        }
        else
        {
            OnGameOver?.Invoke();
        }
    }

    void ChangeIntensity(float newValue)
    {
        _flashlightCone.pointLightOuterRadius = newValue * _flashlightConeBaseO;
        _flashlightInner.pointLightOuterRadius = newValue * _flashlightInnerBaseO;
        _flashlightCone.pointLightInnerRadius = newValue * _flashlightConeBaseI;
        _flashlightInner.pointLightInnerRadius = newValue * _flashlightInnerBaseI;
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("ała kurwa");
            TakeDamage();
        }
    }
}
