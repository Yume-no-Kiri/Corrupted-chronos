using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
public class BaseStatsSO : ScriptableObject
{
    [Serializable]
    public struct StatInit
    {
        public Stat.StatTypeGeneral type;
        public float value;

        // public static 
    }

    public List<StatInit> defaultStats;
}
