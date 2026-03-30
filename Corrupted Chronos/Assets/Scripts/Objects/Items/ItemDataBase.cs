using System;
using System.Collections.Generic;
using UnityEngine;



//change nom, això també ha d'anar a parts
[Serializable]
// [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData
{
    // [field: SerializeField]

    public string nameShow;
    public string nameID;
    public Sprite sprite;

    public GameObject toInstanciate;

    /* public bool canBePlaced;
    public PlacementDataItem placementDataItem{get; private set;} 

 */    public bool isItem;
    // some class for active


    //hauria de tenir el monobehvious a instnaciar 
    
    // public int idAux;    
    // public ItemBase myItemBase;

    public void DefinePlacementData()
    {
        /* if (canBePlaced)
        {
            //algo per acabar de definir
        } */
    }
}

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Scriptable Objects/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{

    [field: SerializeField]
    public List<ItemData> listItemsAux;

    //he de fer una conversió a diccionari
    public Dictionary<string, ItemData> listItems= new Dictionary<string, ItemData>();

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

    public ItemData ReturnItemDataByName(string name)
    {
        if (listItems.ContainsKey(name))
        {
            return listItems[name];
        }else{ return null;}
    }

    //MOLT TEMPORAL, revisar i fusionar amb EachPartScript(potser)
   
}
