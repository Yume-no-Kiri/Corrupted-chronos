using UnityEngine;

public enum NameHitboxInBullet
{
    Null,
    Front,
    Body,
    Tail, 
    AfterDeath, //have travelled and don't impacted
    AfterImpact, //have travelled and impacted
    Detectors
}

public class AddHitboxList : MonoBehaviour
{
    public GameObject BoxCollider;
    public GameObject SphereCollider;
}
public class AddHitboxEB: EffectsBullets
{

    // protected GameObject hitboxBulletInst;
    public NameHitboxInBullet name;
    public override void Setup(NameHitboxInBullet name=NameHitboxInBullet.Null)
    {
        // base.Awake();
        // depeninding on the name we should change how we create the hitbox
        hitboxUsed=Instantiate(prefabHitboxBox,baseBullets.gameObject.transform.position,baseBullets.gameObject.transform.rotation,baseBullets.gameObject.transform);
        baseBullets.GetsNewID(hitboxUsed, name);

        if (name==NameHitboxInBullet.Detectors)
        {
            hitboxUsed.GetComponent<HitboxBullet>().DeactivateCollider();
        }
       /*  baseBullets.dicCollisionEnter[hitboxBulletInst]+=HitboxCollisionEnter;
        baseBullets.dicTriggerEnter[hitboxBulletInst]+=HitboxTriggerEnter;  */ 
    }

    
    

    protected override void OnDestroy() {
        
        baseBullets.DeletesThisID(hitboxUsed);
        //probably deletes the bullet to
    }
}

//add hitbox that will be activated after impact
public class AddHitboxAfterImpactEB: AddHitboxEB
{
    protected override void Awake()
    {
        base.Awake();
        baseBullets.GetComponent<DamageEB>().OnImpact+=()=> hitboxUsed.SetActive(true);
    }
    public override void Setup(NameHitboxInBullet name = NameHitboxInBullet.Null)
    {
        base.Setup(name);
        hitboxUsed.SetActive(false);

    }
}

public class AddHitboxAfterDeathEB: AddHitboxEB
{
    protected override void Awake()
    {
        base.Awake();
        baseBullets.GetComponent<DamageEB>().OnImpact+=()=> hitboxUsed.SetActive(true);
    }
    public override void Setup(NameHitboxInBullet name = NameHitboxInBullet.Null)
    {
        base.Setup(name);
        hitboxUsed.SetActive(false);

    }
}

