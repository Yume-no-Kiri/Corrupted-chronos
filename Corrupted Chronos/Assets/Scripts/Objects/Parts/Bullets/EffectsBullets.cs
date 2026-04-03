using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

//all the effects for the bullets, someeffects could depen on other effects(holly shit)
//to add a new collider/trigger with it's own logic make it son of AddHitboxEB
public class EffectsBullets : MonoBehaviour
{
    // protected int myHitboxID=-1;
    // protected GameObject 
    protected BaseBullets baseBullets;
    protected GameObject hitboxBulletGO;
    protected GameObject hitboxBulletInst;


    protected virtual void Awake()
    {
        baseBullets= this.gameObject.GetComponent<BaseBullets>();

        hitboxBulletGO= GameManager.Instance.bulletDatabase.hitboxBullet;// Resources.Load<GameObject>("HitboxBullet");

    }
    
}



#region HitboxEffects
public class AddHitboxEB: EffectsBullets
{
   protected override void Awake()
    {
        base.Awake();
        hitboxBulletInst=Instantiate(hitboxBulletGO,baseBullets.gameObject.transform.position,baseBullets.gameObject.transform.rotation,baseBullets.gameObject.transform);
        baseBullets.GetsNewID(hitboxBulletInst);

        baseBullets.dicCollisionEnter[hitboxBulletInst]+=HitboxCollisionEnter;
        baseBullets.dicTriggerEnter[hitboxBulletInst]+=HitboxTriggerEnter;  
    }

    protected virtual void HitboxCollisionEnter(Collision collision){

    }

    protected virtual void HitboxTriggerEnter(Collider trigger)
    {
        
    }


    private void OnDestroy() {
        
        baseBullets.DeletesThisID(hitboxBulletInst);
        //probably deletes the bullet to
    }
}
public class DamageEB: AddHitboxEB
{
    protected override void Awake()
    {
        base.Awake();
        //change size probably?
    }
    //or maybe in collision and not on trigger
    protected override void HitboxTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Enemy"))
        {
            DoDamage(trigger);
        }
    }

     public void DoDamage(Collider trigger)
    {
        Debug.Log("DO DAMAGE ENEMY");
        
    }
}

public class KnockbackEB: AddHitboxEB
{


    protected override void Awake()
    {
        base.Awake();

    }


    protected override void HitboxTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            DoKnockback(trigger);
        }
    }

    public void DoKnockback(Collider trigger)
    {
        /* Rigidbody rbVictima = trigger.gameObject.GetComponent<Rigidbody>();
        if(rbVictima==null) return;
        Vector3 direccio = trigger.transform.position - transform.position;
        
        direccio.y = 0; 
        direccio = direccio.normalized;
    
        rbVictima.AddForce(direccio * baseBullets.statsBullet.knockback, ForceMode.Impulse); */
        CharacterController controller = trigger.GetComponent<CharacterController>();
        if(controller==null) {
            Debug.LogError("controller not found");
            return;
        }
        ShipMovement shipMovement= trigger.GetComponent<ShipMovement>();
        if(shipMovement==null) {
            Debug.LogError("pilot movement not found");
            return;
        }
        Vector3 direccio = trigger.transform.position - transform.position;
        shipMovement.AddKnockback(direccio,baseBullets.StatsBullet.knockback);

        // Destroy(this);

    }


}
#endregion