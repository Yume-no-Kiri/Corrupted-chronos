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
    public float staminaTime2Regen=2f;
    public bool StaminaRegen=false;
    public Coroutine CoroutineStamina;

    public float moveSpeedNau=10f;
    public float moveSpeedPilot=4f;
    //modificar segons si implementem speeds diferents segons on estan els propulsors
    

    //podriem tenir referencia del objecte player si fos necesari

    private void Awake()
    {
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
        if(staminaAct<=0) CoroutineStamina=StartCoroutine(TimerRegenStamina());
        if (StaminaRegen)
        {
            staminaAct+=staminaRegenQuantity*Time.deltaTime;
        }
        if (staminaAct== staminaMax)
        {
            StaminaRegen=false;
        }
    }



    IEnumerator TimerRegenStamina()
    {
        yield return staminaTime2Regen;
        StaminaRegen=true;
    }
}
