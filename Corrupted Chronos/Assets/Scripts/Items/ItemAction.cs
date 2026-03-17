using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct ItemParameters
{
    // [field: SerializeField]
    public GameObject gameObject;
    // [field: SerializeField]
    public int idAux;    
}

[CreateAssetMenu(fileName = "ItemAction", menuName = "Scriptable Objects/ItemAction")]
public class ItemAction : ScriptableObject
{

    [field: SerializeField]
    public List<ItemParameters> listItems;

  

    //MOLT TEMPORAL, revisar i fusionar amb EachPartScript
   
}
