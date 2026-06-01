using System;
using System.Collections;
using Ink;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

//when a new effect is addded has to be integrated here
public enum NameEffectBullets
{
    AddHitboxEB=0,
    AddHitsphereEB=1,

    KnockbackEB=30,
    DamageEB=31,
    FollowOpposedEB=32,
    FireEB=33,
    CreateExplosionOnHitEB=34,
    ChangeDistanceToTimerEB=35,
    BounceOnHitEB=36
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

    protected bool UsesHitbox=true;
    protected GameObject hitboxUsed;

    public AllInformationBullet AllInfoBullet{get; private set;}
    protected NameHitboxInBullet MyNameHitbox;


    protected virtual void Awake()
    {
        baseBullets= this.gameObject.GetComponent<BaseBullets>();
        baseBullets.ReturnTags(out OpposedTag,out CreatedTag, out IsFromPlayer);
        prefabHitboxBox= statsManager.instance.listBulletStats.hitboxBullet;// Resources.Load<GameObject>("HitboxBullet");
        AllInfoBullet= baseBullets.ReturnFinalStats();
        prefabHitboxSphere= statsManager.instance.listBulletStats.hitsphereBullet; //no implementat
    }
    public virtual void Setup(NameHitboxInBullet name=NameHitboxInBullet.Null)
    {
        GameObject go=null;
        MyNameHitbox=name;
        if (name == NameHitboxInBullet.Null)
        {
            Debug.LogError("NameHitboxInBullet null when it shouldn't ");
            //baseBullets.getlasthitbox donarà un collider i amb collider ho faig
      /*       go= baseBullets.GetLastHitbox();
            //with this
            // baseBullets.dicCollisionEnter[go]+=HitboxCollisionEnter;
            baseBullets.dicTriggerEnter[go]+=HitboxTriggerEnter;  
           
            // baseBullets.dicCollisionExit[go]+=HitboxCollisionExit;
            baseBullets.dicTriggerExit[go]+=HitboxTriggerExit;   */
        }
        else
        {
            
            go=baseBullets.GetNametHitbox(name);

            /* if (go == null)
            {
                Debug.LogError("no hitbox added with name");
            } */


            // baseBullets.dicCollisionEnter[go]+=HitboxCollisionEnter;
            if(UsesHitbox){
                baseBullets.dicTriggerEnter[go]+=HitboxTriggerEnter;  

            // baseBullets.dicCollisionExit[go]+=HitboxCollisionExit;
                baseBullets.dicTriggerExit[go]+=HitboxTriggerExit;  
            }
        }
        hitboxUsed=go;
        AfterSetup();
    }
    protected virtual void AfterSetup(){}
    
    protected virtual void OnDestroy()
    {
        // baseBullets.dicCollisionEnter[hitboxUsed]-=HitboxCollisionEnter;
        baseBullets.dicTriggerEnter[hitboxUsed]-=HitboxTriggerEnter;  

        // baseBullets.dicCollisionExit[hitboxUsed]-=HitboxCollisionExit;
        baseBullets.dicTriggerExit[hitboxUsed]-=HitboxTriggerExit;  

    }

    /* protected abstract void NecessaryToWork(){
        //aquest null
    } */
    protected virtual void FixedUpdate() {}

    // protected virtual void HitboxCollisionEnter(Collision collision){}
    protected virtual void HitboxTriggerEnter(Collider trigger){}

    //if exit methods are hard, could be changed to timers I guess
    // protected virtual void HitboxCollisionExit(Collision collision){}
    protected virtual void HitboxTriggerExit(Collider trigger){}
}



#region HitboxEffects

//makes the hitbox eable to do damage
public class DamageEB: EffectsBullets
{
    public event Action<Collider> OnHit;

    private Collider lastTrigger=null;
    private Coroutine TriggerCleaner=null;
    // private float penetration

