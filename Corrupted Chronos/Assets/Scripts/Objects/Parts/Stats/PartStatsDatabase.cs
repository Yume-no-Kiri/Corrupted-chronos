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

}

[CreateAssetMenu(fileName="EscopetaStats",menuName = "Stats/EscopetaPartStats")]
public class EscopetaStatsSO: PartBaseStatsSO
{}
[CreateAssetMenu(fileName="MetralletaStats",menuName = "Stats/MetralletaPartStats")]
public class MetralletaStatsSO: PartBaseStatsSO
{}
[CreateAssetMenu(fileName="FlamethrowerStats",menuName = "Stats/FlamethrowerPartStats")]
public class FlamethrowerStatsSO: PartBaseStatsSO
{}
