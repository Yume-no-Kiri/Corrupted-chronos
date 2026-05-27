using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner instance;
    public List<GameObject> bossTowers = new List<GameObject>();


    [Header("Kraken Parameters")]
    public GameObject bossPrefab;
    public Transform spawnPoint;
    public bool dialogue_done = false;

    public GameObject krakenCam;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void towerDestroyed(GameObject obj)
    {
        if (obj == null || bossTowers == null || bossTowers.Count == 0)
            return;

        bossTowers.Remove(obj);
    }

    public void spawnBoss()
    {
        StartCoroutine(spawnBossCinematic());
        
    }

    public IEnumerator spawnBossCinematic()
    {
        krakenCam.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        bossSetup();

        
        yield return new WaitForSeconds(2f);
        krakenCam.SetActive(false);
    }

    void bossSetup()
    {
        var obj = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        KrakenController control = obj.GetComponent<KrakenController>();
        control.player = GameManager.Instance.playerInstance;
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

#if UNITY_EDITOR

[CustomEditor(typeof(BossSpawner))]
public class BossSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BossSpawner spawner = (BossSpawner)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Boss"))
        {
            spawner.spawnBoss();
        }
    }
}
#endif