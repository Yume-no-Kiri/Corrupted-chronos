using UnityEngine;

public class bossTower : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public DialogueManager dialogue_manager;
    
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
        dialogue_manager.UpdatePillars();
        BossSpawner.instance.towerDestroyed(this.gameObject);
        GameManager.Instance.TowerKilled(gameObject.transform.position);
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
