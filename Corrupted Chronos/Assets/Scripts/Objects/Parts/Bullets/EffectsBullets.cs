using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

//when a new effect is addded has to be integrated here
public enum AllEffectsBullets
{
    KnockbackEB,
    DamageEB
}




//all the effects for the bullets, someeffects could depen on other effects(holly shit)
//to add a new collider/trigger with it's own logic make it son of AddHitboxEB
public class EffectsBullets : MonoBehaviour
{
    
    protected BaseBullets baseBullets;
    protected GameObject prefabHitboxBox;
    protected GameObject prefabHitboxSphere;

    public string OpposedTag;
    public string CreatedTag;
    public bool IsFromPlayer;

    protected GameObject hitboxUsed;

    public AllInformationBullet AllInfoBullet{get; private set;}
    
    protected virtual void Awake()
    {
        baseBullets= this.gameObject.GetComponent<BaseBullets>();
        baseBullets.ReturnTags(out OpposedTag,out CreatedTag, out IsFromPlayer);
        prefabHitboxBox= statsManager.instance.listBulletStats.hitboxBullet;// Resources.Load<GameObject>("HitboxBullet");
        AllInfoBullet= baseBullets.ReturnFinalStats();
        // hitboxBulletGOSphere= GameManager.Instance.bulletDatabase.hitboxBullet; //no implementat
    }
    public virtual void Setup(NameHitboxInBullet name=NameHitboxInBullet.Null)
    {
        GameObject go;
        if (name == NameHitboxInBullet.Null)
        {
            //baseBullets.getlasthitbox donarà un collider i amb collider ho faig
            go= baseBullets.GetLastHitbox();
            //with this
            baseBullets.dicCollisionEnter[go]+=HitboxCollisionEnter;
            baseBullets.dicTriggerEnter[go]+=HitboxTriggerEnter;  
           
        }
        else
        {
            
            go=baseBullets.GetNametHitbox(name);

            baseBullets.dicCollisionEnter[go]+=HitboxCollisionEnter;
            baseBullets.dicTriggerEnter[go]+=HitboxTriggerEnter;  
            
        }
        hitboxUsed=go;
    }
    protected virtual void OnDestroy()
    {
        baseBullets.dicCollisionEnter[hitboxUsed]-=HitboxCollisionEnter;
        baseBullets.dicTriggerEnter[hitboxUsed]-=HitboxTriggerEnter;  
    }
    protected virtual void HitboxCollisionEnter(Collision collision)
    {
        
    }
    protected virtual void HitboxTriggerEnter(Collider trigger)
    {
        
    }

}



#region HitboxEffects

//makes the hitbox eable to do damage
public class DamageEB: EffectsBullets
{
    public event Action OnImpact;



    protected override void Awake()
    {
        base.Awake();
        //gets last hitbox added, 

    }
   /*  public override void Setup(NameHitboxInBullet name = NameHitboxInBullet.Null)
    {
       
    } */
    //or maybe in collision and not on trigger
    protected override void HitboxTriggerEnter(Collider trigger)
    {
        //mayeb not only enemy also player
        if (trigger.CompareTag(OpposedTag))
        {
            DoDamage(trigger);
        }
    }

    public void DoDamage(Collider trigger)
    {
        Debug.Log("DO DAMAGE ENEMY");
        //get component enemy or player
        //call method lossHealth
        OnImpact?.Invoke();

    }
}

//knockback when impacted with player //easy to make a new one for the enemys
public class KnockbackEB: EffectsBullets
{
    protected override void Awake()
    {   base.Awake();    }
    protected override void HitboxTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag(OpposedTag))
        {
            DoKnockback(trigger);
        }
    }
    public virtual void DoKnockback(Collider trigger)
    {
        

        //Modifify this, a script with a knockback, recieve damage, and other methods similars, and enemy and player have it,
        //this gets the script and do the stuff
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
        shipMovement.AddKnockback(direccio,AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Knockback]);

    }
}



public class FollowOpposedEB: EffectsBullets
{
    
    private Quaternion initialRotation;
    public float maxTiltAngle = 30f;
    public float rotationSpeed = 5f;
    
    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.rotation;
    }

    protected override void HitboxTriggerEnter(Collider trigger)
    {

        if (trigger.CompareTag(OpposedTag))
        {
            Vector3 directionToTarget = (trigger.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            float angle = Quaternion.Angle(initialRotation, lookRotation);
            if (angle > maxTiltAngle)
            {
                lookRotation = Quaternion.RotateTowards(initialRotation, lookRotation, maxTiltAngle);
            }

                // 5. Aplicar la rotació gradualment
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
/* 
//explosion when impacted with enemy
public class ExplosionEB: EffectsBullets
{
    protected override void Awake()
    {
        base.Awake();
        baseBullets.GetComponent<DamageEB>().OnImpact+=()=> hitboxUsed.SetActive(true);//Explosion;
    }
    public override void Setup(NameHitboxInBullet name = NameHitboxInBullet.Null)
    {
        base.Setup(name);
        hitboxUsed.SetActive(false);
    }
    protected override void OnDestroy() {
        base.Awake();
    }
    /* public void Explosion()
    {
        hitboxUsed.SetActive(true);
    } */

// } */


#endregion