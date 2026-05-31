using Ink.Parsed;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}


    [field: SerializeField] public RewardSpawner rewardSpawner{get; private set;}

    public GameObject defeatCanvas;
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
    public GameObject EnemySpawner;

    // public ShipMovement shipInstance;

    [Header("Materials")]
    public Material WaterMaterial;

    [Header("Variables that should be in other places")]

    /*
    Guardem tota informació que s'haurà d'anar actualitzant, estats del jugador i coses així
    sobre actualització i acces de valors:
    
    */

    


    //demoment les stats del jugador aquí mateix, en un futur potser moure a un script separat i tindre'l també aquí

    // public float health;
    
    //stamina stuff
    /* public float staminaMax=100f;
    public float staminaAct;
    public float staminaRegenQuantity=2.5f;
    public float staminaUseQuantity=10f;
    public float staminaTime2Regen=4f;
    public bool StaminaRegen=false;
    public Coroutine CoroutineRegenStamina=null;
    public Coroutine CoroutineConsumeStamina=null;

    public bool IsStaminaEmpty {get; private set;}
    public float staminaMoveUPUseQuantity=5f;
 */
    // public float EnemyKnockbackResistance=2f;


    // public float moveSpeedNau=100f;
    [NonSerialized]

    public float moveSpeedPilot=4f;

    [NonSerialized]
    public float rotationSpeed=50f;
    [NonSerialized]

    public float rotationSpeedRight=80f;
    [NonSerialized]

    public float rotationSpeedLeft=80f;
    // public float dashingForce=20f;


    //to ceate items and parts, so we can identify diferent items and parts even if they are the same but repeated
    private int _countIDP=0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance=this;
        }
        else
        {
            Destroy(gameObject);
        }

        placementDataBase.StartConfigPositions();
        takableDataBase.StartConfigItem();
        allObjectsDataBase.StartConfigObject();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WaterMaterial.SetVector("_PositionPlayer", playerInstance.transform.position);
  
    }

    public void DeactivateOrActivateEnemies(bool enable)
    {
        if(EnemySpawner!=null) EnemySpawner.SetActive(enable);
        else Debug.LogWarning("EnemySpawner not assigned");
    }

    public void onBossDefeat()
    {

    }

    public void onPlayerDeath()
    {
        //desactivar el jugador, mostrar pantalla de muerte, etc
        defeatCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    #region ForParts
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
    #endregion
    #region rewards

    public void EnemyKilled(Vector3 posReward)
    {
        rewardSpawner.ProbablySpawnReward(posReward);
    }
    #endregion
}
