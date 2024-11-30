using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Light2D _flashlightCone;
    [SerializeField] private Light2D _flashlightInner;
    [SerializeField] private int _fullHealth = 3;
    
    private int _currentHealth;
    
    void Start()
    {
        _currentHealth = _fullHealth;
    }

    void TakeDamage()
    {
        if (_currentHealth > 1)
        {
            _currentHealth--;
            ChangeIntensity((float)_currentHealth/(float)_fullHealth);
        }
        else Debug.Log("You ded bruh");
    }

    void ChangeIntensity(float newValue)
    {
        _flashlightCone.intensity = newValue;
        _flashlightInner.intensity = newValue;
    }
}
