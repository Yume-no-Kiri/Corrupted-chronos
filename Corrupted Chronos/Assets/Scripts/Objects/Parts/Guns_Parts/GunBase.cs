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
public enum NameGunPreset
{
    Null=0,
    Metralleta=1,
    Escopeta=2

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
    public NameBulletPreset NameStatsBullet;

    // protected VisualEffect shoot_vfx;
    // protected List<AllEffectsBullets> addedEffects= new List<AllEffectsBullets>();
    protected List<EffectsAdd> addedEffects= new List<EffectsAdd>();

    protected bool canShot = true;
    protected bool isPressed=false;

    protected AllInformationBullet BaseStatsBullet;
     



    protected override void OnEnable()
    {
        // ObjectNameID="";
        // = 1f;
        DefineBulletStats();
        
    }
    void Update() { }
    
    #region items addefects
    public void AddEffectsBullets(List<EffectsAdd> effectsAdds)
    {
        if(effectsAdds!=null){
            addedEffects.Concat(effectsAdds);   
        }
    }
    public void RemoveEffectsBullets(List<EffectsAdd> effectsAdds)
    {
        if(effectsAdds!=null){
            addedEffects.Concat(effectsAdds);
        }
    }
    #endregion


    #region bullet and shot
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
            Type type=Type.GetType(item.nameEffect.ToString());
            Component script=bulletInstance.Last().AddComponent(type);
            EffectsBullets effectsBullets=script as EffectsBullets;
            if (effectsBullets!=null)
            {
                effectsBullets.Setup(item.setup);
            }
            // bulletInstance.Last().AddComponent(Type.GetType(item.ToString()));
        }
   
    }
    #endregion

    #region call from other scripts
    public TypePart GetTypePart()
    {
        return typePart;
    }
    public void PassVariables(Transform firepoint,GameObject bulletPrefab)
    {
        this.firepoint.Add(firepoint);
        this.bulletPrefab = bulletPrefab;
    }
    public void AssignIDP(int newIDP)
    {
        IDP=newIDP;
    }
    public int ReturnIDP()
    {
        return IDP;
    }
    #endregion

    protected override void DefineModifierItem()
    {
        return;
        // throw new NotImplementedException();
    }


    protected float GetTotalStat(Stat.StatTypeGeneral statType)
    {
        return statsManager.instance.GetShipGunBulletStat(statType, IDP, BaseStatsBullet);
    }
}

//disparar continuadament empitjora la precisió
public class Metralleta : GunBase
{
    Coroutine coroutine=null;
    // float timeBetweenShots=0.3f;
    // bool cnShot=true;

    float timeShooting=0;
    protected override void OnEnable()
    {
        ObjectNameID="MetralletaObject";
        NameStatsBullet=NameBulletPreset.Basic;
        base.OnEnable();
    }

    private void Awake()
    {
        typePart= TypePart.ShootableLeft;
    }
   

    public override void DoShot(InputAction.CallbackContext ctx)
    {
        if ( ctx.performed)
        {
            Debug.Log("left performed");
            Shot();
            if(coroutine!=null) StopCoroutine(coroutine);
            coroutine=StartCoroutine(ShotIE());

        }else if (ctx.canceled)
        {   
            Debug.Log("left cancelled");
            if(coroutine!=null) StopCoroutine(coroutine);

        }
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

    public void Shot()
    {
        foreach (var item in firepoint)
        {
            float accu= GetTotalStat(Stat.StatTypeGeneral.Accuraccy);
            float side=UnityEngine.Random.Range(-1,2);
            float maxDisp= timeShooting*side;

            float t = UnityEngine.Random.value; 
            t = Mathf.Pow(t, accu/2); //com més gran sigui l'exponent, més "biaix" cap al mínim
            if(maxDisp>80) maxDisp=80; 
            float finalDisp= Mathf.Lerp(timeShooting/10*side, maxDisp, t);
            Debug.Log("left disp: "+ maxDisp);

            /* if(disp>0 && disp < accu)
            {
                float howLittle=accu/disp +2;
                float correction= accu/howLittle;
                disp-=correction;
                Debug.Log("left first if disp:"+ disp);
            }
            else if(disp!=0 && disp>accu && disp<(accu*2))
            {
                float correction= accu/2;
                disp-=correction;
                Debug.Log("left second if disp:"+ disp);

            }
            else if(disp!=0 &&disp>accu && disp>(accu*2))
            {
                disp-=accu;
                Debug.Log("left third if disp:"+ disp);
            
            } */

            Quaternion rotationWithOffset = item.GetComponentInParent<Transform>().rotation * Quaternion.Euler(0, finalDisp, 0);

            CreateBullet(item.position,rotationWithOffset);

            // bulletInstanceInstantiate(bulletPrefab, firepoint.position, rotationWithOffset);
            // BaseBullets bulletInfo = bulletInstance.Last().GetComponent<BaseBullets>();
            // bulletInfo.DefinirBala(1, +3);
            
            bulletInstance= new List<GameObject>();
        }
       
    }

    private IEnumerator ShotIE()
    {
        /* canShot = false;
        //Debug.Log($"t2s: {t2s-t2s %PlayerStats.CooldownBalaJugador}");
        yield return new WaitForSeconds(statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.TimeBetweenShots, IDP, BaseStatsBullet));// StatsGun.timeBetweenShots);
        canShot = true; */
        timeShooting=0;
        Shot();
        yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));//statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.TimeBetweenShots, IDP, BaseStatsBullet));
        while (true)
        {
            timeShooting+= Mathf.Pow(10,Time.deltaTime);
            Shot();
            yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
        }


    }
    
    
}

//si al aire, fa knockback, dispara moltes bales, munició 2 carges
public class Escopeta : GunBase
{
    protected override void OnEnable()
    {
        ObjectNameID="EscopetaObject";
        NameStatsBullet=NameBulletPreset.Basic;
        DefineBulletStats();
    }

    void Awake()
    {
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
    }

    private void Shot()
    {
        float accu=GetTotalStat(Stat.StatTypeGeneral.Accuraccy);
        float valor= 10-Mathf.Sqrt(accu);
        if(valor <3) valor= 3;
        // int valor = 10;
        int nbullets = 5;
        float maxDisp= (nbullets-1)/2*valor;
         foreach (var item in firepoint)
        {
            for (int i = 0; i < nbullets; i++)
            {
                valor *= i;
                Quaternion rotationWithOffset = item.GetComponentInParent<Transform>().rotation * Quaternion.Euler(0,(-maxDisp+valor),0 );//(-20+valor) ,0);

                CreateBullet(item.position,rotationWithOffset);
                // valor =10;
                valor= 10-Mathf.Sqrt(accu);

            }
        }
        bulletInstance= new List<GameObject>();
        if (GameManager.Instance.playerInstance.TryGetComponent<IDamageable>(out IDamageable player))
        {
            foreach(var fp in firepoint)
            {
                Vector3 direccio =GameManager.Instance.playerInstance.transform.position-  fp.transform.position ;

                player.AddKnockback(direccio, GetTotalStat(Stat.StatTypeGeneral.Knockback)/1.4f);
            }
        }

    }

    private IEnumerator ShotIE()
    {
        canShot = false;
        yield return new WaitForSeconds(statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.TimeBetweenShots, IDP, BaseStatsBullet));
        canShot = true;
    }   
}