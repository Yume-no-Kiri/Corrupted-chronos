using UnityEngine;

public class bossTower : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    
    void Start()
    {
        health = maxHealth;
        BossSpawner.instance.bossTowers.Add(this.gameObject);
    }

    public void AddKnockback(Vector3 dir, float force)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        Destroy(this.gameObject);
        BossSpawner.instance.towerDestroyed(this.gameObject);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
}
