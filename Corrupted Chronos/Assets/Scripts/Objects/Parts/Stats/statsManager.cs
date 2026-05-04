using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    public static statsManager instance;

    //TODO: Recibir un SO con los stats base
    //TODO: los modificadores deber�an tener source
    //TODO: Stats que dependan de otras stats (ej: da�o que dependa de speed)


    //El diccionario existe para tener acceso O(1) a cualquier stat
    Dictionary<Stat.StatTypeGeneral, Stat> statLookup;
    Dictionary<int, Dictionary<Stat.StatTypeGun, Stat>> statEachGun;

    //Esto SE MANTIENE privado
    //Es una lista para poder editar los stats desde el inspector, pero no se expone a otras clases
    // [SerializeField] List<Stat> stats = new();
    [SerializeField] private BaseStatsSO dadesBase;

    [Header("Stats")]
    [field: SerializeField] public ListPartStats listPartStats { get; private set; }
    [field: SerializeField] public ListBulletStats listBulletStats { get; private set; }

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

        statLookup = new Dictionary<Stat.StatTypeGeneral, Stat>();
        statEachGun = new Dictionary<int, Dictionary<Stat.StatTypeGun, Stat>>();

        if (dadesBase != null)
        {
            foreach (var item in dadesBase.defaultStats)
            {
                Stat novaStat = new Stat
                {
                    name = item.type,
                    baseValue = item.value,
                    currentValue = item.value
                };
                statLookup[item.type] = novaStat;

            }
        }


    }

    /* private void Start() {
        statEachGun= gunStats.ReturnStatsEachGun();
    } */


    public void CreateStatsGun(string name, int idp)
    {
        Dictionary<Stat.StatTypeGun, Stat> baseStats = listPartStats.ReturnStatsByName(name);
        if (baseStats != null && baseStats.Count > 0)
        {
            Dictionary<Stat.StatTypeGun, Stat> independentStats = new Dictionary<Stat.StatTypeGun, Stat>();

            foreach (var kvp in baseStats)
            {
                //creem una copia de les stats base, si no, modiifcariem les stats/*  */
                independentStats[kvp.Key] = new Stat
                {
                    name = kvp.Value.name,
                    baseValue = kvp.Value.baseValue,
                    currentValue = kvp.Value.baseValue
                };
            }
            statEachGun[idp] = independentStats;
        }
        else { Debug.LogError("don't found gun stats"); }
    }

    public float AddModifier(Stat.StatTypeGeneral type, StatModifier mod)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            return stat.AddModifier(mod);
        }

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }

    public void RemoveModifier(Stat.StatTypeGeneral type, StatModifier mod)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            stat.removeModifier(mod);
        }
    }

    public float AddModifier(int modifyIDP, Stat.StatTypeGun type, StatModifier mod)
    {
        if (statEachGun.TryGetValue(modifyIDP, out var gun))
        {
            if (gun.TryGetValue(type, out var stat))
            {
                return stat.AddModifier(mod);
            }
        }
        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }
    public void RemoveModifier(int modifyIDP, Stat.StatTypeGun type, StatModifier mod)
    {
        if (statEachGun.TryGetValue(modifyIDP, out var gunStats))
        {
            if (gunStats.TryGetValue(type, out var stat))
            {
                stat.removeModifier(mod);
            }
        }
        else
        {
            Debug.LogWarning($"No es pot treure el modificador: ID d'arma {modifyIDP} no trobada.");
        }
    }
    /* ia diu:
    Si aquest mètode es crida moltes vegades (per exemple, cada cop que dispares), fer ToString() i TryParse és una mica costós.
     Una solució més professional seria crear un Diccionari de traducció estàtic un sol cop:
     private static Dictionary<Stat.StatTypeBullet, Stat.StatTypeGaneral> _bulletToGeneralMap;

    private void InitializeMap() {
        _bulletToGeneralMap = new();
        foreach (Stat.StatTypeBullet b in Enum.GetValues(typeof(Stat.StatTypeBullet))) {
            if (Enum.TryParse(b.ToString(), out Stat.StatTypeGaneral g)) {
                _bulletToGeneralMap[b] = g;
            }
        }
    }

    // I al teu bucle:
    if (_bulletToGeneralMap.TryGetValue(tipus, out var generalType)) {
        finalBullet.StatsBullet[tipus] = GetShipGunBulletStat(generalType, idp, bulletPreset);
    }

      */
    public AllInformationBullet ReturnFinalStatsBullet(int idp, AllInformationBullet bulletPreset)
    {
        AllInformationBullet finalBullet = new AllInformationBullet();

        finalBullet.NameBullet = bulletPreset.NameBullet;
        finalBullet.sprite = bulletPreset.sprite;
        finalBullet.Effects = bulletPreset.Effects;

        finalBullet.StatsBullet = new Dictionary<Stat.StatTypeBullet, float>();
        foreach (Stat.StatTypeBullet tipus in Enum.GetValues(typeof(Stat.StatTypeBullet)))
        {
            string statName = tipus.ToString();

            if (Enum.TryParse(statName, out Stat.StatTypeGeneral generalEquivalent))
            {
                // Si el nom existeix en tots dos, fem la crida amb el tipus correcte
                finalBullet.StatsBullet[tipus] = GetShipGunBulletStat(generalEquivalent, idp, bulletPreset);
            }
            else
            {
                Debug.LogWarning($"La stat {statName} no té equivalent a StatTypeGaneral");
            }
        }

        return finalBullet;
    }
    public bool StatExistsGun(Stat.StatTypeGeneral GeneralStat)
    {
        string nom = GeneralStat.ToString();
        // Comprova si el string "Damage" existeix dins de StatTypeGeneral
        return Enum.IsDefined(typeof(Stat.StatTypeGun), nom);
    }
    public bool StatExistsBulLet(Stat.StatTypeGeneral GeneralStat)
    {
        string nom = GeneralStat.ToString();
        // Comprova si el string "Damage" existeix dins de StatTypeGeneral
        return Enum.IsDefined(typeof(Stat.StatTypeBullet), nom);
    }
    /* public bool StatExistsBulet(Stat.StatTypeBullet bulletStat)
    {
        string nom = bulletStat.ToString();
        // Comprova si el string "Damage" existeix dins de StatTypeGeneral
        return Enum.IsDefined(typeof(Stat.StatTypeGaneral), nom);
    } */

    #region get single stat
    public float GetShipStat(Stat.StatTypeGeneral type)
    {
        if (statLookup.TryGetValue(type, out var stat))
            // Debug.Log("stats Ship:"+stat.currentValue);
            return stat.currentValue;

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }
    public float GetGunStat(Stat.StatTypeGeneral type, int idp)
    {
        if (statEachGun.ContainsKey(idp))
        {
            if (statEachGun[idp].TryGetValue((Stat.StatTypeGun)type, out var stat3))
            {
                Debug.Log("stats Gun:" + stat3.currentValue);
                return stat3.currentValue;
            }
        }


        Debug.LogWarning($"Stat {type} not found");
        return 0f;
    }

    public float GetShipGunStat(Stat.StatTypeGeneral type, int idp)
    {
        float finalValue = 0;

        if (statLookup.TryGetValue(type, out var stat1))
        { finalValue += stat1.currentValue; }
        if (statEachGun.ContainsKey(idp))
        {
            if (statEachGun[idp].TryGetValue((Stat.StatTypeGun)type, out var stat3))
            {
                Debug.Log("stats ShipGun:" + stat3.currentValue);
                finalValue += stat3.currentValue;
            }
        }
        // Debug.LogWarning($"Stat {type} not found");
        return finalValue;
    }
    public float GetShipGunBulletStat(Stat.StatTypeGeneral type, int idp, AllInformationBullet bulletPreset)
    {
        float finalValue = 0;
        if (statLookup.TryGetValue(type, out var stat1))
        {
            finalValue += stat1.currentValue;
        }
        Debug.Log("stats ShipGunBullet " + type + " idp:" + idp + " base:" + stat1.currentValue + " finalValue:" + finalValue);

        if (statEachGun.ContainsKey(idp))
        {
            // if(StatExistsGun(type)) {
            if (Enum.TryParse(type.ToString(), out Stat.StatTypeGun guntype))
            {

                if (statEachGun[idp].TryGetValue(guntype, out var stat2))
                {

                    finalValue += stat2.currentValue;
                    Debug.Log("stats ShipGunBullet " + type + " idp:" + idp + " gun:" + stat2.currentValue + " finalValue:" + finalValue);

                }
            }

        }
        if (Enum.TryParse(type.ToString(), out Stat.StatTypeBullet bullettype))
        {

            // if(StatExistsBulLet(type)) {
            if (bulletPreset.StatsBullet != null && bulletPreset.StatsBullet.TryGetValue(bullettype, out var stat3))
            {

                finalValue += stat3; //no se modifica les dades de les bullets
                Debug.Log("stats ShipGunBullet " + type + " idp:" + idp + " bullet:" + stat3 + " finalValue:" + finalValue);

            }
        }
        // Debug.LogWarning($"Stat {type} not found");
        Debug.Log("stats ShipGunBullet " + type + " idp:" + idp + " final:" + finalValue);

        return finalValue;
    }
    #endregion
}

