using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable<int>
{
    [SerializeField]
    int _startHealth = 5;

    int _currentHealth;

    HealthBar _healthBar;
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _startHealth;
        _healthBar = gameObject.GetComponentInChildren<HealthBar>();
    }

    public void TakeDamage(int damageAmount)
    {
        ModifyHealth(damageAmount);

        if (_currentHealth <= 0)
            Die();
        else
            _animator.SetTrigger("TakeDamage");
    }

    public void ModifyHealth(int amount)
    {
        _currentHealth -= amount;

        float currentHealthPct = (float)_currentHealth / (float)_startHealth;
        _healthBar.HandleHealthChanged(currentHealthPct);
    }

    private void Die()
    {
        gameObject.tag = "Untagged";
        _animator.SetTrigger("Die");
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
