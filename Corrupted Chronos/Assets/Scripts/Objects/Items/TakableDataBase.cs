using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
// [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class TakableData
{
    // [field: SerializeField]

    public string nameShow;
    public string nameID;
    public Sprite sprite;

    public GameObject toInstanciate;

/* [field: SerializeField] public string nameID { get; private set; }
    [field: SerializeField] public GameObject toInstanciate { get; private set; }
    [field: SerializeField] public float baseDamage { get; private set; } */
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
    public List<TakableData> listItemsAux;

    //he de fer una conversió a diccionari
    public Dictionary<string, TakableData> listItems= new Dictionary<string, TakableData>();

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

    public TakableData ReturnItemDataByName(string name)
    {
        // Debug.Log("awake come here inside1");

        if (listItems.ContainsKey(name))
        {
            return listItems[name];
        }else{ return null;}
    }

    public GameObject GetInstantiateFromNumber(int index)
    {
        if (listItemsAux[index] != null)
        {
            return listItemsAux[index].toInstanciate;
        }
        else
        {
            return null;
        }
    }

   
}
