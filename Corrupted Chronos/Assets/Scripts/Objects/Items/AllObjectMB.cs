using System.Collections.Generic;
using UnityEngine;

public enum ListNameObjects
{
    Null=100,
    LifeItem=101,
    EnergyItem=102,
    RadarItem=103,
    HeavyBulletsItem=104,
    LighterBulletsItem=105,

    KaboomBulletsItem=106,
    PiercingBulletsItem=107,
    BouncingBulletsItem=108
    
}

public struct ModifierItem
{
    public Dictionary<Stat.StatTypeGeneral, StatModifier> ItemGeneralStats;
    public Dictionary<Stat.StatTypeGun, StatModifier> ItemGunStats;

    public HashSet<EffectsAdd> ItemBulletEffects;
    public int modifyIdp;

    public static ModifierItem Create()
    {
       return new ModifierItem
        {
            ItemGeneralStats = new Dictionary<Stat.StatTypeGeneral, StatModifier>(),
            ItemGunStats = new Dictionary<Stat.StatTypeGun, StatModifier>(),
            ItemBulletEffects = new HashSet<EffectsAdd>(),
            modifyIdp=-1

        };
    }

}


/// <summary>
/// This is the basic script for the object, IT belongs to allObjectSO
/// </summary>
//GunBase is child of ItemBase
public abstract class AllObjectMB : MonoBehaviour
{
    public AllObjectSO allObjectSO{get; private set;}

    public string nameShow{get; private set;}
    // protected string nameItem;
    public TakableDataSO takableData{get; private set;}

    //actualment aquest té IDP, s'hauria de moure enrere fins a tindre'l més accesible
    public PlacementDataSO placementData{get; private set;}

    //this is the nameID of the AllObjectSO database
    public string ObjectNameID= "";

    protected ModifierItem modifierItem= new ModifierItem();

    //it's in gunbase 
    // public int IDP=-1;
    #region definers
    public void DefineObject(AllObjectSO allObjectSO)
    {
        if(allObjectSO==null) Debug.LogError("allobjectSO is null, didn't found object in allObjectsSO");
        this.allObjectSO= allObjectSO;

        nameShow=allObjectSO.nameShow;
    
        takableData=allObjectSO.takableDataSO;
        if (allObjectSO.canBePlaced)
        {
            placementData=allObjectSO.placementDataItemSO;
        }
        else
        {
            placementData=null;
        }

    }
    protected abstract void DefineModifierItem();
    protected abstract void OnEnable();
    #endregion
    protected virtual void Awake()
    {
        modifierItem=ModifierItem.Create();
        DefineModifierItem();

    }
    protected virtual void Start()
    {
        /* modifierItem.ItemBulletEffects=new List<EffectsAdd>();
        modifierItem.ItemGeneralStats= new Dictionary<Stat.StatTypeGeneral, StatModifier>();
        modifierItem.ItemGunStats=new Dictionary<Stat.StatTypeGun, StatModifier>(); */
        DefineObject(GameManager.Instance.allObjectsDataBase.ReturnObjectSOByName(ObjectNameID));
       /*  nameItem=this.GetType().Name;
         */// ObjectData=GameManager.Instance.
    }

    #region returners
    public AllObjectSO ReturnAllObjectSO() 
    {
        return allObjectSO;
    }
    public TakableDataSO ReturnTakableDataSO()
    {
        return takableData;
    }

    public PlacementDataSO ReturnPlacementDataSO()
    {
        return placementData;
    }

    public ModifierItem ReturnModifierItem()
    {
        return modifierItem;
    }
    #endregion

    /* public virtual void ActivateSlotEffect()
    {
        //afegeix totes les bonificacions
    }  

    public virtual void DeactivateSlotEffect()
    {
        //treu totes les bonificacions        
    }   */  


    //no necesito remove aquí, perquè es a partAdder on les estats modificades s'assigna i s'eliminen
    protected void StatGeneralTypeAdd(Stat.StatTypeGeneral statTypeGeneral, float number)
    {
        modifierItem.ItemGeneralStats.Add(statTypeGeneral, new StatModifier(number, StatModifier.ModifierType.Add));
        // modifierItem.ItemGeneralStats
    }
    
    /* protected void StatGeneralTypeRemove(Stat.StatTypeGeneral statTypeGeneral, float number)
    {
        modifierItem.ItemGeneralStats.Remove(statTypeGeneral, new StatModifier(number, StatModifier.ModifierType.Add));
    } */
}


public class LifeItem: AllObjectMB
{
    float PlusHealth=50;
    float PlusShield=25;
    // float PlusSpeedBullet=7;


