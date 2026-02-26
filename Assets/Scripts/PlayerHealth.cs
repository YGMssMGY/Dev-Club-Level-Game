using UnityEngine;
using System;

/// <summary>
/// Manages player health and takes damage.
/// Triggers PlayerDeathHandler when health reaching 0.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    public event Action<int, int> OnHealthChanged; // (current, max)
    
    private PlayerDeathHandler _deathHandler;
    private bool _isDead;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _deathHandler = GetComponent<PlayerDeathHandler>();
    }

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        if (_deathHandler != null)
        {
            _deathHandler.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentHealth() => _currentHealth;
    public int GetMaxHealth() => maxHealth;
}
