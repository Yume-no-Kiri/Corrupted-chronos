using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TentacleAttack : MonoBehaviour, IDamageable
{
    public float health;
    Animator anim;
    public GameObject Tentacle;

    private Vector3 rotationToAdd= Vector3.zero;

    
    
    public ParticleSystem hitParticles;
    public bool independent;
    public KrakenController controller;

    public Vector3 Direction;

    private float attackTimer;
    public float attackCooldown=5f;
    public Transform spawnPoint;

    [Header("Slam Attack")]
    public NameBulletPreset slamBulletPreset;
    public float slamBulletAmount;

    [Header("Stab Attack")]
    public NameBulletPreset stabBulletPreset;

    [Header("Spin Attack")]
    public NameBulletPreset spinBulletPreset;

    [Header("Splash Attack")]
    public NameBulletPreset splashBulletPreset;
    public float splashBulletAmount;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.playerInstance != null)
        {
            Direction = GameManager.Instance.playerInstance.transform.position - this.transform.position;
            float angle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            Vector3 anglesActuals = Tentacle.transform.eulerAngles;
            rotationToAdd.y = angle-90;
            Tentacle.transform.eulerAngles = rotationToAdd;
        }

        if (independent)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0f;
                switch (Random.Range(0, 4))
                {
                    case 0:
                        SplashAttack();
                    break;
                    case 1:
                        DirectAttack();
                    break;
                    case 2:
                        SpinAttack();
                        break;
                    case 3:
                        SplashAttack();
                        break;
                }
            }
        }
    }

    //here he should add the methods to do the direct attack


    public void slamAttack()
    {
        anim.SetTrigger("Attack1");
    }

    public void DirectAttack()
    {
        anim.SetTrigger("Attack2");
    }

    public void SpinAttack()
    {
        anim.SetTrigger("Attack3");
    }

    public void SplashAttack()
    {
        anim.SetTrigger("Attack4");
    }



    public void SpawnSlamBullets()
    {
        float angleStep = 360f / slamBulletAmount;

        for (int i = 0; i < slamBulletAmount; i++)
        {
            float angle = angleStep * i;

            Quaternion rotation =
                Quaternion.Euler(
                    0f,
                    angle,
                    0f
                );

            GameObject bulletInst = Instantiate(
                statsManager.instance.listBulletStats.GeneralBullet,
                spawnPoint.position,
                rotation
            );

            bulletInst
                .GetComponent<CreateBullet>()
                .Setup(false, slamBulletPreset);
        }
    }

    public void SpawnSplashBullets()
    {
        float angleStep = 180f / (splashBulletAmount - 1);

        float startAngle = 0f;

        for (int i = 0; i < splashBulletAmount; i++)
        {
            float angle = startAngle + (angleStep * i);

            Quaternion rotation =
                transform.rotation *
                Quaternion.Euler(0f, angle, 0f);

            GameObject bulletInst = Instantiate(
                statsManager.instance.listBulletStats.GeneralBullet,
                transform.position,
                rotation
            );

            bulletInst
                .GetComponent<CreateBullet>()
                .Setup(false, splashBulletPreset);
        }
    }

    public void SpawnStabBullet()
    {
        GameObject bulletInst = Instantiate(
            statsManager.instance.listBulletStats.GeneralBullet,
            spawnPoint.position,
            transform.rotation
        );
        bulletInst
            .GetComponent<CreateBullet>()
            .Setup(false, stabBulletPreset);
    }

    public void SpawnSpinBullet()
    {
        GameObject bulletInst = Instantiate(
            statsManager.instance.listBulletStats.GeneralBullet,
            spawnPoint.position,
            transform.rotation
        );
        bulletInst
            .GetComponent<CreateBullet>()
            .Setup(false, stabBulletPreset);
    }

    /* 
     call in start:

         StartCoroutine(SplashAttack());
    
    public IEnumerator SplashAttack()
    {
        Tentacle.SetActive(true);
        IndicatorToRise.SetActive(false);
        yield return StartCoroutine(PrepSplash());
        yield return StartCoroutine(DoingSplash());
        yield return StartCoroutine(RecoverSplash());
    }
    IEnumerator PrepSplash()
    {
        float timePassed = 0f;
        while (timePassed < TimePrepSA)
        {
            float progressio = timePassed / TimePrepSA;
            Vector3 rot= Vector3.Lerp(Vector3.zero, RotPrepSA, progressio);
            // rot.y=Tentacle.transform.rotation.y;
            // Tentacle.transform.eulerAngles=rot;
            rotationToAdd=rot;
            timePassed += Time.deltaTime;
            yield return null;
        }
        // yield return new WaitForSeconds(TimePrepSA);
    }

    IEnumerator DoingSplash()
    {
        float timePassed = 0f;
        float timeSmashSA= TimeDoingSA/9;
        while (timePassed < timeSmashSA)
        {
            float progressio = timePassed / timeSmashSA;
            Vector3 rot= Vector3.Lerp(RotPrepSA, RotDoingSA, progressio);
            // rot.y=Tentacle.transform.rotation.y;
            rotationToAdd=rot;
            // Tentacle.transform.eulerAngles=rot;
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed += Time.deltaTime;
        float TimeDoingSA2= TimeDoingSA-timePassed;
        //HERE SPAWN WAVES

        yield return new WaitForSeconds(TimeDoingSA2);
        
    }
    IEnumerator RecoverSplash()
    {
        float timePassed = 0f;
        while (timePassed < TimeRecoverSA)
        {
            float progressio = timePassed / TimeRecoverSA;
            Vector3 rot= Vector3.Lerp(RotDoingSA, RotRecoverSA, progressio);
            // rot.y=Tentacle.transform.rotation.y;
            // Tentacle.transform.eulerAngles=rot;
            rotationToAdd=rot;
            timePassed += Time.deltaTime;
            yield return null;
        }
        // yield return new WaitForSeconds(TimeRecoverSA);
    }
    */

    public void TakeDamage(float amount)
    {
        health -= amount;
        hitParticles.Stop();
        hitParticles.Play();
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        controller.spawnedTentacles.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void AddKnockback(Vector3 dir, float force)
    {
        throw new System.NotImplementedException();
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(TentacleAttack))]

public class TentacleAttackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TentacleAttack script = (TentacleAttack)target;
        if (GUILayout.Button("Slam Attack"))
        {
            script.slamAttack();
        }
        if (GUILayout.Button("Direct Attack"))
        {
            script.DirectAttack();
        }
        if (GUILayout.Button("Spin Attack"))
        {
            script.SpinAttack();
        }
        if (GUILayout.Button("Splash Attack"))
        {
            script.SplashAttack();
        }
    }
}

#endif