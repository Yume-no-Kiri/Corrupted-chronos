using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner instance;
    public List<GameObject> bossTowers = new List<GameObject>();


    [Header("Kraken Parameters")]
    public GameObject bossPrefab;
    public Transform spawnPoint;
    public bool dialogue_done = false;
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
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
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
