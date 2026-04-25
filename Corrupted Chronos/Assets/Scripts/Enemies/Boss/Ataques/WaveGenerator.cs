using System.Collections;
using Unity.Mathematics;
using UnityEngine;


//this shoudl generate the waves, still work in progress
public class WaveGenerator : MonoBehaviour
{
    // GameObject waveBullet;

    public float TimeBetweenShots=1;

    // public AllPresetBullets allPresetBullets;
    private GameObject GeneralBullet;

    private GameObject bulletInst;
    private Coroutine coroutine=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        GeneralBullet=GameManager.Instance.bulletDatabase.GeneralBullet;

    }
    void Update()
    {
        if (coroutine == null)
        {
            coroutine=StartCoroutine(Wait2shot());
            
        }
    }

    IEnumerator Wait2shot()
    {
        yield return new WaitForSeconds(TimeBetweenShots);
        bulletInst=Instantiate(GeneralBullet,transform.position, quaternion.identity, null);
        bulletInst.GetComponent<CreateBullet>().Setup(false,"WaveBullet");
        coroutine=null;
    }

}
