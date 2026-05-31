using UnityEngine;


public class LifeItem: AllObjectMB
{
    float PlusHealth=50;
    float PlusShield=25;
    // float PlusSpeedBullet=7;


    protected override void OnEnable()
    {
        ObjectNameID="LifeObject";
    }
    protected override void DefineModifierItem()
    {
        

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.MaxHealth, new StatModifier(PlusHealth, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.MaxShields, new StatModifier(PlusShield, StatModifier.ModifierType.Add));
        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.BulletSpeed, new StatModifier(PlusSpeedBullet,StatModifier.ModifierType.Add));
    }
}

public class EnergyItem: AllObjectMB
{
    float PlusStamina=25;

    protected override void OnEnable()
    {
        ObjectNameID="EnergyObject";
    }
    protected override void DefineModifierItem()
    {
        //  modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.Health, new StatModifier(PlusHealth, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.MaxStamina, new StatModifier(PlusStamina, StatModifier.ModifierType.Add));
    
    }
}

public class RadarItem: AllObjectMB
{
    protected override void OnEnable()
    {
        ObjectNameID="RadarObject";
    }
    protected override void DefineModifierItem()
    {
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.AddHitsphereEB, NameHitboxInBullet.Detectors));
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.FollowOpposedEB, NameHitboxInBullet.Detectors));

        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Accuraccy, new StatModifier(10, StatModifier.ModifierType.Add));

    }
}
public class HeavyBulletsItem: AllObjectMB
{
    float extraSize=0.5f;
    float slowness=-5;


    protected override void OnEnable()
    {
        ObjectNameID="HeavyBulletsObject";
    }
    protected override void DefineModifierItem()
    {

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.BulletSize, new StatModifier(extraSize, StatModifier.ModifierType.Add));
        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.BulletSpeed, new StatModifier(slowness, StatModifier.ModifierType.Add));
    }
}

public class LighterBulletsItem: AllObjectMB
{
    float extraSize=-0.5f;
    float extraTime2Shot=-0.3f;


    protected override void OnEnable()
    {
        ObjectNameID="LighterBulletsObject";
    }
    protected override void DefineModifierItem()
    {

        modifierItem.ItemGeneralStats.Add(Stat.StatTypeGeneral.BulletSize, new StatModifier(extraSize, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.TimeBetweenShots, new StatModifier(extraTime2Shot, StatModifier.ModifierType.Add));
    }
}

public class KaboomBulletsItem: AllObjectMB
{
    // float extraSize=0.7f;
    float extraDamage=3f;

    // float velo=-2f;


    protected override void OnEnable()
    {
        ObjectNameID="KaboomBulletsObject";
    }
    protected override void DefineModifierItem()
    {
        // modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.AddHitsphereEB, NameHitboxInBullet.AfterImpact));
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.CreateExplosionOnHitEB, NameHitboxInBullet.Body));

        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.TimeBetweenShots, new StatModifier(extraSize, StatModifier.ModifierType.Add));
        // modifierItem.ItemGunStats.Add(Stat.StatTypeGun.BulletSpeed, new StatModifier(velo, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Damage, new StatModifier(extraDamage, StatModifier.ModifierType.Add));


    }
}


public class PiercingBulletsItem: AllObjectMB
{
    float extraPiercing=5f;
    float velo=3;

    protected override void OnEnable()
    {
        ObjectNameID="PiercingBulletsObject";
    }
    protected override void DefineModifierItem()
    {
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Piercing, new StatModifier(extraPiercing, StatModifier.ModifierType.Add));
        
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Knockback, new StatModifier(5, StatModifier.ModifierType.Add));
        
    }
}

public class BouncingBulletsItem: AllObjectMB
{

    float extraPiercing=2f;
    float extraDist=4f;

    protected override void OnEnable()
    {
        ObjectNameID="BouncingBulletsObject";
    }   

    protected override void DefineModifierItem()
    {
        modifierItem.ItemBulletEffects.Add(new EffectsAdd(NameEffectBullets.BounceOnHitEB, NameHitboxInBullet.Body));

        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.Piercing, new StatModifier(extraPiercing, StatModifier.ModifierType.Add));
        modifierItem.ItemGunStats.Add(Stat.StatTypeGun.DistMax, new StatModifier(extraDist, StatModifier.ModifierType.Add));        
    }
}