using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    public static statsManager instance;







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


    public Coroutine CoroutineRegenShields=null;
    public Coroutine CoroutineRegenStamina=null;
    public Coroutine CoroutineConsumeStamina=null;
    public bool StaminaRegen=false;
    public bool IsStaminaEmpty {get; private set;}
//  =false;

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

            Stat healthstat = new Stat
            {
                name = Stat.StatTypeGeneral.CurrentHealth,
                baseValue = statLookup[Stat.StatTypeGeneral.MaxHealth].baseValue,
                currentValue = statLookup[Stat.StatTypeGeneral.MaxHealth].baseValue
            };

            statLookup[Stat.StatTypeGeneral.CurrentHealth] = healthstat;

            Stat staminastat = new Stat
            {
                name = Stat.StatTypeGeneral.CurrentStamina,
                baseValue = statLookup[Stat.StatTypeGeneral.MaxStamina].baseValue,
                currentValue = statLookup[Stat.StatTypeGeneral.MaxStamina].baseValue
            };
            statLookup[Stat.StatTypeGeneral.CurrentStamina] = staminastat;

            Stat shieldstat = new Stat
            {
                name = Stat.StatTypeGeneral.CurrentShields,
                baseValue = statLookup[Stat.StatTypeGeneral.MaxShields].baseValue,
                currentValue = statLookup[Stat.StatTypeGeneral.MaxShields].baseValue
            };
            statLookup[Stat.StatTypeGeneral.CurrentShields] = shieldstat;

        }

        IsStaminaEmpty=false;
    }

    private void Update()
    {
       /*  if (GetShipStat(Stat.StatTypeGeneral.MaxShields) > GetShipStat(Stat.StatTypeGeneral.CurrentShields))
        {
            increaseStatValue(Stat.StatTypeGeneral.CurrentShields, GetShipStat(Stat.StatTypeGeneral.ShieldRegenRate) * Time.deltaTime);
        } */
        if(GetShipStat(Stat.StatTypeGeneral.CurrentStamina)<100 && CoroutineRegenStamina==null &&CoroutineConsumeStamina==null){
            CoroutineRegenStamina=StartCoroutine(TimerRegenStamina());
        }
    }

    public void increaseStatValue(Stat.StatTypeGeneral type, float amount)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            stat.currentValue += amount;
        }
    }

    public void decreaseStatValue(Stat.StatTypeGeneral type, float amount)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            stat.currentValue -= amount;
        }
    }
    #region shield
    public void JustHit()
    {
        if(CoroutineRegenShields != null){
            StopCoroutine(CoroutineRegenShields);
            CoroutineRegenShields=null;
        }
        CoroutineRegenShields=StartCoroutine(TimerRegenShields());
    }

    private IEnumerator TimerRegenShields()
    {
         yield return new WaitForSecondsRealtime(statLookup[Stat.StatTypeGeneral.ShieldRegenDelay].currentValue);
        // StaminaRegen=true;
        float shieldsAct= statLookup[Stat.StatTypeGeneral.CurrentShields].currentValue;
        float shieldsMax= statLookup[Stat.StatTypeGeneral.MaxShields].currentValue;

        while(shieldsAct<shieldsMax){
            yield return new WaitForSecondsRealtime(0.2f);
            //staminaAct+=staminaRegenQuantity; 
            increaseStatValue(Stat.StatTypeGeneral.CurrentShields, statLookup[Stat.StatTypeGeneral.ShieldRegenRate].currentValue);    
            // IsStaminaEmpty=false;

            shieldsAct= statLookup[Stat.StatTypeGeneral.CurrentShields].currentValue;
            shieldsMax= statLookup[Stat.StatTypeGeneral.MaxShields].currentValue;
        }
        CoroutineRegenShields=null;
    }

    #endregion


    #region stamina 
    public void InformIfGround(bool IsGround)
    {
        if (IsGround)
        {
            if(CoroutineConsumeStamina!=null){ 
                StopCoroutine(CoroutineConsumeStamina);
                CoroutineConsumeStamina=null;
            }
        }
    }
    public bool CanFly()
    {
        return IsStaminaEmpty? false:true;
      
    }

    public void MoveUpStamina(bool ground)
    {
        // if(!ground){
        if(CoroutineConsumeStamina==null) {
            if (CoroutineRegenStamina != null)
            {
                StopCoroutine(CoroutineRegenStamina);
                CoroutineRegenStamina=null;
            }
            // StopCoroutine(CoroutineConsumeStamina);
            CoroutineConsumeStamina= StartCoroutine(UseStamina());
        }
        // }// staminsaAct-=staminaMoveUPUseQuantity; 
    }



    private IEnumerator TimerRegenStamina()
    {
        yield return new WaitForSecondsRealtime(statLookup[Stat.StatTypeGeneral.StaminaRegenDelay].currentValue);
        StaminaRegen=true;
        float staminaAct= statLookup[Stat.StatTypeGeneral.CurrentStamina].currentValue;
        float staminaMax= statLookup[Stat.StatTypeGeneral.MaxStamina].currentValue;

        while(staminaAct<staminaMax){
            yield return new WaitForSecondsRealtime(0.2f);
            //staminaAct+=staminaRegenQuantity; 
            increaseStatValue(Stat.StatTypeGeneral.CurrentStamina, statLookup[Stat.StatTypeGeneral.StaminaRegenRate].currentValue);    
            IsStaminaEmpty=false;

            staminaAct= statLookup[Stat.StatTypeGeneral.CurrentStamina].currentValue;
            staminaMax= statLookup[Stat.StatTypeGeneral.MaxStamina].currentValue;
        }
        CoroutineRegenStamina=null;
    }
    
    private IEnumerator UseStamina()
    {
        // float staminaAct= statLookup[Stat.StatTypeGeneral.CurrentStamina].currentValue;

        if(CoroutineRegenStamina!=null){ 
            StopCoroutine(CoroutineRegenStamina);
            CoroutineRegenStamina=null;    
        }
        while(statLookup[Stat.StatTypeGeneral.CurrentStamina].currentValue>0){
            yield return new WaitForSecondsRealtime(0.2f);
            //staminaAct-=staminaMoveUPUseQuantity;    
            decreaseStatValue(Stat.StatTypeGeneral.CurrentStamina, statLookup[Stat.StatTypeGeneral.StaminaConsumeRate].currentValue);    

        }
        statLookup[Stat.StatTypeGeneral.CurrentStamina].currentValue=0;
        IsStaminaEmpty=true;
        CoroutineConsumeStamina=null;
    }
    
    #endregion

 
    #region AddModifier and RemoveMoidifier
    public float AddModifier(Stat.StatTypeGeneral type, StatModifier mod)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            switch (stat.name)
            {
                case Stat.StatTypeGeneral.MaxHealth:
                    //Se recalcula el currentHealth para que no sigui superior al maxHealth
                    break;
                case Stat.StatTypeGeneral.MaxStamina:
                    //Se recalcula el currentStamina para que no sigui superior al maxStamina
                    break;
                case Stat.StatTypeGeneral.MaxShields:
                    //Se recalcula el currentStamina para que no sigui superior al maxStamina
                    break;
            }
            return stat.AddModifier(mod);
        }

        Debug.LogWarning($"Stat {type} not found");
        return 0f;
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
    public void RemoveModifier(Stat.StatTypeGeneral type, StatModifier mod)
    {
        if (statLookup.TryGetValue(type, out var stat))
        {
            stat.removeModifier(mod);
        }
    }
    #endregion

    #region get stats
    public void CreateStatsGun(ListNameParts name, int idp)
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
    public AllInformationBullet ReturnFinalStatsBullet(int idp, AllInformationBullet bulletPreset)
    {
        AllInformationBullet finalBullet = new AllInformationBullet();

        finalBullet.NameBullet = bulletPreset.NameBullet;
        finalBullet.sprite = bulletPreset.sprite;
        finalBullet.Effects = bulletPreset.Effects;
        finalBullet.classBullet= bulletPreset.classBullet;

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
// <<<<<<< HEAD

        Debug.Log("stats ShipGunBullet "+ type);
        Debug.Log("stats idp:"+idp);
        Debug.Log("stats base:"+stat1.currentValue);
        Debug.Log("stats finalValue:"+finalValue);
// >>>>>>> Arnau3

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
    public enum StatTypeGeneral
    {
        //ship: 6
        MaxHealth=0, //rang (0, inf)
        CurrentHealth=1,
        Speed=2, //rang (0, inf)
        Agility=3, //rang (0, inf)
        KnockbackResistance=4,

        MaxShields=10, //rang (0, inf)
        CurrentShields=11,
        ShieldRegenRate =12, //rang (0, inf)
        ShieldRegenDelay=13, //rang (0, inf)
        
        MaxStamina=20, //rang (0, inf)
        CurrentStamina=21,
        StaminaRegenRate=22,
        StaminaRegenDelay=23,
        StaminaConsumeRate=24,



        // guns: 4
        TimeBetweenShots =50, //rang (0, inf)
        Magazine=51, //rang (0, inf)
        Accuraccy=52, //rang (0, inf)
        NumberBullets=53,


        // bullet: 7
        Damage=100,
        BulletSpeed=101,
        Piercing=102,
        DistEffec=103,
        DistMax=104,
        Knockback=105,
        BulletSize=106,
        ElementalDamage=107

    }
    public enum StatTypeGun
    {

         // guns
        TimeBetweenShots=50,
        Magazine=51,
        Accuraccy=52,
        NumberBullets=53,

        // bullet
        Damage=100,
        BulletSpeed=101,
        Piercing=102,
        DistEffec=103,
        DistMax=104,
        Knockback=105,
        BulletSize=106,
        ElementalDamage=107


    }
    public enum StatTypeBullet
    {

        // bullet

        Damage=100,
        BulletSpeed=101,
        Piercing=102,
        DistEffec=103,
        DistMax=104,
        Knockback=105,
        BulletSize=106,
        ElementalDamage=107

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
