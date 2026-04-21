using JetBrains.Annotations;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public GameObject headGO;
    Animator anim;

    [Header("Move kraken")]
    public Vector3 newPos;


    // private Vector3 PosStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
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
     

    /* call in start:
        StartCoroutine(WaterLazer(transform.rotation.eulerAngles));
     
    public IEnumerator WaterLazer(Vector3 rotOri)
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
    }*/

    #endregion 

    #region up and down attack

    public void HeadUpAndDown()
    {
        anim.SetTrigger("HeadAttack");
    }

    public void generateWaves()
    {

    }

    /*
        to call it in start only one:
        StartCoroutine(HeadUpAndDown());
    
      To call it in update consecutivamente:
        if (coroutine == null)
        {
            coroutine= StartCoroutine(HeadUpAndDown());

        } 
    public IEnumerator HeadUpAndDown()
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
    }*/
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