using System;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    public float initialHealth;

    public event System.Action OnDeath;

    protected float health;
    protected bool dead;

    protected virtual void Start()
    {
        health = initialHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
