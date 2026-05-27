using System;
using Unity.Mathematics;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    [SerializeField] private int AsseguratedReward=20;
    [SerializeField] private float possiblityToReward;//=1/23;
    private int nTriesToReward=0;

    private int nRewardsTotal=0;

    // int NItemsSpawned=0;
    // int NgunsSpawned=0;
    TakableDataBase Rewards;
    int GunsDiversity=3; //diversitat d'armes

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rewards= GameManager.Instance.takableDataBase;        
    }

    // Update is called once per frame
    

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

    public void ProbablySpawnReward(Vector3 posReward)
    {
        Debug.Log("reward called ntries:"+nTriesToReward);
        if (nTriesToReward >= AsseguratedReward)
        {
            //spawn reward
            nTriesToReward=0;
            GenerateReward(posReward);
        }else{
            int pos= (int)UnityEngine.Random.Range(0,possiblityToReward);
            if (pos == 0)
            {
                nTriesToReward=0;
                GenerateReward(posReward);
                //spawn reward
            }
            else
            {
                nTriesToReward++;
            }
        }

        // nRewardsTotal++;
    }
}
