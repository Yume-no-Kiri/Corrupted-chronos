using System;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

[Serializable]
public struct ListBullets
{
    public string NameBullet;
    public BulletStatsSO bulletStatsSO;
}

//Make this better in the future
[CreateAssetMenu(menuName = "Bullet/BulletDatabase")]
public class BulletDatabase : ScriptableObject {

    public GameObject emptyBullet;
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

}

[CreateAssetMenu(menuName = "Bullet/BulletStats")]
public class BulletStatsSO : ScriptableObject
{
    [Header("Bullet Properties")]
    [Tooltip("The name of the bullet")]
    public Sprite sprite;
    public int damage;
    public float penetration;
    public float distEffective;
    public float distMax;
    public float speed;
    public float knockback;

    [Header("Gameobjects")]
    public GameObject[] hitEffects;
    public GameObject[] trailEffects;
}

[CreateAssetMenu(menuName = "Bullet/WaveBulletData")]
public class WaveStatsSO: BulletStatsSO
{
    public WaveStatsSO(){
        damage=0;
        penetration=5;//dudo de como usar-lo
        distEffective=10;
        distMax=15;
        speed=1;
        knockback=10;
    }

}

[CreateAssetMenu(menuName = "Bullet/BasicBulletData")]
public class BasicStatsSO: BulletStatsSO
{
    public BasicStatsSO(){
        damage=10;
        penetration=5;//dudo de como usar-lo
        distEffective=10;
        distMax=15;
        speed=2;
        knockback=4;
    }

}
