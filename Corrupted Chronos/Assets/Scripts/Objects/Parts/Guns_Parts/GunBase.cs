using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    TIENE TODOS LOS CODIGOS ARMAS 
*/
public enum TypePart
{
    ShootableLeft,
    ShootableRight,
    Mele,
    Moveable
    
}
[Serializable]
/* public struct InformationPart
{
    //start capa1
    public InformationBullet informationBullet; */
    /* public float timeBetweenShots;// = 1f;
    public float magazine;
    public float accuraccy;
    
    public float t2s2;// = 1f;
    public int cargador;// = 5;
    public int contCargador;// = 5; */

    // public float timeBetweenShots;


    //fin capa1 

    /* public static InformationBullet Default(BulletStatsSO so)
    {
        return new InformationBullet
        {
            damage = so.damage,
            penetration = so.penetration,
            distEffec = so.distEffective,
            distMax = so.distMax,
            speed = so.speed,
            knockback= so.knockback
        };
    } */

    //més variables de part
// }


//classe general, del que venen les diferents parts, revisar en un futur
public class GunBase : AllObjectMB
{
    
    //private InputManager inputManager;
    protected TypePart typePart;

    protected int IDP;

    //potser guardar en llista si s'hagues de fer algo especial no se
    protected List<GameObject> bulletInstance=new List<GameObject>();

    protected List<Transform> firepoint= new List<Transform>();
    protected GameObject bulletPrefab;
    public string NameStatsBullet;

    // protected VisualEffect shoot_vfx;
    // protected List<AllEffectsBullets> addedEffects= new List<AllEffectsBullets>();
    protected List<EffectsAdd> addedEffects= new List<EffectsAdd>();

    protected bool canShot = true;

    protected AllInformationBullet BaseStatsBullet;
     
    // public InformationPart StatsGun;

    // Dictionary<Stat.StatTypeGun, Stat> NewStatsGun;

    // hauriem de tenir algo per l'inventari

    //to do damage tag


    public TypePart GetTypePart()
    {
        return typePart;
    }
    
    protected override void OnEnable()
    {
        ObjectNameID="";
        // = 1f;
        
    }
    void Update() { }

    public virtual void DoShot( InputAction.CallbackContext ctx)
    {
        
        throw new System.NotImplementedException();
    }
    protected void DefineBulletStats(){
        AllInformationBullet? allInformationBullet= statsManager.instance.listBulletStats.ReturnBulletStatsSO(NameStatsBullet);

        if (allInformationBullet != null)
        {
            BaseStatsBullet= allInformationBullet.Value;  
        }else Debug.LogError("bullet not found");
    }
    public void PassVariables(Transform firepoint,GameObject bulletPrefab)
    {
        this.firepoint.Add(firepoint);
        this.bulletPrefab = bulletPrefab;
    }

    //quan una arma crea una bala només crida aquest mètode 
    public void CreateBullet(Vector3 spawnPoint, quaternion rotation)
    {
        //crear bullet prefab
        AllInformationBullet finalStats= statsManager.instance.ReturnFinalStatsBullet(IDP, BaseStatsBullet);
        

        bulletInstance.Add(Instantiate(bulletPrefab, spawnPoint, rotation));
        bulletInstance.Last().GetComponent<CreateBullet>().Setup(true,finalStats);
        //afegir effectes de items
        foreach (var item in addedEffects)
        {
            Type type=Type.GetType(item.nameEffect);
            Component script=bulletInstance.Last().AddComponent(type);
            EffectsBullets effectsBullets=script as EffectsBullets;
            if (effectsBullets!=null)
            {
                effectsBullets.Setup(item.setup);
            }
            // bulletInstance.Last().AddComponent(Type.GetType(item.ToString()));
        }

    
        
    }
    public void AssignIDP(int newIDP)
    {
        IDP=newIDP;
    }
    public int ReturnIDP()
    {
        return IDP;
    }
    
}

