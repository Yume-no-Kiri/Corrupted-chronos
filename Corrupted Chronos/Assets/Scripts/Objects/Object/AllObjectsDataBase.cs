using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//exists allObjectMB


[CreateAssetMenu(fileName = "AllObjectsDataBase", menuName = "Scriptable Objects/AllObjectsDataBase")]
public class AllObjectsDataBase : ScriptableObject
{
    [ SerializeField]
    public List<AllObjectSO> AllObjects = new List<AllObjectSO>();

    public void StartConfigObject()
    {
        foreach (var item in AllObjects)
        {
            // Debug.Log("awake come here1");
            //AssignTakableData
            // float a = GameManager.Instance.staminaAct;
            // Debug.Log("awake come here2");

            item.AssignTakableData(GameManager.Instance.takableDataBase.ReturnItemDataByName(item.IDItemName));
            // Debug.Log("awake come here3");


            //AssignPlacementData
            if(item.canBePlaced){
                item.AssignPlacementData(GameManager.Instance.placementDataBase.ReturnPartDataById(item.IDPlacementData));
            }
            else
            {
                item.AssignPlacementData(null);
            }
            
            }
    }

    public AllObjectSO ReturnObjectSOByName(string ObjectNameID)
    {
        //probablement canvia a acces directe []
        AllObjectSO dataItem=null;
        foreach (var item in AllObjects)
        {
            if (item.NameID == ObjectNameID)
            {
                dataItem=item;
                break;
            }
        }
        return dataItem;
    }

}