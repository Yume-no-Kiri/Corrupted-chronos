using UnityEngine;

public enum AllPresetBullets
{
    Basic,
    Wave


}

public class CreateBullet: MonoBehaviour
{
    private GameObject EmptyBullet;

    // public AllPresetBullets presetBullets;

    // override statsSO=WaveStatsSO;
    
    protected void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        EmptyBullet=GameManager.Instance.bulletDatabase.emptyBullet;

    }

    public GameObject SelectPreset(AllPresetBullets presetBullets )
    {
        GameObject newBullet=EmptyBullet;
        switch (presetBullets)
        {
            case AllPresetBullets.Basic:
                // newBullet.GetComponent<BaseBullets>().statsSO = 
                newBullet.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("BasicBullet"));
                newBullet.AddComponent<DamageEB>();
            break;
            case AllPresetBullets.Wave:
                // newBullet.GetComponent<BaseBullets>().statsSO=;
                newBullet.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("WaveBullet"));
                newBullet.AddComponent<KnockbackEB>();

            break;
            default:
            Debug.LogError("no tipo bala");
            break;
        }
        return newBullet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
