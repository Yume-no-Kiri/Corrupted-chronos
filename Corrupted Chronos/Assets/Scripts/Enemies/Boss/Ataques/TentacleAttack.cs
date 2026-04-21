using System.Collections;
using UnityEditor;
using UnityEngine;

public class TentacleAttack : MonoBehaviour
{
    Animator anim;
    public GameObject Tentacle;

    private Vector3 rotationToAdd= Vector3.zero;

    [Header ("Rise Attack")]
    public float DamageRise=4;
    public float Time2Rise=2;

    public Vector3 initialRiseRange=new Vector3(1,1,1), finalRiseRange=new Vector3(3,3,3);

    public GameObject IndicatorToRise;
    //should add damage

    public bool independent;
    

    [Header ("General Attack")]
    public float TimeBetweenAttacks=3;

    [Header ("Splash Attack")]
    public float TimePrepSA=2;
    public Vector3 RotPrepSA= new Vector3(-45,0,0);

    public float TimeDoingSA=3;
    public Vector3 RotDoingSA= new Vector3(90,0,0);


    public float TimeRecoverSA=1.5f;
    public Vector3 RotRecoverSA= new Vector3(0,0,0);


    public Vector3 Direction;

    private float attackTimer;
    public float attackCooldown=5f;

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

    void OnCollisionEnter(Collision collision)
    {
        //Detect player and do damage
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