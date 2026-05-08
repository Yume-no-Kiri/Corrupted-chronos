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

/* public class AddHitboxList : MonoBehaviour
{
    public GameObject BoxCollider;
    public GameObject SphereCollider;
} */
public class AddHitboxEB: EffectsBullets
{

    // protected GameObject hitboxBulletInst;
    public NameHitboxInBullet name;
    public override void Setup(NameHitboxInBullet name=NameHitboxInBullet.Null)
    {
        // base.Awake();
        // depeninding on the name we should change how we create the hitbox
        CreateHitbox();
        baseBullets.GetsNewID(hitboxUsed, name);

        switch (name)
        {
            case NameHitboxInBullet.Detectors:

                // hitboxUsed.GetComponent<HitboxBullet>().DeactivateCollider();
                hitboxUsed.GetComponent<HitboxBullet>().DefineTriggerSize(new Vector3(5, 5, 5));
                break;
            case NameHitboxInBullet.AfterImpact:

                baseBullets.GetComponent<DamageEB>().OnImpact += () => hitboxUsed.SetActive(true);
                hitboxUsed.SetActive(false);
                break;
            case NameHitboxInBullet.AfterDeath:
                /* baseBullets.GetComponent<DamageEB>().Onde+=()=> hitboxUsed.SetActive(true);
                hitboxUsed.SetActive(false); */
                break;
            default:
                break;
        }


        /*  baseBullets.dicCollisionEnter[hitboxBulletInst]+=HitboxCollisionEnter;
         baseBullets.dicTriggerEnter[hitboxBulletInst]+=HitboxTriggerEnter;  */
    }

    protected virtual void CreateHitbox()
    {
        hitboxUsed = Instantiate(prefabHitboxBox, baseBullets.gameObject.transform.position, baseBullets.gameObject.transform.rotation, baseBullets.gameObject.transform);
    }



    protected override void OnDestroy() {
        
        baseBullets.DeletesThisID(hitboxUsed);
        //probably deletes the bullet to
    }
}


public class AddHitsphereEB: AddHitboxEB
{
    protected override void CreateHitbox()
    {
        // base.CreateHitbox();
        hitboxUsed = Instantiate(prefabHitboxSphere, baseBullets.gameObject.transform.position, baseBullets.gameObject.transform.rotation, baseBullets.gameObject.transform);

    }
}

//add hitbox that will be activated after impact
/* public class AddHitboxAfterImpactEB: AddHitboxEB
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Setup(NameHitboxInBullet name = NameHitboxInBullet.Null)
    {
        base.Setup(name);

    }
} */

/* public class AddHitboxAfterDeathEB: AddHitboxEB
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

 */