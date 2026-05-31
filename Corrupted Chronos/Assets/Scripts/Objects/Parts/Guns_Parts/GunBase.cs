using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
public enum ListNameParts
{
    Null=0,
    Metralleta=1,
    Escopeta=2,
    Flamethrower=3

}

/*  */


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
    protected SpriteRenderer sprite;
    public NameBulletPreset NameStatsBullet;

    // protected VisualEffect shoot_vfx;
    // protected List<AllEffectsBullets> addedEffects= new List<AllEffectsBullets>();
    protected HashSet<EffectsAdd> addedEffects= new HashSet<EffectsAdd>();

    protected bool canShot = true;
    protected bool isPressed=false;

    protected AllInformationBullet BaseStatsBullet;
     



    protected override void OnEnable()
    {
        // ObjectNameID="";
        // = 1f;
        bulletPrefab= statsManager.instance.listBulletStats.GeneralBullet;
        DefineBulletStats();
        
    }
    void Update() { }
    
    #region items addefects
    public void AddEffectsBullets(HashSet<EffectsAdd> effectsAdds)
    {
        if(effectsAdds!=null){
            addedEffects.UnionWith(effectsAdds);   

        }
    }
    public void RemoveEffectsBullets(HashSet<EffectsAdd> effectsAdds)
    {
        if(effectsAdds!=null){
            addedEffects.ExceptWith(effectsAdds);
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
        finalStats = ModifingFinalStats( finalStats);
        bulletInstance.Last().GetComponent<CreateBullet>().Setup(true,finalStats);
        // bulletInstance.Last().transform.localScale*=finalStats.StatsBullet[Stat.StatTypeBullet.BulletSize];
        //afegir effectes de items
        foreach (var singleEffect in addedEffects)
        {
            Type type=Type.GetType(singleEffect.nameEffect.ToString());
            Component script=bulletInstance.Last().AddComponent(type);
            EffectsBullets effectsBullets=script as EffectsBullets;

            // if(script==add)

            if (effectsBullets!=null)
            {
                effectsBullets.Setup(singleEffect.setup);
            }
            // bulletInstance.Last().AddComponent(Type.GetType(item.ToString()));
        }
   
    }
    protected virtual AllInformationBullet ModifingFinalStats( AllInformationBullet finalBullet)
    {
        return finalBullet;
    }
    #endregion

    #region call from other scripts
    public TypePart GetTypePart()
    {
        return typePart;
    }
    public void PassVariables(Transform firepoint, SpriteRenderer spriteRenderer)
    {
        this.firepoint.Add(firepoint);
        // this.bulletPrefab = bulletPrefab;
        this.sprite= spriteRenderer;
    }
    public void AssignIDP(int newIDP)
    {
        IDP=newIDP;
    }
    public int ReturnIDP()
    {
        return IDP;
    }

    public void SubscribeEvent()
    {
        switch (GetTypePart())
        {
            case TypePart.Mele:
                Debug.LogWarning("part mele no acabat");
                break;
            case TypePart.Moveable:
                Debug.LogWarning("part movable no acabat");
                break;
            case TypePart.ShootableLeft:
                // GameManager.Instance.inputManager.OnShotLeft
                // GameManager.Instance.inputManager.OnShotLeft -= pa.DoShot;
                GameManager.Instance.inputManager.OnShotLeft += DoShot;
                Debug.Log("creating OnShotLeft");
                break;
            case TypePart.ShootableRight: 
                // GameManager.Instance.inputManager.OnShotRight -= pa.DoShot;s
                GameManager.Instance.inputManager.OnShotRight += DoShot;
                break;
        }
    }
    public void DesubcribeEvent()
    {
        switch (GetTypePart())
        {
            case TypePart.Mele:
                Debug.LogError("part mele no acabat");
                break;
            case TypePart.Moveable:
                Debug.LogError("part movable no acabat");
                break;
            case TypePart.ShootableLeft:
                GameManager.Instance.inputManager.OnShotLeft -= DoShot;
                break;
            case TypePart.ShootableRight:
                GameManager.Instance.inputManager.OnShotRight -= DoShot;
                break;
        }
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
    // Coroutine coroutineCanShot=null;

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
            // Shot();
            if(coroutine!=null) StopCoroutine(coroutine);
            coroutine=StartCoroutine(ShotIE());
            // if(coroutineCanShot==null)  coroutineCanShot=StartCoroutine(CanShotIE());

        }else if (ctx.canceled)
        {   
            Debug.Log("left cancelled");
            if(coroutine!=null) StopCoroutine(coroutine);

        }
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

    private IEnumerator ShotIE()
    {
        timeShooting=0;
        while (true)
        {
            timeShooting+= Time.deltaTime;
            if(canShot) { 
                Shot();
               
                StartCoroutine(CanShotIE());
            }
            // yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
            yield return null;
        }
    }

    private IEnumerator CanShotIE()
    {
        canShot=false;
        float time2shot=GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots)-timeShooting/4;
        if (time2shot < 0.2)
        {
            time2shot=0.2f;
        }
        yield return new WaitForSeconds(time2shot);
        canShot=true;
    }
    public void Shot()
    {
        foreach (var item in firepoint)
        {
            float accu= GetTotalStat(Stat.StatTypeGeneral.Accuraccy);
            // float side=UnityEngine.Random.Range(-1,2);
            float side = (UnityEngine.Random.value > 0.5f) ? 1f : -1f;
            // float maxDisp= timeShooting*side;

            /* float t = UnityEngine.Random.value; 
            t = Mathf.Pow(t, accu/2); //com més gran sigui l'exponent, més "biaix" cap al mínim
            if(maxDisp>80) maxDisp=80; 
            float finalDisp= Mathf.Lerp(timeShooting/6*side, maxDisp, t); */
            //Debug.Log("left disp: "+ maxDisp);
            float TimeLlindar1=0.5f, TimeLlindar2=1.6f;
            float Dispersio1=20f, Dispersio2=50f, Dispersio3=90f;
            float t= accu/20;
            if(t>=0.75) t=0.75f;
            // float t = UnityEngine.Random.value; 
            // t = Mathf.Pow(t, accu/2);

            float finalDisp=0;
            if(timeShooting<=TimeLlindar1)
            {
                // float howLittle=accu/disp +2;
                // float correction= accu/howLittle;
                // disp-=correction;
                finalDisp= Mathf.Lerp(0, Dispersio1*side, timeShooting/TimeLlindar1)* (1 - t);

                // Debug.Log("left first if disp:"+ disp);
            }
            else if(timeShooting>TimeLlindar1 && timeShooting<=TimeLlindar2)
            {
                // float correction= accu/2;
                // disp-=correction;
                // Debug.Log("left second if disp:"+ disp);
                finalDisp= Mathf.Lerp(0, Dispersio2*side, (timeShooting-TimeLlindar1)/(TimeLlindar2-TimeLlindar1))* (1 - t);


            }
            else if(timeShooting>TimeLlindar2)
            {
                // disp-=accu;
                // Debug.Log("left third if disp:"+ disp);
                float DispersioMin= (timeShooting/TimeLlindar2) *3;
                if(DispersioMin>30) DispersioMin=30;
                float minMaxDisp= UnityEngine.Random.Range(0, DispersioMin*side);
                finalDisp= UnityEngine.Random.Range(minMaxDisp,Dispersio3*side)* (1 - t);
            }

            Quaternion rotationWithOffset = item.GetComponentInParent<Transform>().rotation * Quaternion.Euler(0, finalDisp, 0);

            CreateBullet(item.position,rotationWithOffset);
            
            bulletInstance= new List<GameObject>();

            
        }
       
    }
}
public class Blast: GunBase
{
    Coroutine coroutine=null;

