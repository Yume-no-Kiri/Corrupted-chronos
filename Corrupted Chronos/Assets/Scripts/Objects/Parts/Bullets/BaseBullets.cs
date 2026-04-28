using Ink;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

//probablement implementar un enum per diferents tipus de Bales (No creo que sea necesario)
//potser canviar-li el nom

//classe general, del que venen els diferents projectils, revisar en un futur
/* public struct InformationBullet
{
    public int damage;
    public float speed;
    public float penetration;
    public float distEffec;
    public float distMax;
    public float knockback;

    public static InformationBullet Default(BulletStatsSO so)
    {
        if (so == null)
        {
             return new InformationBullet
            {
                damage = 0,
                penetration = 0,
                distEffec = 0,
                distMax = 0,
                speed = 0,
                knockback= 0
            };
        }else{
            return new InformationBullet
            {
                damage = so.Damage,
                penetration = so.Penetration,
                distEffec = so.DistEffec,
                distMax = so.DistMax,
                speed = so.AttackSpeed,
                knockback= so.Knockback
            };
        }
    } */

   /*  public static InformationBullet empty()
    {
        return new InformationBullet
        {
            damage=0,
            penetration=0,
            distEffec=0,
            distMax = so.distMax,
            speed = so.speed,
            knockback= so.knockback
        }
    } */

   /*  public void PlusDamage(int nouMal)
    {
        damage+=nouMal;
    }
    public void PlusDistMaz(float dist)
    {
        distMax+=dist;
    }
} */


public class BaseBullets : MonoBehaviour
{
    [HideInInspector] public Vector3 iniPos;

    public GameObject myCreator;
    private Rigidbody rb;
    private SpriteRenderer spr;

    public AllInformationBullet AllInfoBullet{get; private set;}
    // [HideInInspector] public Dictionary<Stat.StatTypeBullet, Stat> statsSO;

    // public GameObject FirstHitboxBullet;

    private string EnemyTag="Enemy";
    private string PlayerTag="Player";
    [HideInInspector]public string OpposedTag;
    [HideInInspector]public string CreatedTag;
    private bool fromPlayer;

    public event Action OnEffectiveRange;
    public event Action OnDecliveRange;
    public event Action OnMaxRange;

    public Dictionary<GameObject,Action<Collision>> dicCollisionEnter = new Dictionary<GameObject,Action<Collision>>();
    public Dictionary<GameObject,Action<Collider>> dicTriggerEnter = new Dictionary<GameObject,Action<Collider>>();



    public List<GameObject> hitboxBullets= new List<GameObject>();
    public Dictionary<NameHitboxInBullet, GameObject> NameHitboxBullets= new Dictionary<NameHitboxInBullet, GameObject>();

    // public InformationBullet StatsBullet { get => statsBullet; set => statsBullet = value; }

    private void Awake()
    {
        AllInfoBullet=new AllInformationBullet();
        rb = GetComponent<Rigidbody>();
        spr = GetComponentInChildren<SpriteRenderer>();

    }
    protected virtual void Start()
    {
        iniPos = this.transform.position;
    }

    #region assigners
    public void AssignSO(AllInformationBullet newBulletSO)
    {
        // statsSO= newBulletSO.StatsBullet;
        AllInfoBullet=newBulletSO;
        // StatsBullet = AllInformationBullet.Default(statsSO);
        if (AllInfoBullet.sprite != null)
        {
            spr.sprite = AllInfoBullet.sprite;
        }
        newBulletSO.Debuger();

    }

    public void AssignTarget(string Owner)
    {
        CreatedTag=Owner;
        if (Owner == EnemyTag) {
            OpposedTag=PlayerTag;
            fromPlayer=false;
        }
        else {
            OpposedTag=EnemyTag;
            fromPlayer=true;
        }
    }

    #endregion

    #region returners
    public GameObject GetLastHitbox()
    {
        return hitboxBullets.Last();
    }
 
    public GameObject GetNametHitbox(NameHitboxInBullet name)
    {
        if (NameHitboxBullets.ContainsKey(name))
        {
            return NameHitboxBullets[name];
        }
        else
        {
            throw new Exception("no existe {name.ToString} hitbox");
        }
    }
    public float ReturnDamage()
    {
        return AllInfoBullet.StatsBullet[Stat.StatTypeBullet.Damage];
    }

