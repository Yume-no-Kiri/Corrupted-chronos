using System.Collections.Generic;
using UnityEngine;

public class KrakenController : MonoBehaviour
{
    #region Variables

    public GameObject player; //Perchance 
    public BossAttacks head;
    public List<GameObject> spawnedTentacles;

    public float BossHealth;


    [Header("Tentacle Options")]
    public float tentacleSpawnRadius;
    public float tentaclePlayerSpawnRadius;
    public int maxTentacles;
    public GameObject[] tentacles;
    

    public krakenStateMachine sm;

    Phase1 phase1;


    #endregion

    public void Awake()
    {
        sm = new krakenStateMachine(this);
        phase1 = new Phase1(sm);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sm.initialize(phase1); //TODO: Agregar el estado inicial del kraken
    }

    // Update is called once per frame
    void Update()
    {
        sm.currentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        sm.currentState.PhysicsUpdate();
    }

    public void bossDefeated()
    {
        GameManager.Instance.onBossDefeat();
        //Enviar mensaje de victoria al player
        for (int i = 0; i < spawnedTentacles.Count; i++)
        {
            if (spawnedTentacles[i] != null)
                Destroy(spawnedTentacles[i]);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("FINAL");
        Destroy(this.gameObject);
    }
}
