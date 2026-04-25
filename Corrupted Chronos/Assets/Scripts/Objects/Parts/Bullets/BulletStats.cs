using System;
using System.Collections.Generic;
using UnityEngine;


public struct EffectsAdd
{
    public string nameEffect;
    public NameHitboxInBullet setup;

    public EffectsAdd(string v, NameHitboxInBullet body) : this()
    {
        this.nameEffect = v;
        this.setup = body;
    }
    public EffectsAdd(string v) : this()
    {
        this.nameEffect = v;
        this.setup = NameHitboxInBullet.Null;
    }
}

//Defined in the ScripteableObject in project
[CreateAssetMenu(fileName="WaveStats", menuName = "Bullet/WaveBulletStats")]
public class WaveStatsSO: BulletStatsSO
{
    private WaveStatsSO(){
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

    }

}

[CreateAssetMenu(fileName="RedWaveStats", menuName = "Bullet/RedWaveBulletStats")]
public class RedWaveStatsSO: BulletStatsSO
{
    private RedWaveStatsSO(){
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

    }

}

[CreateAssetMenu(fileName="BasicStats", menuName = "Bullet/BasicBulletStats")]
public class BasicStatsSO: BulletStatsSO
{
    private BasicStatsSO(){
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
    }

}
