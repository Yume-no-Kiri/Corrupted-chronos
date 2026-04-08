using Ink;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

//probablement implementar un enum per diferents tipus de Bales (No creo que sea necesario)
//potser canviar-li el nom

//classe general, del que venen els diferents projectils, revisar en un futur
public struct InformationBullet
{
    public int damage;
    public float penetration;
    public float distEffec;
    public float distMax;
    public float speed;
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
                damage = so.damage,
                penetration = so.penetration,
                distEffec = so.distEffective,
                distMax = so.distMax,
                speed = so.speed,
                knockback= so.knockback
            };
        }
    }

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

    public void PlusDamage(int nouMal)
    {
        damage+=nouMal;
    }
    public void PlusDistMaz(float dist)
    {
        distMax+=dist;
    }
}


public class BaseBullets : MonoBehaviour
{
    [HideInInspector] public Vector3 iniPos;

    public GameObject myCreator;
    private Rigidbody rb;
    private SpriteRenderer spr;

    public InformationBullet StatsBullet{get; private set;}
    [HideInInspector] public BulletStatsSO statsSO;

    // public GameObject FirstHitboxBullet;

    public event Action OnEffectiveRange;
    public event Action OnDecliveRange;
    public event Action OnMaxRange;
    public Dictionary<GameObject,Action<Collision>> dicCollisionEnter = new Dictionary<GameObject,Action<Collision>>();
    public Dictionary<GameObject,Action<Collider>> dicTriggerEnter = new Dictionary<GameObject,Action<Collider>>();



    public List<GameObject> hitboxBullets= new List<GameObject>();

    // public InformationBullet StatsBullet { get => statsBullet; set => statsBullet = value; }

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        spr = GetComponentInChildren<SpriteRenderer>();

    }

    public void AssignSO(BulletStatsSO newBulletSO)
    {
        statsSO= newBulletSO;
        StatsBullet = InformationBullet.Default(statsSO);
        if (statsSO.sprite != null)
        {
            spr.sprite = statsSO.sprite;
        }
    }

    #region add effect collider
    public void GetsNewID(GameObject Hitbox)
    {
        //centrilized colliders reciever
        /*An effect bullet creates a new htibox and sents (OnCollisionEnterHitbox, OnTriggerEnterHitbox) when detect something,
        this script recieve it and send (dicCollisionEnter[hitbox.gameObject], dicTriggerEnter[hitbox.gameObject]),
        the effect bullet that created the new hitbox recieve (dicCollisionEnter[hitbox.gameObject], dicTriggerEnter[hitbox.gameObject])
        and does what it should do
        */

        HitboxBullet newScript= Hitbox.GetComponent<HitboxBullet>();
        hitboxBullets.Add(Hitbox);

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

    //should change and upgrade this funciton with what it needs the next modular effects for the bullets
   /*  public void addNewHitboxBullet(HitboxBullet newHitboxBullet){
        hitboxBullets.Add( newHitboxBullet); 
    } */
    

    protected virtual void Start()
    {
        iniPos = this.transform.position;
    }

    protected void FixedUpdate()
    {
        // if(StatsBullet==null) return;
        Debug.LogWarning("statsbullet speed:"+StatsBullet.speed);
        rb.linearVelocity = transform.forward * StatsBullet.speed;
        float distance = Vector3.Distance(iniPos, this.transform.position);
        //Debug.Log("Distance: " + distance +"__iniPos: "+iniPos+ "__transform.position: " + this.transform.position);
        if(distance< StatsBullet.distEffec)
        {
            OnEffectiveRange?.Invoke();

        }else if(distance> StatsBullet.distEffec&& distance < StatsBullet.distMax)
        {
            //should pass how far are we from distEffect?
            OnDecliveRange?.Invoke();
        } else if(distance > StatsBullet.distMax)
        {
            OnMaxRange?.Invoke();
            //should pass how far are we from distMax?
            AwayDistMax(distance);
            Debug.LogWarning("SHOULD DELETE BULLET");
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

    public int ReturnDamage()
    {
        return StatsBullet.damage;
    }
    public void DefinirBala(int nouMalBala, float distancia)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");
        // StatsBullet.damage+= nouMalBala;
        StatsBullet.PlusDamage(nouMalBala);
        // StatsBullet.damage += nouMalBala;
        StatsBullet.PlusDistMaz( distancia);
        //Debug.Log($"mal1 {mal} naumal1{nouMalBala}");

    }
    
    public void DefinirBala(InformationBullet statsBase)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");

        StatsBullet=statsBase;
        //Debug.Log($"mal1 {mal} naumal1{nouMalBala}");
    }  
}



