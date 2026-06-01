using System.Collections;
using UnityEngine;

public enum NameHitboxInBullet
{
    Null=0,
    Front=1,
    Body=2,
    Tail=3, 
    AfterDeath=4, //have travelled and don't impacted
    // AfterImpact=5, //have travelled and impacted
    Detectors=6
}

/* public class AddHitboxList : MonoBehaviour
{
    public GameObject BoxCollider;
    public GameObject SphereCollider;
} */
public class AddHitboxEB: EffectsBullets
{

    // protected GameObject hitboxBulletInst;
    [HideInInspector] public NameHitboxInBullet name;
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
        Debug.Log("scale arrived inside hitbox");
        hitboxUsed = Instantiate(prefabHitboxBox, baseBullets.gameObject.transform.position, baseBullets.gameObject.transform.rotation, baseBullets.gameObject.transform);
        hitboxUsed.transform.localScale*= baseBullets.AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSize];
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
        hitboxUsed.transform.localScale*= baseBullets.AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSize];

    }
}
