using System;
using UnityEngine;

[Serializable]
public class AllObject /*  : ScriptableObject */
{

    [ SerializeField]
    public string nameShow;

    [ SerializeField]
    public string NameID;// { get; private set; }
    
    // this is not so, could save idp


    // public bool isItem;
    public string IDItemName;
    [HideInInspector]public TakableData takableDataSO;

    // public int IDP;// { get; private set; }

    
    //I should make the ID the same, two strings or two ints, 
    public bool canBePlaced;
    public int IDPlacementData=-1;
    [HideInInspector]public PlacementDataSO placementDataItemSO;
    
    // we should create consructors for each defined part?

    public void AssignTakableData(TakableData takableData)
    {
        this.takableDataSO=takableData;
    }
    public void AssignPlacementData(PlacementDataSO placementData)
    {
        this.placementDataItemSO=placementData;
    }
}