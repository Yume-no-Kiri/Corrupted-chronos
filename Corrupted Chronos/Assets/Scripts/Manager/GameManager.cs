using System;
using System.Collections;
using Ink.Parsed;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}


    [field: SerializeField] public RewardSpawner rewardSpawner{get; private set;}


    [Header("DataBases")]
    public AllObjectsDataBase allObjectsDataBase;
    public TakableDataBase takableDataBase;
    public PlacementDatabaseSO placementDataBase;

    // [Header ("Stats")]
    // public ListPartStats partDatabase;

    [Header("Manager")]
    // public GameObject HitboxBulletPrefab;
    public PlacementSystem placementSystem;
    
    public InputManager inputManager;

    public GameObject playerInstance;
    // public ShipMovement shipInstance;

    [Header("Materials")]
    public Material WaterMaterial;

    [Header("Variables that should be in statsManager")]

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
    public Coroutine CoroutineRegenStamina=null;
    public Coroutine CoroutineConsumeStamina=null;

    public bool IsStaminaEmpty {get; private set;}
    public float staminaMoveUPUseQuantity=5f;

    public float knockbackResistance=2f;


    public float moveSpeedNau=100f;
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


    //to ceate items and parts, so we can identify diferent items and parts even if they are the same but repeated
    private int _countIDP=0;

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

        placementDataBase.StartConfigPositions();
        takableDataBase.StartConfigItem();
        allObjectsDataBase.StartConfigObject();

        staminaAct=staminaMax;
        IsStaminaEmpty=false;

        
        // shipInstance= playerInstance.GetComponent<ShipMovement>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Para crear un nuevo modificador
        /* statsManager.instance.AddModifier(Stat.StatType.Stamina, new StatModifier()
        {
            type = StatModifier.ModifierType.Add,
            value = 2.5f,
            source = null
        });

        //Para obtener un stat
        statsManager.instance.GetStat(Stat.StatType.Stamina); */
    }

    // Update is called once per frame
    void Update()
    {
        WaterMaterial.SetVector("_PositionPlayer", playerInstance.transform.position);
        Debug.Log("stamina:"+staminaAct);

        //if ens falta stamina, iniciem coroutine per si podem regenerar
        if(staminaAct<100 && CoroutineRegenStamina==null &&CoroutineConsumeStamina==null){
             CoroutineRegenStamina=StartCoroutine(TimerRegenStamina());
        }
        /* if (StaminaRegen)
        {
            //regenerem
            // staminaAct += staminaRegenQuantity*Time.deltaTime;

            //si màxim apaguem
            if (staminaAct>= staminaMax)
            {
                StaminaRegen=false;
            }
        } */

        //si ens ha dit que vol utiltizar stamina
        /* if (want2Fly)
        {
            //parem regeneració
            StopCoroutine(CoroutineRegenStamina);
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
        } */
        //print("staminaAct: "+staminaAct);
    }
    
    public void InformIfGround(bool IsGround)
    {
        if (IsGround)
        {
            if(CoroutineConsumeStamina!=null){ 
                StopCoroutine(CoroutineConsumeStamina);
                CoroutineConsumeStamina=null;
            }
        }
    }
    public bool CanFly()
    {
        return IsStaminaEmpty? false:true;
        /* if (!IsStaminaEmpty)
        {
            return true;
        }
        else
        {
            return false;
        } */
    }

    public void MoveUpStamina(bool ground)
    {
        if(!ground){
            if (CoroutineRegenStamina != null)
            {
                StopCoroutine(CoroutineRegenStamina);
                CoroutineRegenStamina=null;
            }
            if(CoroutineConsumeStamina!=null) StopCoroutine(CoroutineConsumeStamina);
            CoroutineConsumeStamina= StartCoroutine(UseStamina());
        }// staminaAct-=staminaMoveUPUseQuantity; 
    }



    IEnumerator TimerRegenStamina()
    {
        yield return new WaitForSeconds(staminaTime2Regen);
        StaminaRegen=true;
        while(staminaAct<staminaMax){
            yield return new WaitForSeconds(0.1f);
            staminaAct+=staminaRegenQuantity;    
            IsStaminaEmpty=false;
        }
        CoroutineRegenStamina=null;
    }
    
    IEnumerator UseStamina()
    {
        if(CoroutineRegenStamina!=null){ 
            StopCoroutine(CoroutineRegenStamina);
            CoroutineRegenStamina=null;    
        }
        while(staminaAct>0){
            yield return new WaitForSeconds(0.1f);
            staminaAct-=staminaMoveUPUseQuantity;    
        }
        staminaAct=0;
        IsStaminaEmpty=true;
        CoroutineConsumeStamina=null;
    }


    public PartAdder returnGarageAdder()
    {
        if( !playerInstance.GetComponent<PartAdder>()) Debug.LogError("NO HI HA GARAGEADDER");
        return playerInstance.GetComponent<PartAdder>();
    } 
    public GameObject returnAdderParts()
    {
        return playerInstance.transform.Find("AdderParts").gameObject;
    }

    //revisar que es cridi correctament
    public int GiveNextIdp()
    {
        _countIDP++;
        return _countIDP;
    }
    #region rewards

    public void EnemyKilled(Vector3 posReward)
    {
        rewardSpawner.ProbablySpawnReward(posReward);
    }
    #endregion
}
