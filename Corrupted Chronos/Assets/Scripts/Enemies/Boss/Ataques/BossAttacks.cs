using JetBrains.Annotations;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BossAttacks : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public GameObject headGO;
    Animator anim;
    public Transform bulletSpawnPoint;

    [Header("Move kraken")]
    public Vector3 newPos;

    [Header("Laser Attack")]
    public NameBulletPreset laserBulletPreset;
    public int laserBulletAmount;

    public KrakenController krakenController;

    // private Vector3 PosStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        

        

    }

    #region move kraken

    //Funcion la que se activa por el StateMachine
    //NO ES LA DE EL ANIMADOR
    public void krakenMove()
    {
        anim.SetTrigger("ChangePosition");
        newPos=generateNewPosition();
    }

    //Usada por el animator, cambia la posición del kraken en el momento correcto de la animación
    public void teleportKraken()
    {
        this.gameObject.transform.position = generateNewPosition();
    }

    public Vector3 generateNewPosition()
    {
        return new Vector3(Random.Range(-20, 20), transform.position.y, Random.Range(-20, 20));
    }


    #endregion

    #region water laser

    public void WaterLaser()
    {
        anim.SetTrigger("SpinAttack");
    }

    public void ShootLaser()
    {
        StartCoroutine(ShootLaserRoutine());
    }

    private IEnumerator ShootLaserRoutine()
    {
        float duration = 0.4f;

        float delayBetweenShots =
            duration / laserBulletAmount;

        float angleStep =
            360f / laserBulletAmount;

        for (int i = 0; i < laserBulletAmount; i++)
        {
            float angle = angleStep * i;

            Quaternion rotation =
                Quaternion.Euler(0f, angle, 0f);

            GameObject bulletInst = Instantiate(
                statsManager.instance.listBulletStats.GeneralBullet,
                bulletSpawnPoint.position,
                rotation
            );

            bulletInst
                .GetComponent<CreateBullet>()
                .Setup(false, laserBulletPreset);

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    #endregion 

    #region up and down attack

    public void HeadUpAndDown()
    {
        anim.SetTrigger("HeadAttack");
    }

    public void generateWaves()
    {

    }


    #endregion

    #region interface
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        krakenController.BossHealth -= maxHealth;
    }

    public void AddKnockback(Vector3 dir, float force)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}

#if UNITY_EDITOR

[CustomEditor(typeof(BossAttacks))]

public class BossAttacksEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BossAttacks script = (BossAttacks)target;
        if (GUILayout.Button("Test Water Lazer"))
        {
            script.WaterLaser();
        }
        if (GUILayout.Button("Test Up and Down"))
        {
            script.HeadUpAndDown();
        }
        if (GUILayout.Button("Kraken Move"))
        {
            script.krakenMove();
        }
        if (GUILayout.Button("Teleport"))
        {
            script.teleportKraken();
        }
    }
}

#endif