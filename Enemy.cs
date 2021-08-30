using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int>
{
    [SerializeField] 
    AudioSource _dieSound;
    [SerializeField]
    int _startHealth = 5;

    int _currentHealth;

    HealthBar _healthBar;
    Animator _animator;

    EnemyMovement _enemyMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _startHealth;
        _healthBar = gameObject.GetComponentInChildren<HealthBar>();
        _enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    public void TakeDamage(int damageAmount)
    {
        ModifyHealth(damageAmount);

        if (_currentHealth <= 0)
        {
            Die();
        }          
        else
        {
            _animator.SetTrigger("TakeDamage");
            _enemyMovement.IsChasingAfterAttack = true;
        }            
    }

    public void ModifyHealth(int amount)
    {
        _currentHealth -= amount;

        float currentHealthPct = (float)_currentHealth / (float)_startHealth;
        _healthBar.HandleHealthChanged(currentHealthPct);
    }

    private void Die()
    {
        _enemyMovement.Stop();
        _dieSound.Play();
        _animator.SetTrigger("Die");
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
