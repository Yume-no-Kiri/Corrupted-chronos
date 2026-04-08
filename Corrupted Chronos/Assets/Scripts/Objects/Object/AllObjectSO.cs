using System;
using UnityEngine;

[Serializable]
public class AllObjectSO /*  : ScriptableObject */
{

    [ SerializeField]
    public string nameShow;

    [ SerializeField]
    public string NameID;// { get; private set; }
    
    //Deberiamos estructurar como hacemos el id
    /* [field: SerializeField]
    public int ID { get; private set; } */


    // public bool isItem;
    public string IDItemName;
    [HideInInspector]public TakableDataSO takableDataSO;

    // public int IDP;// { get; private set; }

    
    //I should make the ID the same, two strings or two ints, 
    public bool canBePlaced;
    public int IDPlacementData=-1;
    [HideInInspector]public PlacementDataSO placementDataItemSO;
    
    // we should create consructors for each defined part?

    public void AssignTakableData(TakableDataSO takableData)
    {
        this.takableDataSO=takableData;
    }
    public void AssignPlacementData(PlacementDataSO placementData)
    {
        this.placementDataItemSO=placementData;
    }
}