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
    public GameObject EnemySpawner;

    // public ShipMovement shipInstance;

    [Header("Materials")]
    public Material WaterMaterial;

    [Header("Variables that should be in other places")]

    /*
    Guardem tota informació que s'haurà d'anar actualitzant, estats del jugador i coses així
    sobre actualització i acces de valors:
    
    */


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
            DontDestroyOnLoad(gameObject);
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
