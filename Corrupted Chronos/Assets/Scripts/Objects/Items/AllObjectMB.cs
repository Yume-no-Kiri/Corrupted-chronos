using UnityEngine;

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

    //it's in gunbase 
    // public int IDP=-1;
    public void DefineObject(AllObjectSO allObjectSO)
    {
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
    protected abstract void OnEnable();

    protected virtual void Start()
    {
        DefineObject(GameManager.Instance.allObjectsDataBase.ReturnObjectSOByName(ObjectNameID));
       /*  nameItem=this.GetType().Name;
         */// ObjectData=GameManager.Instance.
    }

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

    public virtual void ActivateSlotEffect()
    {
        //afegeix totes les bonificacions
    }

    public virtual void DeactivateSlotEffect()
    {
        //treu totes les bonificacions        
    }    
}


public class LifeItem: AllObjectMB
{
    protected override void OnEnable()
    {
        ObjectNameID="LifeObject";
    }
    public override void ActivateSlotEffect()
    {
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

    protected override void Start()
    {
        base.Start();
        // Debug.Log("energy item start");
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

