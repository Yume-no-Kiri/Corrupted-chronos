using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    /*
    Guardem tota informació que s'haurà d'anar actualitzant, estats del jugador i coses així
    sobre actualització i acces de valors:
    Estaria subscriure scripts a certs grups de variables, si una variable d'aquest grup es canviada actualitzem als scripts subscrits, en comptes de cada frame tornar a preguntar per els valors
    
    */

    


    //demoment les stats del jugador aquí mateix, en un futur potser moure a un script separat i tindre'l també aquí

    //GRUP stats jugador //Podriem fer subgrups si fos necesari
    public float health;
    public float staminaMax=100f;
    public float staminaAct;
    public float staminaRegen=2.5f;
    public float staminaTime2Regen=2f;
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
        
    }
}
