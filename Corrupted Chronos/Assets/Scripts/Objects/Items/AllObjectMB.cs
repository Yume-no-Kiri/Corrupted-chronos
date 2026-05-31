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
    public AllObject allObjectSO{get; private set;}

    public string nameShow{get; private set;}
    // protected string nameItem;
    public TakableData takableData{get; private set;}

    //actualment aquest té IDP, s'hauria de moure enrere fins a tindre'l més accesible
    public PlacementDataSO placementData{get; private set;}

    //this is the nameID of the AllObjectSO database
    public string ObjectNameID= "";

    protected ModifierItem modifierItem= new ModifierItem();

    //it's in gunbase 
    // public int IDP=-1;
    #region definers
    public void DefineObject(AllObject allObjectSO)
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
    public AllObject ReturnAllObjectSO() 
    {
        return allObjectSO;
    }
    public TakableData ReturnTakableDataSO()
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

