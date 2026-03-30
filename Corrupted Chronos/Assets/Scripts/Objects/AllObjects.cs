using System;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "AllObjects", menuName = "Scriptable Objects/AllObjects")]
[Serializable]
public class AllObjects /*  : ScriptableObject */
{

    [field: SerializeField]
    public string nameShow;

    [field: SerializeField]
    public string NameID { get; private set; }
    
    //Deberiamos estructurar como hacemos el id
    /* [field: SerializeField]
    public int ID { get; private set; } */


    //I should make the ID the same, two strings or two ints, 
    public bool canBePlaced;
    public int IDplacementData;
    public PlacementDataItem placementDataItem{get; private set;} 


    // public bool isItem;
    public string IDitemName;
    public ItemData ItemData{get; private set;}

    public int IDP { get; private set; }

}

[CreateAssetMenu(fileName = "AllObjectsDataBase", menuName = "Scriptable Objects/AllObjectsDataBase")]
public class AllObjectsDataBase : ScriptableObject
{
    [field: SerializeField]
    public List<AllObjects> AllObjects = new List<AllObjects>();


}