    protected override void Awake()
    {
        base.Awake();
        //gets last hitbox added, 

    }
    protected override void AfterSetup()
    {
        if (MyNameHitbox == NameHitboxInBullet.Null)
        {
            Debug.LogError("name hitbox is null");
        }
        if (baseBullets.DamageHitbox.ContainsKey(MyNameHitbox))
        {
            Debug.LogError("this damage htibox is already applied");
            return;
        }

        baseBullets.DamageHitbox.Add(MyNameHitbox, this);
    }

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
        if (trigger.gameObject.TryGetComponent<IDamageable>(out IDamageable victim))
        {
            victim.TakeDamage(AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Damage]);
        }
        if(trigger.CompareTag(OpposedTag)){
            if (lastTrigger != trigger)
            {
                AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Piercing]-=1;
            }
            if(TriggerCleaner==null)TriggerCleaner= StartCoroutine(ClearTrigger());
            lastTrigger=trigger;

        }
        

        OnHit?.Invoke(trigger);

        if (AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Piercing] < 0)
        {
            //maybe dejar 4 frames de tiempo apra eliminar la bala
            Debug.Log("piercing equals:"+ AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Piercing]);
            // baseBullets.DieWithTimer();
            Destroy(this.gameObject);

            // this.gameObject.active(false);
        }
    }

    /* public IEnumerator Time2Die()
    {
        yield return new WaitForSecondsRealtime(1f);
        Destroy(this.gameObject);

    } */
    public IEnumerator ClearTrigger()
    {
        yield return new WaitForSecondsRealtime(2f);
        lastTrigger=null;
        TriggerCleaner=null;

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
        Vector3 direccio = trigger.transform.position - transform.position;

        if (trigger.gameObject.TryGetComponent<IDamageable>(out IDamageable victim))
        {
            // Si el troba, cridem el mètode directament
            victim.AddKnockback(direccio, AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Knockback]);
        }

        //Modifify this, a script with a knockback, recieve damage, and other methods similars, and enemy and player have it,
        //this gets the script and do the stuff
       /*  CharacterController controller = trigger.GetComponent<CharacterController>();
        if(controller==null) {
            Debug.LogError("controller not found");
            return;
        }
        ShipMovement shipMovement= trigger.GetComponent<ShipMovement>();
        if(shipMovement==null) {
            Debug.LogError("pilot movement not found");
            return;
        }
        shipMovement.AddKnockback(direccio,AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Knockback]); */

    }
}



public class FollowOpposedEB: EffectsBullets
{
    
    private Quaternion initialRotation;
    public float maxTiltAngle = 50f;
    public float rotationSpeed = 30f;
    
    private Collider target=null;
    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.rotation;
    }

    protected override void HitboxTriggerEnter(Collider trigger)
    {
        Debug.Log("follow trigger enter");
        
        if (trigger.CompareTag(OpposedTag))
        {
            Debug.Log("follow BINGO");
            target=trigger;
        }
        else
        {
            Debug.Log("follow merda"+ trigger.tag );
            
        }
    }
    protected override void HitboxTriggerExit(Collider trigger)
    {
        if (trigger.CompareTag(OpposedTag))
        {
            Debug.Log("follow BINGO");
            target=null;
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void RotateBullet()
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        Vector3 targetEuler = lookRotation.eulerAngles;

        Quaternion filteredRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, initialRotation.eulerAngles.z);
        float angle = Quaternion.Angle(initialRotation, filteredRotation);
        if (angle > maxTiltAngle)
        {
            filteredRotation = Quaternion.RotateTowards(initialRotation, filteredRotation, maxTiltAngle);
        }

        // 5. Aplicar la rotació gradualment
        transform.rotation = Quaternion.Slerp(transform.rotation, filteredRotation, rotationSpeed * Time.deltaTime);
    }

    protected override void FixedUpdate() {
        if(target!=null){
            RotateBullet();
        }
    }

}



// Fire bullets
public class FireEB: EffectsBullets
{