    protected override void OnEnable()
    {
        ObjectNameID="BlastObject";
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
            // Shot();
            if(coroutine!=null) StopCoroutine(coroutine);
            coroutine=StartCoroutine(ShotIE());
            // if(coroutineCanShot==null)  coroutineCanShot=StartCoroutine(CanShotIE());

        }else if (ctx.canceled)
        {   
            Debug.Log("left cancelled");
            if(coroutine!=null) StopCoroutine(coroutine);

        }
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }
    private IEnumerator ShotIE()
    {
        // timeShooting=0;
        while (true)
        {
            // timeShooting+= Mathf.Pow(10,Time.deltaTime);
            if(canShot) { 
                Shot();
               
                StartCoroutine(CanShotIE());
            }
            // yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
        }
    }
     private IEnumerator CanShotIE()
    {
        canShot=false;
        yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
        canShot=true;
    }
    public void Shot()
    {
        
    }
}

//si al aire, fa knockback, dispara moltes bales, munició 2 carges
public class Escopeta : GunBase
{
    protected override void OnEnable()
    {
        ObjectNameID="EscopetaObject";
        NameStatsBullet=NameBulletPreset.Basic;
        // DefineBulletStats();
        base.OnEnable();
    }

    void Awake()
    {
        typePart= TypePart.ShootableRight;
    }

