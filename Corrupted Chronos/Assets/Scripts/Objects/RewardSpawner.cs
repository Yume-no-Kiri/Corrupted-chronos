using System;
using Unity.Mathematics;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    int NItemsSpawned=0;
    int NgunsSpawned=0;
    TakableDataBase Rewards;
    int GunsDiversity=3; //diversitat d'armes

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rewards= GameManager.Instance.takableDataBase;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateFirstGun(Vector3 position){
        
        int index= UnityEngine.Random.Range(0, GunsDiversity);
        GameObject gun= Rewards.GetInstantiateFromNumber(index);
        Instantiate( gun, position, quaternion.identity);
        // InstantiateFromNumber
    }

    void GenerateReward(Vector3 position)
    {
        int index= UnityEngine.Random.Range(0, Rewards.listItems.Count);
        GameObject gun= Rewards.GetInstantiateFromNumber(index);
        Instantiate( gun, position, quaternion.identity);
    }
}
