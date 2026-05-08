using System.Collections.Generic;
using UnityEngine;

public enum ListNameObjects
{
    Null=100,
    LifeItem=101,
    EnergyItem=102,
    RadarItem=103
}

public struct ModifierItem
{
    public Dictionary<Stat.StatTypeGeneral, StatModifier> ItemGeneralStats;
    public Dictionary<Stat.StatTypeGun, StatModifier> ItemGunStats;

    public List<EffectsAdd> ItemBulletEffects;
    public int modifyIdp;

    public static ModifierItem Create()
    {
       return new ModifierItem
        {
            ItemGeneralStats = new Dictionary<Stat.StatTypeGeneral, StatModifier>(),
            ItemGunStats = new Dictionary<Stat.StatTypeGun, StatModifier>(),
            ItemBulletEffects = new List<EffectsAdd>(),
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

    public virtual void ActivateSlotEffect()
    {
        //afegeix totes les bonificacions
    }  

    public virtual void DeactivateSlotEffect()
    {
        //treu totes les bonificacions        
    }    


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
    float PlusSpeedBullet=7;


    protected override void OnEnable()
    {
        ObjectNameID="LifeObject";
    }
    protected override void DefineModifierItem()
    {
        

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.Health, new StatModifier(PlusHealth, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.Shields, new StatModifier(PlusShield, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.BulletSpeed, new StatModifier(PlusSpeedBullet,StatModifier.ModifierType.Add));
    }

    
    //return List<EffectsAdd>
    public override void ActivateSlotEffect()//int idpGun)
    {
        //amb el idpGun podriem afegir els buffos a l'arma 
        Debug.Log("health item activate");
    }
    public override void DeactivateSlotEffect()
    {
        Debug.Log("health item deactivate");
    }

   
}

public class EnergyItem: AllObjectMB
{
    protected override void OnEnable()
    {
        ObjectNameID="EnergyObject";
    }
    protected override void DefineModifierItem()
    {
        
    }

    public override void ActivateSlotEffect()
    {
        Debug.LogError("energy item activate");
        
    }
    public override void DeactivateSlotEffect()
    {
        Debug.Log("energy item deactivate");
    }

}

public class RadarItem: AllObjectMB
{
    protected override void OnEnable()
    {
        ObjectNameID="RadarObject";
    }
    protected override void DefineModifierItem()
    {
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.FollowOpposedEB, NameHitboxInBullet.Detectors));

    }
    public override void ActivateSlotEffect()
    {
        Debug.LogError("energy item activate");
        
    }
    public override void DeactivateSlotEffect()
    {
        Debug.Log("energy item deactivate");
    }

}



/* public struct SingleStatModifierItem
{
    StatModifier statModifier;
    float value;
} */