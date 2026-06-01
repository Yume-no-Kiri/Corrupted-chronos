using Ink;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;


[Serializable]
public enum ClassBullet
{
    Null=0, 
    Projectil=1,
    Raygun=2
}

public class BaseBullets : MonoBehaviour
{
    [HideInInspector] public Vector3 iniPos;
    private Vector3 LastPos;
    private float DistanceTravelled=0f;
    private bool UseTimerToDie=false;
    private Coroutine CoroutineTimerToDie=null;


    private ClassBullet myClassBullet=ClassBullet.Null;
    public GameObject myCreator;
    private Rigidbody rb;
    private SpriteRenderer spr;

    public AllInformationBullet AllInfoBullet{get; private set;}

    private float distBeforeEffect=0.2f;
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

    // public Dictionary<GameObject,Action<Collision>> dicCollisionEnter = new Dictionary<GameObject,Action<Collision>>();
    // public Dictionary<GameObject,Action<Collision>> dicCollisionExit = new Dictionary<GameObject,Action<Collision>>();

    public Dictionary<GameObject,Action<Collider>> dicTriggerEnter = new Dictionary<GameObject,Action<Collider>>();
    public Dictionary<GameObject,Action<Collider>> dicTriggerExit = new Dictionary<GameObject,Action<Collider>>();




    public List<GameObject> hitboxBullets= new List<GameObject>();
    public Dictionary<NameHitboxInBullet, GameObject> NameHitboxBullets= new Dictionary<NameHitboxInBullet, GameObject>();

    public Dictionary<NameHitboxInBullet, DamageEB> DamageHitbox= new Dictionary<NameHitboxInBullet, DamageEB>();
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
        LastPos=iniPos;
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
        myClassBullet=newBulletSO.classBullet;
        // newBulletSO.Debuger();
        Debug.Log("scale arrived inside basebullet");

        // this.transform.localScale*=AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSize];
        spr.gameObject.transform.localScale *=AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSize];
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

    public void ChangeDistanceToTimer()
    {
        UseTimerToDie=true;
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
            // return null;
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
        // dicCollisionEnter[Hitbox] = delegate { };
        dicTriggerEnter[Hitbox] = delegate { };
       
        /* newScript.OnCollisionEnterHitbox+= (col,hb) =>{
        if (dicCollisionEnter.ContainsKey(hb)) 
            dicCollisionEnter[hb]?.Invoke(col);
        }; */

        newScript.OnTriggerEnterHitbox+= (col,hb)=>{
        if (dicTriggerEnter.ContainsKey(hb)) 
            dicTriggerEnter[hb]?.Invoke(col);
        };
        
        // dicCollisionExit[Hitbox] = delegate { };
        dicTriggerExit[Hitbox] = delegate { };
       
       /*  newScript.OnCollisionExitHitbox+= (col,hb) =>{
        if (dicCollisionExit.ContainsKey(hb)) 
            dicCollisionExit[hb]?.Invoke(col);
        }; */

        newScript.OnTriggerExitHitbox+= (col,hb)=>{
        if (dicTriggerExit.ContainsKey(hb)) 
            dicTriggerExit[hb]?.Invoke(col);
        };

        // return index;
    }

    public void DeletesThisID(GameObject Hitbox)
    {
        /* 
        when hitbox or effect bullet gets deleted, this delets all the actions and information releted
         */
        HitboxBullet oldScript= Hitbox.GetComponent<HitboxBullet>();

        if (dicTriggerEnter.ContainsKey(Hitbox) && dicTriggerExit.ContainsKey(Hitbox))
        {
            // oldScript.OnCollisionEnterHitbox-= (col,hb) =>dicCollisionEnter[hb]?.Invoke(col);
            oldScript.OnTriggerEnterHitbox-= (col,hb)=> dicTriggerEnter[hb]?.Invoke(col);
            oldScript.OnTriggerExitHitbox-= (col,hb)=> dicTriggerExit[hb]?.Invoke(col);

        }
        
        hitboxBullets.Remove(Hitbox);

        Destroy(Hitbox);

    }


    #endregion 


    #region life and invokers
    protected void FixedUpdate()
    {
        if(myClassBullet==ClassBullet.Null) return;
        if(AllInfoBullet.IsEmpty()) return;
        switch (myClassBullet){
            case ClassBullet.Projectil:
                LogicProjectile();  
            break;
            
        }

        
            // if(StatsBullet==null) return;
        

    }

    private void LogicProjectile()
    {
        Debug.LogWarning("statsbullet speed:" + AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSpeed]);
        rb.linearVelocity = transform.forward * AllInfoBullet.StatsBullet[Stat.StatTypeBullet.BulletSpeed];
        // float distance = Vector3.Distance(iniPos, this.transform.position);
        float distance = Vector3.Distance(this.transform.position, LastPos);
        


        DistanceTravelled += distance;
        LastPos = transform.position;
        //Debug.Log("Distance: " + distance +"__iniPos: "+iniPos+ "__transform.position: " + this.transform.position);
        if (DistanceTravelled> distBeforeEffect && DistanceTravelled < AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistEffec])
        {
            OnEffectiveRange?.Invoke();

        }
        else if (DistanceTravelled > AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistEffec] && DistanceTravelled < AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistMax])
        {
            //should pass how far are we from distEffect?
            OnDecliveRange?.Invoke();
        }
        else if ( DistanceTravelled >= AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistMax])
        {
            OnMaxRange?.Invoke();
            //should pass how far are we from distMax?
            AwayDistMax(DistanceTravelled);
            // Debug.LogWarning("SHOULD DELETE BULLET");
        }

        if (UseTimerToDie)
        {
            DieWithTimer();

        }
    }

    public void DieWithTimer()
    {
        if (CoroutineTimerToDie == null)
        {
            CoroutineTimerToDie = StartCoroutine(TimerToDie());
        }
    }

    private IEnumerator TimerToDie()
    {
        float timer= AllInfoBullet.StatsBullet[Stat.StatTypeBullet.DistMax]/2;
        yield return new WaitForSecondsRealtime( timer);
        Debug.Log("projecitle die to time, timer  equals: "+ timer);
        AwayDistMax(DistanceTravelled);
        CoroutineTimerToDie=null;
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



