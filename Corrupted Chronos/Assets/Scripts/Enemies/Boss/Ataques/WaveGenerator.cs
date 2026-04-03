using System.Collections;
using Unity.Mathematics;
using UnityEngine;


//this shoudl generate the waves, still work in progress
public class WaveGenerator : MonoBehaviour
{
    GameObject waveBullet;

    public float TimeBetweenShots=1;

    public AllPresetBullets allPresetBullets;
    private GameObject EmptyBullet;

    private GameObject bulletInst;
    private Coroutine coroutine=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        EmptyBullet=GameManager.Instance.bulletDatabase.emptyBullet;

    }

    public void SelectPreset(GameObject inst, AllPresetBullets presetBullets )
    {
        // GameObject newBullet=EmptyBullet;
        switch (presetBullets)
        {
            case AllPresetBullets.Basic:
                // newBullet.GetComponent<BaseBullets>().statsSO = 
                inst.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("BasicBullet"));
                inst.AddComponent<DamageEB>();
            break;
            case AllPresetBullets.Wave:
                // newBullet.GetComponent<BaseBullets>().statsSO=;
                inst.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("WaveBullet"));
                inst.AddComponent<KnockbackEB>();

            break;
            default:
            Debug.LogError("no tipo bala");
            break;
        }
        // return newBullet;
    }
    // Update is called once per frame
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
        bulletInst=Instantiate(EmptyBullet,transform.position, quaternion.identity, null);
        SelectPreset(bulletInst, allPresetBullets);
        coroutine=null;
    }

}
