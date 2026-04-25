using System;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

[Serializable]
public struct ListBullets
{
    public string NameBullet;
    public BulletStatsSO bulletStatsSO;

    public ListBullets(string name, BulletStatsSO stats)
    {
        this.NameBullet = name;
        this.bulletStatsSO = stats;
    }
}

//Make this better in the future
[CreateAssetMenu(menuName = "Bullet/BulletDatabase")]
public class BulletDatabase : ScriptableObject {

    public GameObject GeneralBullet;
    public GameObject hitboxBullet;

    public List<ListBullets> listStatsPresetsBullets= new List<ListBullets>();
    /* public bulletStatsSO basicBullet;
    public bulletStatsSO waveBullet;
    public bulletStatsSO fireBullet; */
    public BulletStatsSO ReturnBulletStatsSO(string name)
    {
        BulletStatsSO bulletStatsSO=null;
        foreach (var item in listStatsPresetsBullets)
        {
            if (item.NameBullet == name)
            {
                bulletStatsSO= item.bulletStatsSO;
                break;
            }
        }
        return bulletStatsSO;
    }

    /* public void CreateList()
    {
        ListBullets novaBala = new ListBullets("WaveBullet", new BulletStatsSO(AllPresetBullets.Wave));   

    }   */

}

[CreateAssetMenu(menuName = "Bullet/BulletStats")]
public class BulletStatsSO : ScriptableObject
{
    [Header("Bullet Properties")]
    [Tooltip("The name of the bullet")]
    public Sprite sprite;
    public int Damage;
    public float AttackSpeed;
    public float Penetration;
    public float DistEffec;
    public float DistMax;
    public float Knockback;

    [Header("Gameobjects")]
    public List<EffectsAdd> Effects;

    
}

