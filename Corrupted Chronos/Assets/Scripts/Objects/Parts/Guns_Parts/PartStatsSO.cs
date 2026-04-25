using System;
using System.Collections.Generic;
using UnityEngine;




/* Explication: CAME AND RETHINK OF THIS LATER
    This script was made to save the stats of the parts, similar to the system of the bullets, but the stats of the parts, can be definied in each
    monobehaviour, I think this is just innecesary complex 
 */

/* [Serializable]
public struct ListParts
{
    public string NamePart;
    public PartsStatsSO bulletStatsSO;
} */
// public class 

// [CreateAssetMenu(menuName = "Stats/GunBaseStatsSO")]
public class GunBaseStatsSO : ScriptableObject
{
    Dictionary<Stat.StatTypeGun, Stat> statEachGun;

    // public List<ListParts> listBaseStatsParts = new List<ListParts>();

    /* public PartsStatsSO ReturnPartStatsSO(string name)
    {
        PartsStatsSO partsStatsSO=null;
        foreach (var item in listBaseStatsParts)
        {
            if (item.NamePart == name)
            {
                partsStatsSO= item.bulletStatsSO;
                break;
            }
        }
        return partsStatsSO;
    } */
}
/* 
[CreateAssetMenu(menuName = "Parts/PartStats")]
public class PartsStatsSO: ScriptableObject
{
    // public InformationBullet informationBullet;

    public float t2s;// = 1f;
    public float t2s2;// = 1f;
    public int cargador;// = 5;
    public int contCargador;
} */

[CreateAssetMenu(fileName="EscopetaStats",menuName = "Stats/EscopetaPartStats")]
public class EscopetaStatsSO: GunBaseStatsSO
{}
[CreateAssetMenu(fileName="MetralletaStats",menuName = "Stats/MetralletaPartStats")]
public class MetralletaStatsSO: GunBaseStatsSO
{}
