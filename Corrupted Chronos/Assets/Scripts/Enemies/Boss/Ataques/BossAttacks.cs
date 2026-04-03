using System.Collections;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public GameObject headGO;

    [Header ("up and down Attack")]
    public float Time2Up=2;
    public Vector3 PosUp;

    public float Time2Down=0.5f;
    public Vector3 PosDown;

    [Header ("water lazer Attack")]
    public GameObject Lazer;
    public float TimeTurn=2f;
    
    [Header ("Move kraken")]


    private Coroutine coroutine=null;
    // private Vector3 PosStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PosUp+= transform.position;
        PosDown+= transform.position;
        Lazer.SetActive(false);
       
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    #region move kraken

    #endregion

    #region water lazer
    /* call in start:
        StartCoroutine(WaterLazer(transform.rotation.eulerAngles));
     */
    IEnumerator WaterLazer(Vector3 rotOri)
    {
        Lazer.SetActive(true);
        yield return StartCoroutine(TurnLazer(rotOri));
        Lazer.SetActive(false);
    }
    
    IEnumerator TurnLazer(Vector3 rotOri){
        Vector3 rotFinal=rotOri+new Vector3(0,360,0);

        // Vector3 escalaInicial = IndicatorToRise.transform.localScale;
        float timePassed = 0f;

        while (timePassed < TimeTurn)
        {
            float progressio = timePassed / TimeTurn;
            headGO.transform.eulerAngles = Vector3.Lerp(rotOri, rotFinal, progressio);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    #endregion 

    #region up and down attack

    /*  To call it in update:
        if (coroutine == null)
        {
            coroutine= StartCoroutine(HeadUpAndDown());

        } */
    IEnumerator HeadUpAndDown()
    {
        yield return StartCoroutine(HeadUp());
        yield return StartCoroutine(HeadDown());
        coroutine=null;
    }
    IEnumerator HeadUp(){
        // Vector3 escalaInicial = IndicatorToRise.transform.localScale;
        float timePassed = 0f;

        while (timePassed < Time2Up)
        {
            float progressio = timePassed / Time2Up;
            headGO.transform.position = Vector3.Lerp(PosDown, PosUp, progressio);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator HeadDown(){
        // Vector3 escalaInicial = IndicatorToRise.transform.localScale;
        float timePassed = 0f;

        while (timePassed < Time2Down)
        {
            float progressio = timePassed / Time2Down;
            headGO.transform.position = Vector3.Lerp(PosUp,PosDown, progressio);
            timePassed += Time.deltaTime;
            yield return null;
        }
        //HERE GENERATE WAVES
    }
    #endregion
}
