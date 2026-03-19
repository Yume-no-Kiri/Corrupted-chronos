using UnityEngine;

public class ItemBase : MonoBehaviour
{
    //no se quines funcions hauria de tenir un objecte
    protected string nameItem;
    protected ItemData itemData;

    void Awake()
    {
        nameItem=this.GetType().Name;
        itemData= GameManager.Instance.itemDataBase.ReturnItemDataByName(nameItem);
    }

    public ItemData GetScipteableObject()
    {
        return itemData;
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


public class HealthItem: ItemBase
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

public class EnergyItem: ItemBase
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