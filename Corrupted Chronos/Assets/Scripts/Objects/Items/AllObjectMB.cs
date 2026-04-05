using UnityEngine;

/// <summary>
/// This is the basic script for the object, IT belongs to allObjectSO
/// </summary>
//GunBase is child of ItemBase
public class AllObjectMB : MonoBehaviour
{
    public AllObjectSO allObjectSO{get; private set;}

    public string nameShow{get; private set;}
    // protected string nameItem;
    public TakableDataSO takableData{get; private set;}

    public PlacementDataSO placementData{get; private set;}

    public void DefineObject(AllObjectSO allObjectSO)
    {
        this.allObjectSO= allObjectSO;

        nameShow=allObjectSO.nameShow;
    
        takableData=allObjectSO.takableData;
        if (allObjectSO.canBePlaced)
        {
            placementData=allObjectSO.placementDataItem;
        }
        else
        {
            placementData=null;
        }

    }
    void Awake()
    {
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

    protected virtual void Activate()
    {
        //afegeix totes les bonificacions
    }

    protected virtual void Deactivate()
    {
        //treu totes les bonificacions        
    }    
}


public class LifeItem: AllObjectMB
{
    protected override void Activate()
    {
        Debug.Log("health item activate");
    }
    protected override void Deactivate()
    {
        Debug.Log("health item deactivate");
    }

}

public class EnergyItem: AllObjectMB
{
    void Start()
    {
        Debug.Log("energy item start");
    }
    protected override void Activate()
    {
        Debug.Log("energy item activate");
    }
    protected override void Deactivate()
    {
        Debug.Log("energy item deactivate");
    }

}