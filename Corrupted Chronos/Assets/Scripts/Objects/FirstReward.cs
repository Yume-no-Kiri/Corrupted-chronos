using System.Collections;
using UnityEngine;

public class FirstReward : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerateReward());
    }


    IEnumerator GenerateReward()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        GameManager.Instance.FirstGun(this.transform.position);
    }

}
