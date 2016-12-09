using System;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    public float initialHealth;

    protected float health;
    protected bool dead;

    protected virtual void Start()
    {
        health = initialHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0f && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
    }
}
