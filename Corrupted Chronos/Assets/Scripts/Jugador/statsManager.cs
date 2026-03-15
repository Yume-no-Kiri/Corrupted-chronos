using System;
using System.Collections.Generic;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    //TODO: Recibir un SO con los stats base
    //TODO: los modificadores deberían tener source


    //El diccionario existe para tener acceso O(1) a cualquier stat
    Dictionary<Stat.StatType, Stat> statLookup;

    //Esto SE MANTIENE privado
    //Es una lista para poder editar los stats desde el inspector, pero no se expone a otras clases
    [SerializeField] List<Stat> stats = new();

    void Awake()
    {
        statLookup = new Dictionary<Stat.StatType, Stat>();

        foreach (var stat in stats)
        {
            statLookup[stat.name] = stat;
        }
    }

    public float AddModifier(Stat.StatType type, StatModifier mod)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            return stat.AddModifier(mod);
        }

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }

    public float GetStat(Stat.StatType type)
    {
        if (statLookup.TryGetValue(type, out var stat))
            return stat.currentValue;

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }
}

[Serializable]
public class Stat
{
    public enum StatType
    {
        Health,
        Shields,
        ShieldRegen,
        Damage,
        Speed,
        Agility,
        AttackSpeed
    }
    public StatType name;

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

