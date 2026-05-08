using System;
using System.Collections.Generic;
using UnityEngine;


public class PartBaseStatsSO : ScriptableObject
{
    [Serializable]
    public struct StatInit
    {
        public Stat.StatTypeGun type;
        public float value;
    }

    public List<StatInit> GunStats;


    private void Reset()
    {
        var names = System.Enum.GetValues(typeof(Stat.StatTypeGun));
        GunStats = new List<StatInit>();

        foreach (Stat.StatTypeGun t in names)
        {
            GunStats.Add(new StatInit { type = t, value = 0 });
        }
    }

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
public class EscopetaStatsSO: PartBaseStatsSO
{}
[CreateAssetMenu(fileName="MetralletaStats",menuName = "Stats/MetralletaPartStats")]
public class MetralletaStatsSO: PartBaseStatsSO
{}
[CreateAssetMenu(fileName="FlamethrowerStats",menuName = "Stats/FlamethrowerPartStats")]
public class FlamethrowerStatsSO: PartBaseStatsSO
{}
