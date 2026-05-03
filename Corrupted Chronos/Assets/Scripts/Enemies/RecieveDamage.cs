using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public interface IDamageable
{
    public void TakeDamage(float amount);
    public void Die();
    public void AddKnockback(Vector3 dir, float force);
}
public interface IElemental<T>
{
    void Setup(IDamageable _target, T data);
}

public abstract class ElementalEffect<T>: MonoBehaviour, IElemental <T>
{
    protected IDamageable target;

    public void Setup(IDamageable _target, T data)
    {
        target=_target;
        OnSetup(data);
    }

    public abstract void OnSetup(T data);

}


//això hauria d'heretar de elementalEffect
public class FireEffect : MonoBehaviour
{
    private IDamageable target;
    private float damageForWave;
    private float durationEffect;
    private float durationWave=0.4f;
    private float charges=0;

    // Aquest mètode configura l'efecte
    public void Setup(IDamageable targetToBurn, float dmg, float time)
    {
        target = targetToBurn;
        damageForWave = dmg;
        durationEffect = time;
        charges=1;
        StartCoroutine(BurnRoutine());
    }

    public void BurnMore(float dmg, float time){
        durationEffect+=time/2;
        damageForWave+=dmg/2;
        charges++;
        durationWave-=0.01f* Random.Range( 0, 20);
    }
    private IEnumerator BurnRoutine()
    {
        float elapsed = 0;
        float elapsed2=0;

        while (elapsed < durationEffect && target!=null)
        {
            if (elapsed2 >= durationWave)
            {
                target.TakeDamage(damageForWave * Time.deltaTime);
                elapsed2=0;
            }
            elapsed2 += Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(this); // L'efecte s'elimina quan acaba
    }
}

//això hauria d'heretar de elementalEffect i això igual
public class AshesEffect : MonoBehaviour
{
    private IDamageable target;
    
    
    
}