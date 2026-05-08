using System;
using System.Collections.Generic;
using UnityEngine;


/* struct SetUpHitbox
{
    public NameHitboxInBullet Type;
    
    Vector3 size;
    public bool IsTrigger;
    public bool IsCollider;


} */

public enum NameBulletPreset
{
    Null=0,
    Basic=1,
    Wave=2, 
    RedWave=3,
    FollowMissile=4,
    Flame=5
}

// Ara mateix la relació de colliders i effectes es: molts efectes a 1 collider
// Es bastant estricte i s'hauria de revisar en un futur, efectes amb un collider especific,
//2 effectes seguits augmenten els collider i stats d'aquell efecte i tal

//probablement separar effectsAdd, en: EffectsHitboxAdd i EffectsBulletAdd;

[Serializable]
public struct EffectsAdd
{
    public NameEffectBullets nameEffect;
    public NameHitboxInBullet setup;

    public EffectsAdd(NameEffectBullets v, NameHitboxInBullet body) : this()
    {
        this.nameEffect = v;
        this.setup = body;
    }
    public EffectsAdd(NameEffectBullets v) : this()
    {
        this.nameEffect = v;
        this.setup = NameHitboxInBullet.Null;
    }
}
// [CreateAssetMenu(menuName = "Bullet/BulletStats")]
public class BulletStatsSO : ScriptableObject
{
    //maybe keep the sprite? idk
    public Sprite sprite;
     public List<EffectsAdd> Effects;

    [Serializable]
    public struct StatInit
    {
        public Stat.StatTypeBullet type;
        public float value;
    }
    public List<StatInit> BulletStats;//= new List<StatInit>(Enum.GetNames(typeof(Stat.StatTypeBullet)).Length);

    private void Reset()
    {
        var names = System.Enum.GetValues(typeof(Stat.StatTypeBullet));
        BulletStats = new List<StatInit>();

        foreach (Stat.StatTypeBullet t in names)
        {
            BulletStats.Add(new StatInit { type = t, value = 0 });
        }
    }

   /*  [Header("Bullet Properties")]
    [Tooltip("The name of the bullet")]
    public Sprite sprite;
    public int Damage;
    public float AttackSpeed;
    public float Penetration;
    public float DistEffec;
    public float DistMax;
    public float Knockback; */

    

    
}


//Defined in the ScripteableObject in project
[CreateAssetMenu(fileName="WaveStats", menuName = "Bullet/WaveBulletStats")]
public class WaveStatsSO: BulletStatsSO
{
    // private WaveStatsSO()
    /* private void Reset()
    {
        Effects=new List<EffectsAdd>();

        Damage=0;
        Penetration=5;//dudo de como usar-lo
        DistEffec=10;
        DistMax=15;
        AttackSpeed=7;
        Knockback=10;
        // Effects= new System.Collections.Generic.List<string>();
        // Effects.Add( new EffectsAdd("BaseBullets"));
        Effects.Add(new EffectsAdd("AddHitboxEB", NameHitboxInBullet.Body));
        Effects.Add(new EffectsAdd("KnockbackEB"));//will pick the last hitbox created, in this case body

    } */

}

[CreateAssetMenu(fileName="RedWaveStats", menuName = "Bullet/RedWaveBulletStats")]
public class RedWaveStatsSO: BulletStatsSO
{
    // private RedWaveStatsSO()
    /* private void Reset()
    {
        Effects=new List<EffectsAdd>();

        Damage=10;
        Penetration=5;//dudo de como usar-lo
        DistEffec=10;
        DistMax=15;
        AttackSpeed=7;
        Knockback=10;
        // Effects.Add( new EffectsAdd("BaseBullets"));
        Effects.Add(new EffectsAdd("AddHitboxEB", NameHitboxInBullet.Body));
        Effects.Add(new EffectsAdd("KnockbackEB"));
        Effects.Add(new EffectsAdd("DamageEB"));

    } */

}

[CreateAssetMenu(fileName="BasicStats", menuName = "Bullet/BasicBulletStats")]
public class BasicStatsSO: BulletStatsSO
{
        // private BasicStatsSO(){

   /*  private void Reset()
    {
        Effects=new List<EffectsAdd>();

        Damage=10;
        Penetration=5;//dudo de como usar-lo
        DistEffec=10;
        DistMax=15;
        AttackSpeed=7;
        Knockback=4;
        // Effects.Add( new EffectsAdd("BaseBullets"));
        Effects.Add(new EffectsAdd("AddHitboxEB", NameHitboxInBullet.Body));
        Effects.Add(new EffectsAdd("DamageEB"));
    } */

}

[CreateAssetMenu(fileName="FollowinMissile", menuName = "Bullet/FollowinMissile")]
public class FollowinMissileSO: BulletStatsSO
{
    
}

[CreateAssetMenu(fileName="FlameStats", menuName = "Bullet/FlameStats")]
public class FlameStatsSO: BulletStatsSO
{
    
}