    protected override void OnEnable()
    {
        ObjectNameID="LifeObject";
    }
    protected override void DefineModifierItem()
    {
        

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.MaxHealth, new StatModifier(PlusHealth, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.MaxShields, new StatModifier(PlusShield, StatModifier.ModifierType.Add));
        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.BulletSpeed, new StatModifier(PlusSpeedBullet,StatModifier.ModifierType.Add));
    }

    
    //return List<EffectsAdd>
    /* public override void ActivateSlotEffect()//int idpGun)
    {
        //amb el idpGun podriem afegir els buffos a l'arma 
        Debug.Log("health item activate");
    }
    public override void DeactivateSlotEffect()
    {
        Debug.Log("health item deactivate");
    }
 */
   
}

public class EnergyItem: AllObjectMB
{
    float PlusStamina=25;

    protected override void OnEnable()
    {
        ObjectNameID="EnergyObject";
    }
    protected override void DefineModifierItem()
    {
        //  modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.Health, new StatModifier(PlusHealth, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.MaxStamina, new StatModifier(PlusStamina, StatModifier.ModifierType.Add));
    
    }

   /*  public override void ActivateSlotEffect()
    {
        Debug.LogError("energy item activate");
        
    }
    public override void DeactivateSlotEffect()
    {
        Debug.Log("energy item deactivate");
    } */

}

public class RadarItem: AllObjectMB
{
    protected override void OnEnable()
    {
        ObjectNameID="RadarObject";
    }
    protected override void DefineModifierItem()
    {
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.AddHitsphereEB, NameHitboxInBullet.Detectors));
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.FollowOpposedEB, NameHitboxInBullet.Detectors));

        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Accuraccy, new StatModifier(10, StatModifier.ModifierType.Add));

    }
   /*  public override void ActivateSlotEffect()
    {
        Debug.LogError("energy item activate");
        
    }
    public override void DeactivateSlotEffect()
    {
        Debug.Log("energy item deactivate");
    } */

}
public class HeavyBulletsItem: AllObjectMB
{
    float extraSize=0.5f;
    float slowness=-5;


    protected override void OnEnable()
    {
        ObjectNameID="HeavyBulletsObject";
    }
    protected override void DefineModifierItem()
    {

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.BulletSize, new StatModifier(extraSize, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.BulletSpeed, new StatModifier(slowness, StatModifier.ModifierType.Add));
    }
}

public class LighterBulletsItem: AllObjectMB
{
    float extraSize=-0.5f;
    float extraTime2Shot=-0.3f;


    protected override void OnEnable()
    {
        ObjectNameID="LighterBulletsObject";
    }
    protected override void DefineModifierItem()
    {

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.BulletSize, new StatModifier(extraSize, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.TimeBetweenShots, new StatModifier(extraTime2Shot, StatModifier.ModifierType.Add));
    }
}

public class KaboomBulletsItem: AllObjectMB
{
    // float extraSize=0.7f;
    float extraDamage=3f;

    // float velo=-2f;


    protected override void OnEnable()
    {
        ObjectNameID="KaboomBulletsObject";
    }
    protected override void DefineModifierItem()
    {
        // modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.AddHitsphereEB, NameHitboxInBullet.AfterImpact));
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.CreateExplosionOnHitEB, NameHitboxInBullet.Body));

        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.TimeBetweenShots, new StatModifier(extraSize, StatModifier.ModifierType.Add));
        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.BulletSpeed, new StatModifier(velo, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Damage, new StatModifier(extraDamage, StatModifier.ModifierType.Add));


    }
}


public class PiercingBulletsItem: AllObjectMB
{
    float extraPiercing=5f;
    float velo=3;

    protected override void OnEnable()
    {
        ObjectNameID="PiercingBulletsObject";
    }
    protected override void DefineModifierItem()
    {
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Piercing, new StatModifier(extraPiercing, StatModifier.ModifierType.Add));
        
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Knockback, new StatModifier(5, StatModifier.ModifierType.Add));
        
    }
}

public class BouncingBulletsItem: AllObjectMB
{

    float extraPiercing=2f;
    float extraDist=4f;

    protected override void OnEnable()
    {
        ObjectNameID="BouncingBulletsObject";
    }   

    protected override void DefineModifierItem()
    {
        // modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.AddHitsphereEB, NameHitboxInBullet.AfterImpact));
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.BounceOnHitEB, NameHitboxInBullet.Body));


        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Piercing, new StatModifier(extraPiercing, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.DistMax, new StatModifier(extraDist, StatModifier.ModifierType.Add));

        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Accuraccy, new StatModifier(10, StatModifier.ModifierType.Add));
        
    }
}