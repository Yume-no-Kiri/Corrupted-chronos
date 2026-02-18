using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    /*
    Guardem tota informació que s'haurà d'anar actualitzant, estats del jugador i coses així
    sobre actualització i acces de valors:
    
    */

    


    //demoment les stats del jugador aquí mateix, en un futur potser moure a un script separat i tindre'l també aquí

    public float health;
    
    //stamina stuff
    public float staminaMax=100f;
    public float staminaAct;
    public float staminaRegenQuantity=2.5f;
    public float staminaUseQuantity=10f;
    public float staminaTime2Regen=4f;
    public bool StaminaRegen=false;
    public Coroutine CoroutineStamina;

    public float staminaMoveUPUseQuantity=5f;


    public float moveSpeedNau=10f;
    [NonSerialized]

    public float moveSpeedPilot=4f;

    [NonSerialized]
    public float rotationSpeed=50f;
    [NonSerialized]

    public float rotationSpeedRight=80f;
    [NonSerialized]

    public float rotationSpeedLeft=80f;


    public float dashingForce=20f;



    //variable que es crida d'altres mètodes per voler volar i usar la stamina
    public bool want2Fly=false;
    //variable que respon gameManager i contesta a si es pot volar, osigui usar la stamina
    public bool staminaInUse { get; private set;}

    //modificar segons si implementem speeds diferents segons on estan els propulsors
    

    //podriem tenir referencia del objecte player si fos necesari

    private void Awake()
    {
        staminaAct=staminaMax;
        if(Instance == null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if ens falta stamina, iniciem coroutine per si podem regenerar
        if(staminaAct<=100) CoroutineStamina=StartCoroutine(TimerRegenStamina());
        if (StaminaRegen)
        {
            //regenerem
            staminaAct+=staminaRegenQuantity*Time.deltaTime;

            //si màxim apaguem
            if (staminaAct>= staminaMax)
            {
                StaminaRegen=false;
            }
        }

        //si ens ha dit que vol utiltizar stamina
        if (want2Fly)
        {
            //parem regeneració
            StopCoroutine(CoroutineStamina);
            StaminaRegen=false;

            //podem usar stamina o no
            if(staminaAct>0) staminaInUse=true;
            else {
                staminaInUse=false;
                return;
            }

            //gastar stamina
            staminaAct-=staminaUseQuantity*Time.deltaTime;
            
            //ja no podem usar stamina
            if (staminaAct <= 0)
            {
                staminaInUse=false;
                want2Fly=false;
            }
        }
        print("staminaAct: "+staminaAct);
    }



    IEnumerator TimerRegenStamina()
    {
        yield return new WaitForSeconds(staminaTime2Regen);
        StaminaRegen=true;
    }

    public void MoveUpStamina()
    {
        staminaAct-=staminaMoveUPUseQuantity; 
    }
}
