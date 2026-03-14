using System;
using System.Collections.Generic;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    //TODO: Recibir un SO con los stats base
    //TODO: los modificadores deberían tener source

    [SerializeField] List<Stat> stats = new();

    public void AddModifier(string statName, StatModifier mod)
    {
        var stat = stats.Find(s => s.name == statName);
        if (stat != null)
            stat.AddModifier(mod);
    }
}

[Serializable]
public class Stat
{
    public string name;

    public float baseValue;
    public float currentValue;

    [SerializeField] List<StatModifier> additive = new();
    [SerializeField] List<StatModifier> multiplicative = new();
    [SerializeField] List<StatModifier> exponent = new();

    public void AddModifier(StatModifier mod)
    {
        /*
        switch (mod.type)
        {
            case ModifierType.Add:
                additive.Add(mod);
                break;

            case ModifierType.Multiply:
                multiplicative.Add(mod);
                break;

            case ModifierType.Exponent:
                exponent.Add(mod);
                break;
        }*/

        Recalculate();
    }

    void Recalculate()
    {
        float value = baseValue;

        foreach (var m in additive)
            value += m.value;

        foreach (var m in multiplicative)
            value *= m.value;

        foreach (var m in exponent)
            value = Mathf.Pow(value, m.value);

        currentValue = value;
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