public class Metralleta : GunBase
{
    protected override void OnEnable()
    {
        ObjectNameID="MetralletaObject";
        NameStatsBullet="BasicBullet";
        DefineBulletStats();
    }

    void Awake()
    {
        /* StatsGun.timeBetweenShots = 0.25f;
        StatsGun.t2s2=0;
        StatsGun.cargador = -1;
        StatsGun.contCargador=-1;
        StatsGun.informationBullet=InformationBullet.Default(null); */
        
        // public int contCargador = 5;
        
        typePart= TypePart.ShootableLeft;
    }

    /*protected override void saveVariablesShot()
    {
        
    }*/
    
    
    public override void DoShot(InputAction.CallbackContext ctx)
    {
        if (canShot && ctx.performed)
        {
            Debug.Log("Transfrom.position: " + transform.position);
            Shot();
            StartCoroutine(ShotIE());
        }
        
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

    public void Shot()
    {
        foreach (var item in firepoint)
        {
            Quaternion rotationWithOffset = item.GetComponentInParent<Transform>().rotation * Quaternion.Euler(0, 90, 90);

            CreateBullet(item.position,rotationWithOffset);

            // bulletInstanceInstantiate(bulletPrefab, firepoint.position, rotationWithOffset);
            // BaseBullets bulletInfo = bulletInstance.Last().GetComponent<BaseBullets>();
            // bulletInfo.DefinirBala(1, +3);
            
            bulletInstance= new List<GameObject>();
        }
       
    }

    private IEnumerator ShotIE()
    {
        
        canShot = false;
        //Debug.Log($"t2s: {t2s-t2s %PlayerStats.CooldownBalaJugador}");
        yield return new WaitForSeconds(statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.TimeBetweenShots, IDP, BaseStatsBullet));// StatsGun.timeBetweenShots);
        canShot = true;
    }
    
    
}

public class Escopeta : GunBase
{
    protected override void OnEnable()
    {
        ObjectNameID="EscopetaObject";
        NameStatsBullet="BasicBullet";
        DefineBulletStats();

    }

    void Awake()
    {
        /* StatsGun.timeBetweenShots = 1f;
        StatsGun.t2s2=0;
        StatsGun.cargador = -1;
        StatsGun.contCargador=-1;
        StatsGun.informationBullet=InformationBullet.Default(null);
 */
        typePart= TypePart.ShootableRight;
    }

    public override void DoShot(InputAction.CallbackContext ctx)
    {
        if (canShot && ctx.performed)
        {
            Debug.Log("Transfrom.position: " + transform.position);
            Shot();
            StartCoroutine(ShotIE());
        }
        
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

    private void Shot()
    {
        int valor = 10;
        int nbullets = 5;
         foreach (var item in firepoint)
        {
            for (int i = 0; i < nbullets; i++)
            {
                valor *= i;
                Quaternion rotationWithOffset = item.GetComponentInParent<Transform>().rotation * Quaternion.Euler((-20+valor), 90, 90);

                CreateBullet(item.position,rotationWithOffset);
                /* bulletInstance = Instantiate(bulletPrefab, firepoint.position, rotationWithOffset);
                BaseBullets bulletInfo = bulletInstance.GetComponent<BaseBullets>(); */

                // bulletInstance.GetComponent<CreateBullet>()
                
                // .allPresetBullets= Activate();

                // bulletInfo.DefinirBala(1, -8 );
                // bulletInstance.Last().gameObject.SetActive(false);
                valor =10;

            }
        }
        bulletInstance= new List<GameObject>();

    }

    private IEnumerator ShotIE()
    {
        
       /*  for (int i = 0; i < nbullets; i++)
        {
            bulletInstance[i].gameObject.SetActive(true);
        } */
        
        canShot = false;
        yield return new WaitForSeconds(statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.TimeBetweenShots, IDP, BaseStatsBullet));
        canShot = true;
    }   
}