[Serializable]
public class Stat
{

    /*  public enum StatsEnemy
     {
         Health=0,
         Stamina=1,
         Shields=2,
         ShieldRegen=3,
         Speed=4,
         Agility=5,
     } */

    public enum StatTypeGeneral
    {
        //ship:
        Health = 0, //rang (0, inf)
        Stamina = 1, //rang (0, inf)
        Shields = 2, //rang (0, inf)
        ShieldRegen = 3, //rang (0, inf)
        Speed = 4, //rang (0, inf)
        Agility = 5, //rang (0, inf)

        // guns
        TimeBetweenShots = 50, //rang (0, inf)
        Magazine = 51, //rang (0, inf)
        Accuraccy = 52, //rang (0, inf)

        // bullet
        Damage = 100,
        BulletSpeed = 101,
        Penetration = 102,
        DistEffec = 103,
        DistMax = 104,
        Knockback = 105,

    }
    public enum StatTypeGun
    {
        // guns
        TimeBetweenShots = 50,
        Magazine = 51,
        Accuraccy = 52,

        // bullet
        Damage = 100,
        BulletSpeed = 101,
        Penetration = 102,
        DistEffec = 103,
        DistMax = 104,
        Knockback = 105,
    }

    public enum StatTypeBullet
    {

        // bullet
        Damage = 100,
        BulletSpeed = 101,
        Penetration = 102,
        DistEffec = 103,
        DistMax = 104,
        Knockback = 105,
    }



    public StatTypeGeneral name;

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
    public StatModifier(float value, ModifierType type, object source = null)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }


}
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
