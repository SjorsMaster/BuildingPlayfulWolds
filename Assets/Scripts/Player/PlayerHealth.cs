using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Script for handling health and death
/// 
/// Sources:
/// https://answers.unity.com/questions/1387562/set-a-function-to-call-another-using-the-inspector.html
/// </summary>

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Health and max health")]
    [SerializeField]
    float _health, _maxHealth = 100f;
    [Tooltip("What should happen when the player dies")]
    [SerializeField]
    UnityEvent _onDeath;
    [Tooltip("Reference to healthbar")]
    [SerializeField]
    Slider _healthSlider;

    //Initially setting up healthbar
    private void Awake()
    {
        UpdateHP();
    }

    //Whenever health should be updated for example while being attacked
    public void UpdateHP()
    {
        UpdateHP(0);
    }

    /// <summary>
    /// Change player health
    /// </summary>
    /// <param name="amount">Amount of health that should be added (For example +1 or -1)</param>
    public void UpdateHP(float amount)
    {
        _health += amount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthSlider.value = _health;
        if(_health <= 0)
        {
            _onDeath.Invoke();
        }
    }
}
