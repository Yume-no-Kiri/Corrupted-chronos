using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum NameBulletPreset
{
    Null=0,
    Basic=1,
    Wave=2, 
    RedWave=3,
    FollowMissile=4,
    Flame=5,
    Explosion=6
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

    public EffectsAdd Clone(){
        EffectsAdd clone;
        clone.nameEffect=nameEffect;
        clone.setup=setup;
        return clone;
    }
}
// [CreateAssetMenu(menuName = "Bullet/BulletStats")]
public class BulletStatsSO : ScriptableObject
{
    //maybe keep the sprite? idk
    public Sprite sprite;
    public ClassBullet classBullet;

    public List<EffectsAdd> Effects;


    [Serializable]
    public struct StatInit
    {
        public Stat.StatTypeBullet type;
        public float value;
    }

    public List<StatInit> BulletStats= new List<StatInit>();//= new List<StatInit>(Enum.GetNames(typeof(Stat.StatTypeBullet)).Length);

    private void OnEnable()
    {
        if (BulletStats == null || BulletStats.Count == 0){

        var names = System.Enum.GetValues(typeof(Stat.StatTypeBullet));
        BulletStats = new List<StatInit>();

        foreach (Stat.StatTypeBullet t in names)
        {
            BulletStats.Add(new StatInit { type = t, value = 0 });
        }
        }
    }

    
}


//Defined in the ScripteableObject in project
[CreateAssetMenu(fileName="WaveStats", menuName = "Bullet/WaveBulletStats")]
public class WaveStatsSO: BulletStatsSO
{}

[CreateAssetMenu(fileName="RedWaveStats", menuName = "Bullet/RedWaveBulletStats")]
public class RedWaveStatsSO: BulletStatsSO
{}

[CreateAssetMenu(fileName="BasicStats", menuName = "Bullet/BasicBulletStats")]
public class BasicStatsSO: BulletStatsSO
{}

[CreateAssetMenu(fileName="FollowinMissile", menuName = "Bullet/FollowinMissile")]
public class FollowinMissileSO: BulletStatsSO
{}

[CreateAssetMenu(fileName="FlameStats", menuName = "Bullet/FlameStats")]
public class FlameStatsSO: BulletStatsSO
{}

[CreateAssetMenu(fileName="ExplosionStats", menuName = "Bullet/ExplosionStats")]
public class ExplosionStatsSO: BulletStatsSO
{
}

