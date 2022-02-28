using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    public float initialHealth;
    public float maxShield;

    public event System.Action OnDeath;

    public float health { get; set; }
    public float shield { get; set; }
    protected bool dead;

    protected virtual void Start()
    {
        health = initialHealth;
        shield = 0;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        if(shield > 0)
        {
            shield -= damage;
        }
        else
        {
            health -= damage;
        }
       

        if (health <= 0f && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        if (dead == true) return;

        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