    private float dmg;
    private float durationFire=3f;
    // public event Action OnImpact;

    private bool onDeclive=false;
    protected override void AfterSetup()
    {   base.Awake();    
        baseBullets.OnDecliveRange+= ()=>onDeclive=true;
        dmg= AllInfoBullet.StatsBullet[Stat.StatTypeBullet.ElementalDamage];
        // definir ALGO AMB ELS REQUISITS
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        baseBullets.OnDecliveRange-= ()=>onDeclive=true;
    }
    protected override void HitboxTriggerEnter(Collider trigger)
    {
        
        if (trigger.gameObject.TryGetComponent(out IDamageable victim) && trigger.CompareTag("Enemy"))
        {
            if(!onDeclive){
            
                    if(trigger.gameObject.TryGetComponent(out FireEffect fireEffect))
                    {
                        fireEffect.BurnMore(dmg, durationFire);
                    }else{
                        FireEffect foc = trigger.gameObject.AddComponent<FireEffect>();
                        foc.Setup(victim, dmg, durationFire); 
                    }
            }else
            {
                //add efect of ashes probably
            }
        }

    }

    

}

public class CreateExplosionOnHitEB: EffectsBullets
{
     protected override void Awake()
    {   
        Debug.Log("generate explosion awake");

        base.Awake();
        UsesHitbox=false;
        }

    protected override void AfterSetup()
    {
        baseBullets.DamageHitbox[MyNameHitbox].OnHit += CreateExplosion;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        baseBullets.DamageHitbox[MyNameHitbox].OnHit -= CreateExplosion; 
    }
    protected void CreateExplosion(Collider trigger)
    {
        
        // AllInformationBullet? newBulletInfo= statsManager.instance.listBulletStats.ReturnBulletStatsSO(NameBulletPreset.Explosion);
        GameObject newBullet= Instantiate(statsManager.instance.listBulletStats.GeneralBullet, gameObject.transform.position, quaternion.identity);
        newBullet.GetComponent<CreateBullet>().Setup(true, NameBulletPreset.Explosion);
        Debug.Log("generate explosion");
    }
}

public class ChangeDistanceToTimerEB : EffectsBullets
{
    protected override void Awake()
    {   base.Awake();
        UsesHitbox=false;
        baseBullets.ChangeDistanceToTimer();
    }

}

public class BounceOnHitEB : EffectsBullets
{
    protected override void Awake()
    {   base.Awake();
        UsesHitbox=false;
    }
    bool OutOfBounceRange=false;
    protected override void AfterSetup()
    {
        baseBullets.DamageHitbox[MyNameHitbox].OnHit += BounceBullet;
        baseBullets.OnDecliveRange+=()=>OutOfBounceRange=true;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        baseBullets.DamageHitbox[MyNameHitbox].OnHit -= BounceBullet; 
    }
    protected void BounceBullet(Collider trigger)
    {   
        /* float nRotation= UnityEngine.Random.Range(100f,230f); 
        transform.Rotate(0,nRotation,0);  */
        if(!OutOfBounceRange){
            Vector3 direccioActual = transform.forward;
            Ray ray = new Ray(transform.position - direccioActual * 0.5f, direccioActual);
            RaycastHit hit;
            if (trigger.Raycast(ray, out hit, 1.5f))
            {
                Vector3 normalWall = hit.normal;

                Vector3 dirBounce = Vector3.Reflect(direccioActual, normalWall);

                float nRotation = UnityEngine.Random.Range(-35f, 35f);
                
                dirBounce = Quaternion.Euler(0, nRotation, 0) * dirBounce;
                transform.rotation = Quaternion.LookRotation(dirBounce);
    
                transform.position = hit.point + normalWall * 0.05f;
            }
        }
    }
    
}

/* public class OverTheLimitEB : EffectsBullets
{
    
    should reach the basegun script and modifify the method of the maxlimit, it shouldn't delte it, 
    it should extend the life of the bullet, some more time
    

} */


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