    public void ReturnTags(out string opposedTag,out string createdTag, out bool isFromPlayer)
    {
        opposedTag=OpposedTag;
        createdTag=CreatedTag;
        isFromPlayer= fromPlayer;
    }
    #endregion

    #region add effect collider
    public void GetsNewID(GameObject Hitbox, NameHitboxInBullet name)
    {
        //centrilized colliders reciever
        /*An effect bullet creates a new htibox and sents (OnCollisionEnterHitbox, OnTriggerEnterHitbox) when detect something,
        this script recieve it and send (dicCollisionEnter[hitbox.gameObject], dicTriggerEnter[hitbox.gameObject]),
        multiple effect bullets can be subscirved to the same collider
        some of this more important colliders are saves with a name, in a dictionary 
        */

        HitboxBullet newScript= Hitbox.GetComponent<HitboxBullet>();
        hitboxBullets.Add(Hitbox);
        if (!NameHitboxBullets.ContainsKey(name))
        {
            //segons els noms fer diferentes coses
            NameHitboxBullets.Add(name, Hitbox);
            switch (name)
            {
                case NameHitboxInBullet.Null:
                    Debug.LogError("this shouldn't happen");
                    break;
                /* case NameHitboxInBullet.AfterDeath:
                    Hitbox.SetActive(false);
                    break;*/
                default: 
                break;
            }

        }
        else
        {
            Hitbox.transform.SetParent(NameHitboxBullets[name].transform);
            Debug.LogError("revisar això pot ser problema");
            //perque ja existeix un collider per això, pot ser innecesari
        }

        //revisar això
        dicCollisionEnter[Hitbox] = delegate { };
        dicTriggerEnter[Hitbox] = delegate { };
       
        newScript.OnCollisionEnterHitbox+= (col,hb) =>{
        if (dicCollisionEnter.ContainsKey(hb)) 
            dicCollisionEnter[hb]?.Invoke(col);
        };

        newScript.OnTriggerEnterHitbox+= (col,hb)=>{
        if (dicTriggerEnter.ContainsKey(hb)) 
            dicTriggerEnter[hb]?.Invoke(col);
        };
        
        // return index;
    }

    public void DeletesThisID(GameObject Hitbox)
    {
        /* 
        when hitbox or effect bullet gets deleted, this delets all the actions and information releted
         */
        HitboxBullet oldScript= Hitbox.GetComponent<HitboxBullet>();

        if (dicCollisionEnter.ContainsKey(Hitbox))
        {
            oldScript.OnCollisionEnterHitbox-= (col,hb) =>dicCollisionEnter[hb]?.Invoke(col);
            oldScript.OnTriggerEnterHitbox-= (col,hb)=> dicTriggerEnter[hb]?.Invoke(col);
        }
        
        hitboxBullets.Remove(Hitbox);

        Destroy(Hitbox);

    }


    #endregion 


    #region invokers
    protected void FixedUpdate()
    {
        if(!AllInfoBullet.IsEmpty())
        {
            // if(StatsBullet==null) return;
            Debug.LogWarning("statsbullet speed:"+AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSpeed]);
            rb.linearVelocity = transform.forward * AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSpeed];
            float distance = Vector3.Distance(iniPos, this.transform.position);
            //Debug.Log("Distance: " + distance +"__iniPos: "+iniPos+ "__transform.position: " + this.transform.position);
            if(distance< AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistEffec])
            {
                OnEffectiveRange?.Invoke();

            }else if(distance> AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistEffec]&& distance < AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistMax])
            {
                //should pass how far are we from distEffect?
                OnDecliveRange?.Invoke();
            } else if(distance > AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistMax])
            {
                OnMaxRange?.Invoke();
                //should pass how far are we from distMax?
                AwayDistMax(distance);
                // Debug.LogWarning("SHOULD DELETE BULLET");
            }
        }
     
    }
    protected virtual void AwayDistMax(float distance)
    {
        Destroy(this.gameObject);
    }


    protected virtual void AwayDistEffec (float distance)
    {
        //reduction damage or something
    }

    public AllInformationBullet ReturnFinalStats()
    {
        return AllInfoBullet;
    }

    #endregion


}



