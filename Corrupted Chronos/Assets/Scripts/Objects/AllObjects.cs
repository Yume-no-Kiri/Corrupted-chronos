using System;
using System.Collections.Generic;
using UnityEngine;


//exists allObjectMB
[Serializable]
public class AllObjectSO /*  : ScriptableObject */
{

    [field: SerializeField]
    public string nameShow;

    [field: SerializeField]
    public string NameID { get; private set; }
    
    //Deberiamos estructurar como hacemos el id
    /* [field: SerializeField]
    public int ID { get; private set; } */


    // public bool isItem;
    public string IDItemName;
    public TakableDataSO takableData{get; private set;}

    public int IDP { get; private set; }

    
    //I should make the ID the same, two strings or two ints, 
    public bool canBePlaced;
    public int IDPlacementData=-1;
    public PlacementDataSO placementDataItem{get; private set;} 
    
    // we should create consructors for each defined part?



}

[CreateAssetMenu(fileName = "AllObjectsDataBase", menuName = "Scriptable Objects/AllObjectsDataBase")]
public class AllObjectsDataBase : ScriptableObject
{
    [field: SerializeField]
    public List<AllObjectSO> AllObjects = new List<AllObjectSO>();

    public void FinishCreateList()
    {
        foreach (var item in AllObjects)
        {
            //AssignTakableData
            // takableData= GameManager.Instance.itemDataBase.ReturnItemDataByName(nameItem);


            //AssignPlacementData
        }
    }

}