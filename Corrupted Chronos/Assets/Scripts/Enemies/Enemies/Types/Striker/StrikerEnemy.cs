using UnityEngine;

public class StrikerEnemy : EnemyUnit
{
    public override void primaryAttack()
    {
        if (!CanFire())
            return;

        Instantiate(projectile, transform.position, transform.rotation);

        RegisterFire();
    }

    public override void secondarySkill()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //primaryAttack();
    }
}
