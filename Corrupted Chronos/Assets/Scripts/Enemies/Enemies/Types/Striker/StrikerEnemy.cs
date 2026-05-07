using UnityEngine;

public class StrikerEnemy : EnemyUnit
{
    public override void primaryAttack()
    {
        if (!CanFire())
            return;

        GameObject bulletInst = Instantiate(statsManager.instance.listBulletStats.GeneralBullet, transform.position, transform.rotation);

        bulletInst.GetComponent<CreateBullet>().Setup(false, projectile);

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
