using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    
    public GameObject Tentacle;

    private Vector3 rotationToAdd= Vector3.zero;

    [Header ("Rise Attack")]
    public float DamageRise=4;
    public float Time2Rise=2;

    public Vector3 initialRiseRange=new Vector3(1,1,1), finalRiseRange=new Vector3(3,3,3);

    public GameObject IndicatorToRise;
    //should add damage
    

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
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tentacle.SetActive(true);
        IndicatorToRise.SetActive(false);
        
        StartCoroutine(SplashAttack());
    }

    // Update is called once per frame
    void Update()
    {
        Direction=GameManager.Instance.playerInstance.transform.position-this.transform.position;
        float angle =Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
        Vector3 anglesActuals = Tentacle.transform.eulerAngles;
        rotationToAdd.y=angle;
        
        // anglesActuals.y = angle;
        

        Tentacle.transform.eulerAngles=rotationToAdd;
    }
    #region DirectAttack
    //here he should add the methods to do the direct attack

    #endregion


    #region SplashAttack

    /* 
     call:
        Tentacle.SetActive(true);
        IndicatorToRise.SetActive(false);
        
         StartCoroutine(SplashAttack());
     */
    IEnumerator SplashAttack()
    {
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
    #endregion



    #region tentacle rise
    /* 
        call:
         Tentacle.SetActive(false);
        IndicatorToRise.SetActive(true);
        
        RiseTentacle();
    */

    public void RiseTentacle()
    {
        Tentacle.SetActive(false);
        IndicatorToRise.transform.localScale=initialRiseRange;
        StartCoroutine(TentacleRise());
    }


    IEnumerator TentacleRise(){
        // Vector3 escalaInicial = IndicatorToRise.transform.localScale;
        float timePassed = 0f;

        while (timePassed < Time2Rise)
        {
            float progressio = timePassed / Time2Rise;
            IndicatorToRise.transform.localScale = Vector3.Lerp(initialRiseRange, finalRiseRange, progressio);
            timePassed += Time.deltaTime;
            yield return null;
        }
        
        IndicatorToRise.SetActive(false);
        Tentacle.SetActive(true);

    }
    #endregion

    
    void OnCollisionEnter(Collision collision)
    {
        //Detect player and do damage
    }
}
