using System;
using System.Collections.Generic;
using System.Text;
using Ink.Parsed;
using UnityEngine;



//Make this better in the future
// [CreateAssetMenu(menuName = "Bullet/BulletDatabase")]
public class ListBulletStats : MonoBehaviour {

    public GameObject GeneralBullet;
    public GameObject hitboxBullet;
    public GameObject hitsphereBullet;

 
    public Dictionary<NameBulletPreset, AllInformationBullet> statEachBullet= new Dictionary<NameBulletPreset, AllInformationBullet>();

    [SerializeField]
    private List<AuxStatsBullets> auxListStatsBullets= new List<AuxStatsBullets>();



    void Awake()
    {

        foreach (var auxStat in auxListStatsBullets)
        {
            AllInformationBullet allInformation = new AllInformationBullet();
            // Dictionary<Stat.StatTypeBullet, Stat> statBullet;
            Dictionary<Stat.StatTypeBullet, float> newStat= new Dictionary<Stat.StatTypeBullet, float>();

            foreach (var item in auxStat.bulletStatsSO.BulletStats)
            {
                /* Stat novaStat = new Stat {
                    name = (Stat.StatTypeGaneral)item.type,
                    baseValue = v
                    currentValue = item.value
                }; */
                newStat[item.type] = item.value;
                // statEachBullet[stat.NameBullet] = newStat;
                // newStat[item.type]=item.value;
            }
            allInformation.StatsBullet=newStat;

            allInformation.sprite=auxStat.bulletStatsSO.sprite;
            allInformation.Effects=auxStat.bulletStatsSO.Effects;
            allInformation.NameBullet=auxStat.NameBullet;
            allInformation.classBullet=auxStat.bulletStatsSO.classBullet;
            
            //  effectsEachBullet[stat.NameBullet]= allInformation;
            statEachBullet[auxStat.NameBullet]=allInformation;
        }

    }

    public AllInformationBullet? ReturnBulletStatsSO(NameBulletPreset name)
    {
        AllInformationBullet? returnStats=null;
        if (statEachBullet.ContainsKey(name))
        { returnStats=  statEachBullet[name].Clone(); }

        return returnStats;
    }

    /* public void CreateList()
    {
        ListBullets novaBala = new ListBullets("WaveBullet", new BulletStatsSO(AllPresetBullets.Wave));   

    }   */

}


//només té les dades base de la bullet, aquí no es sumarà mai res
public struct AllInformationBullet
{
    public NameBulletPreset NameBullet;
    public ClassBullet classBullet;


    public Dictionary<Stat.StatTypeBullet, float> StatsBullet;
    public Sprite sprite;
    public List<EffectsAdd> Effects;

    internal bool IsEmpty()
    {
        return NameBullet==NameBulletPreset.Null;
    }

    public void Debuger()
    {
        if (IsEmpty())
        {
            Debug.LogError("Intentant fer Debug d'una Bullet buida.");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"<b>--- Info Bullet: {NameBullet} ---</b>");

        // 1. Stats del Diccionari
        sb.AppendLine("<b>Stats:</b>");
        if (StatsBullet != null && StatsBullet.Count > 0)
        {
            foreach (var kvp in StatsBullet)
            {
                sb.AppendLine($"- {kvp.Key}: {kvp.Value}");
            }
        }
        else
        {
            sb.AppendLine("- <i>Sense stats assignades</i>");
        }

        // 2. Llista d'Efectes
        sb.AppendLine("<b>Efectes:</b>");
        if (Effects != null && Effects.Count > 0)
        {
            foreach (var effect in Effects)
            {
                // Suposant que EffectsAdd té nameEffect i setup (com hem vist abans)
                sb.AppendLine($"- Nom: {effect.nameEffect} | Setup: {effect.setup}");
            }
        }
        else
        {
            sb.AppendLine("- <i>Sense efectes</i>");
        }

        // 3. Sprite
        sb.AppendLine($"<b>Sprite:</b> {(sprite != null ? sprite.name : "Null")}");

        // Imprimim tot el bloc a la consola d'Unity
        Debug.Log("stat"+sb.ToString());
    }
    public AllInformationBullet Clone()
    {
        AllInformationBullet clone = new AllInformationBullet();
        
        clone.NameBullet = this.NameBullet;
        clone.classBullet = this.classBullet;
        clone.sprite = this.sprite;

        if (this.StatsBullet != null)
        {
            clone.StatsBullet = new Dictionary<Stat.StatTypeBullet, float>(this.StatsBullet);
        }
        else
        {
            clone.StatsBullet = new Dictionary<Stat.StatTypeBullet, float>();
        }

        // 3. Dupliquem la Llista (si existeix)
        if (this.Effects != null)
        {
            clone.Effects = new List<EffectsAdd>(this.Effects);
            
            // NOTA: Si 'EffectsAdd' és una CLASSE i vols modificar les seves variables internes 
            // en la còpia sense canviar l'original, hauries de clonar cada efecte individualment:
            
            clone.Effects = new List<EffectsAdd>();
            foreach (var effect in this.Effects)
            {
                clone.Effects.Add(effect.Clone());
            }
           
        }
        else
        {
            clone.Effects = new List<EffectsAdd>();
        }

        return clone;
    }

}


[Serializable]
public struct AuxStatsBullets
{
    public NameBulletPreset NameBullet;
    public BulletStatsSO bulletStatsSO;
}