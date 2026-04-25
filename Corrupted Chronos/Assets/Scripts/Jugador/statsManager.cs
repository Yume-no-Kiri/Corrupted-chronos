using System;
using System.Collections.Generic;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    public static statsManager instance;

    //TODO: Recibir un SO con los stats base
    //TODO: los modificadores deber�an tener source
    //TODO: Stats que dependan de otras stats (ej: da�o que dependa de speed)


    //El diccionario existe para tener acceso O(1) a cualquier stat
    Dictionary<Stat.StatTypeGaneral, Stat> statLookup;
    Dictionary<int, Dictionary<Stat.StatTypeGun, Stat>> statEachGun;

    //Esto SE MANTIENE privado
    //Es una lista para poder editar los stats desde el inspector, pero no se expone a otras clases
    // [SerializeField] List<Stat> stats = new();
    [SerializeField] private BaseStatsSO dadesBase;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        statLookup = new Dictionary<Stat.StatTypeGaneral, Stat>();

        if(dadesBase != null)
        {
            foreach (var item in dadesBase.defaultStats)
            {
                Stat novaStat = new Stat {
                    name = item.type,
                    baseValue = item.value,
                    currentValue = item.value
                };
                statLookup[item.type] = novaStat;

            }
        }
   
    
    }

    public float AddModifier(Stat.StatTypeGaneral type, StatModifier mod)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            return stat.AddModifier(mod);
        }

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }

    public void RemoveModifier() { 
    }

    public float GetStat(Stat.StatTypeGaneral type)
    {
        if (statLookup.TryGetValue(type, out var stat))
            return stat.currentValue;

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }
    public float GetStat(Stat.StatTypeGaneral type, int idp)
    {
        float finalValue=0;

        if (statLookup.TryGetValue(type, out var stat1))
            {finalValue+= stat1.currentValue;}
        if(statEachGun.ContainsKey(idp)){
            if (statEachGun[idp].TryGetValue((Stat.StatTypeGun)type, out var stat3))
                {finalValue+= stat3.currentValue;}
        }
        // Debug.LogWarning($"Stat {type} not found");
        return finalValue;
    }

}

[Serializable]
public class Stat
{
    public enum StatTypeGaneral
    {
        //ship:
        Health,
        Stamina,
        Shields,
        ShieldRegen,
        Speed,
        Agility,

        // guns
        TimeBetweenShots,
        Magazine,
        Accuraccy,

        // bullet
        Damage,
        AttackSpeed,
        Penetration,
        DistEffec,
        DistMax,
        Knockback,

    }
    public enum StatTypeGun
    {
         // guns
        TimeBetweenShots,
        Magazine,
        Accuraccy,

        // bullet
        Damage,
        BulletSpeed,
        Penetration,
        DistEffec,
        DistMax,
        Knockback,
    }

    /* public enum StatTypeBullet
    {

        // bullet
        Damage,
        AttackSpeed,
        Penetration,
        DistEffec,
        DistMax,
        Knockback,
    } */



    public StatTypeGaneral name;

    public float baseValue;
    public float currentValue;

    [SerializeField] List<StatModifier> additive = new();
    [SerializeField] List<StatModifier> multiplicative = new();
    [SerializeField] List<StatModifier> exponent = new();

    public float AddModifier(StatModifier mod)
    {
        switch (mod.type)
        {
            case StatModifier.ModifierType.Add:
                additive.Add(mod);
                break;

            case StatModifier.ModifierType.Multiply:
                multiplicative.Add(mod);
                break;

            case StatModifier.ModifierType.Exponent:
                exponent.Add(mod);
                break;
        }

        return Recalculate();
    }

    public void removeModifier(StatModifier mod)
    {
        switch (mod.type)
        {
            case StatModifier.ModifierType.Add:
                additive.Remove(mod);
                break;
            case StatModifier.ModifierType.Multiply:
                multiplicative.Remove(mod);
                break;
            case StatModifier.ModifierType.Exponent:
                exponent.Remove(mod);
                break;
        }
        Recalculate();
    }

    float Recalculate()
    {
        float value = baseValue;

        foreach (var m in additive)
            value += m.value;

        foreach (var m in multiplicative)
            value *= m.value;

        foreach (var m in exponent)
            value = Mathf.Pow(value, m.value);

        currentValue = value;
        return value;
    }
}

[Serializable]
public class StatModifier
{
    public enum ModifierType
    {
        Add,
        Multiply,
        Exponent
    }

    public ModifierType type;
    public float value;
    public object source;
}
[CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
public class BaseStatsSO : ScriptableObject
{
    [Serializable]
    public struct StatInit
    {
        public Stat.StatTypeGaneral type;
        public float value;
    }

    public List<StatInit> defaultStats;
}