    public override void DoShot(InputAction.CallbackContext ctx)
    {
        Debug.Log("SHOUld shot "+ ctx);
        if (canShot && ctx.performed)
        {
            Debug.Log("SHOUld .position: " + transform.position);
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
        float nbullets = GetTotalStat(Stat.StatTypeGeneral.NumberBullets);
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

public class Flamethrower :GunBase
{
    Coroutine coroutineConstShot=null;
    // Coroutine coroutineCanShot=null;

    Coroutine coroutineHeat=null;
    bool OverHeat=false;


    float MaxHeat=100;
    float CurrentHeat=0;
    float regen;
    float HeatXShot=8f;
    protected override void OnEnable()
    {
        ObjectNameID="FlamethrowerObject";
        NameStatsBullet=NameBulletPreset.Flame;
        base.OnEnable();//== DefineBulletStats();
        

        // HeatCurrent= statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.Magazine, IDP);

    }
    void Awake(){    
        typePart= TypePart.ShootableRight;

        MaxHeat=GetTotalStat(Stat.StatTypeGeneral.Magazine);
        // CurrentHeat=MaxHeat;
        Debug.Log("color maxHeat:"+MaxHeat+" currentHeat:"+CurrentHeat);
        regen= GetTotalStat(Stat.StatTypeGeneral.ShieldRegenRate);
    }
    protected override AllInformationBullet ModifingFinalStats( AllInformationBullet finalStats)
    {
        finalStats.StatsBullet[Stat.StatTypeBullet.DistMax]+= UnityEngine.Random.Range(-1, 3);
        finalStats.StatsBullet[Stat.StatTypeBullet.BulletSpeed]+= UnityEngine.Random.Range(-2, 3);
        // finalStats.StatsBullet[Stat.StatTypeBullet.DistEffec]-= UnityEngine.Random.Range(0, 10);

        return finalStats;
        // finalStats = default;
        // return finalStats;
    }
    public override void DoShot(InputAction.CallbackContext ctx)
    {
        //IDK if maxHeat would change as it should, so here is a savecheck
        MaxHeat=GetTotalStat(Stat.StatTypeGeneral.Magazine);
        regen= GetTotalStat(Stat.StatTypeGeneral.ShieldRegenRate);

        if ( ctx.performed)
        {
            Debug.Log("left performed");
            if(coroutineConstShot!=null) StopCoroutine(coroutineConstShot);
            if (!OverHeat)
            {
                if (coroutineConstShot != null) StopCoroutine(coroutineConstShot);
                coroutineConstShot = StartCoroutine(ShotIE());
            }

            /* if(!OverHeat){ 

                if(coroutineHeat!=null) StopCoroutine(coroutineHeat);
            } */

        }else if (ctx.canceled)
        {   
            Debug.Log("left cancelled");
            if(coroutineConstShot!=null) StopCoroutine(coroutineConstShot);

            if(coroutineHeat!=null) StopCoroutine(coroutineHeat);
            coroutineHeat= StartCoroutine( RechargeHeat());

        }
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

      private IEnumerator ShotIE()
    {
        // if(canShot){ 
           /*  Shot();
            // coroutineCanShot=StartCoroutine(CanShotIE());    
            // canShot=false;
            yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots)); */
            // canShot=true;
        // }
        // yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));//statsManager.instance.GetShipGunBulletStat(Stat.StatTypeGeneral.TimeBetweenShots, IDP, BaseStatsBullet));
        while (!OverHeat)
        {
            // timeShooting+= Mathf.Pow(10,Time.deltaTime);
            // if(canShot) {
                Shot();
                // coroutineCanShot=StartCoroutine(CanShotIE());     
                // canShot=false;
                yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
                // canShot=true;
            // }
            // yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
        }
    }
    /* private IEnumerator CanShotIE()
    {
        canShot=false;
        yield return new WaitForSeconds(GetTotalStat(Stat.StatTypeGeneral.TimeBetweenShots));
        canShot=true;
    } */
     public void Shot()
    {
        foreach (var item in firepoint)
        {
            for (int i = 0; i < 2; i++)
            {

                float accur=GetTotalStat(Stat.StatTypeGeneral.Accuraccy)/5;
                float distorsion= UnityEngine.Random.Range(accur-30 ,31-accur);
                Quaternion rotationWithOffset = item.GetComponentInParent<Transform>().rotation * Quaternion.Euler(0, distorsion, 0);
                CreateBullet(item.position,rotationWithOffset);
                CurrentHeat+=HeatXShot;
              
            }
        }
        CalculOverHeat();
        UpdateColor();
    }

    void UpdateColor()
    {

        Color colorBase = Color.red;
        float h, s, v;
        Color.RGBToHSV(colorBase, out h, out s, out v);
        float novaSaturacio = CurrentHeat / MaxHeat;
        float saturacioSegura = Mathf.Clamp01(novaSaturacio);
        Debug.Log("color h:"+h +" s:"+s +" v:"+v+" newSat:"+novaSaturacio+ " saveSat:"+saturacioSegura);


        Color finalColor=Color.HSVToRGB(h, saturacioSegura, v);
        finalColor.a=1f;
        sprite.color = finalColor;

    }
    void CalculOverHeat()
    {
        if (CurrentHeat >= MaxHeat)
        {
            CurrentHeat=MaxHeat;
            OverHeat=true;
            if(coroutineConstShot!=null) StopCoroutine(coroutineConstShot);
            if (coroutineHeat != null) StopCoroutine(coroutineHeat);
            coroutineHeat = StartCoroutine(RechargeHeat());
        }
        // if(OverHeat) 
    }
    private IEnumerator RechargeHeat()
    {
        yield return new WaitForSeconds(1f);
        //should change this to a reload bullets 
        while(CurrentHeat>0){
            CurrentHeat-= regen;
            if(CurrentHeat<0) CurrentHeat=0;
            UpdateColor();

            // sprite.color = Color.Lerp(Color.red, Color.white, CurrentHeat / MaxHeat);
            yield return new WaitForSeconds(0.1f);
        }
        OverHeat=false;
    }

}