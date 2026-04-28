using System;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;


[Serializable]
public class ListPartStats:MonoBehaviour
{

    private Dictionary<string, Dictionary<Stat.StatTypeGun, Stat>> statEachGun= new Dictionary<string, Dictionary<Stat.StatTypeGun, Stat>>();

    [SerializeField]
    private List<StatsGun> listStatsGuns;

    // public string NamePart;
    // public PartsStatsSO bulletStatsSO;




    void Awake()
    {

        foreach (var stat in listStatsGuns)
        {

            Dictionary<Stat.StatTypeGun, Stat> newStat= new Dictionary<Stat.StatTypeGun, Stat>();
            foreach (var item in stat.gunStatsSO.GunStats)
            {
                Stat novaStat = new Stat {
                    name = (Stat.StatTypeGeneral)item.type,
                    baseValue = item.value,
                    currentValue = item.value
                };
                newStat[item.type] = novaStat;

                // newStat[item.type]=item.value;
            }
            statEachGun[stat.NameGun] = newStat;
            
        }

    }

    public Dictionary<Stat.StatTypeGun, Stat> ReturnStatsByName(string name)
    {
        Dictionary<Stat.StatTypeGun, Stat> returnState= new Dictionary<Stat.StatTypeGun, Stat>();
        if (statEachGun.ContainsKey(name))
        { returnState= statEachGun[name]; }
        return returnState;
    }

   
}
// public class 

// [CreateAssetMenu(menuName = "Stats/GunBaseStatsSO")]


[Serializable]
public struct StatsGun
{
    public string NameGun;
    public PartBaseStatsSO gunStatsSO;
}


