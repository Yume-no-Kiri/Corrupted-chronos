using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData
{
    // [field: SerializeField]

    string name;
    public Sprite sprite;
    //hauria de tenir el monobehvious a instnaciar 
    
    public int idAux;    
    public ItemBase myItemBase;

}

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Scriptable Objects/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{

    [field: SerializeField]
    public List<ItemData> listItemsAux;

    //he de fer una conversió a diccionari
    public Dictionary<string, ItemData> listItems;

    public ItemData ReturnItemDataByName(string name)
    {
        if (listItems.ContainsKey(name))
        {
            return listItems[name];
        }else{ return null;}
    }

    //MOLT TEMPORAL, revisar i fusionar amb EachPartScript
   
}
