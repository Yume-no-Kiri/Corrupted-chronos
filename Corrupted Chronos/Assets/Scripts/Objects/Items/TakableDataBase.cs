using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
// [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class TakableDataSO
{
    // [field: SerializeField]

    public string nameShow;
    public string nameID;
    public Sprite sprite;

    public GameObject toInstanciate;

    /* public bool canBePlaced;
    public PlacementDataItem placementDataItem{get; private set;} 

 */ 
    // public bool isItem;
    // some class for active

    
}

[CreateAssetMenu(fileName = "TakableDataBase", menuName = "Scriptable Objects/TakableDataBase")]
public class TakableDataBase : ScriptableObject
{

    [field: SerializeField]
    public List<TakableDataSO> listItemsAux;

    //he de fer una conversió a diccionari
    public Dictionary<string, TakableDataSO> listItems= new Dictionary<string, TakableDataSO>();

    #region Change to usable values
    public void StartConfigItem()
    {
        foreach(var item in listItemsAux)
        {
            string name= item.nameID;
            listItems.Add(name, item);
        }
    }
    #endregion

    public TakableDataSO ReturnItemDataByName(string name)
    {
        if (listItems.ContainsKey(name))
        {
            return listItems[name];
        }else{ return null;}
    }

    //MOLT TEMPORAL, revisar i fusionar amb EachPartScript(potser)
   
}
