using UnityEngine;

public enum AllPresetBullets
{
    Basic,
    Wave, 
    Null


}

//waveGenerator has this and it works

public class CreateBullet: MonoBehaviour
{

    public AllPresetBullets allPresetBullets=AllPresetBullets.Null;
    public bool CreateMySelf=false;
    // private GameObject EmptyBullet;

    // public AllPresetBullets presetBullets;

    // override statsSO=WaveStatsSO;
    
    protected void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        if(CreateMySelf) Activate();
        // EmptyBullet=GameManager.Instance.bulletDatabase.emptyBullet;
        
    }

    public void Activate()
    {
        SelectPreset(allPresetBullets);
    }

    public void SelectPreset(AllPresetBullets presetBullets )
    {
        // GameObject newBullet=EmptyBullet;
        switch (presetBullets)
        {
            case AllPresetBullets.Basic:
                // newBullet.GetComponent<BaseBullets>().statsSO = 
                this.gameObject.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("BasicBullet"));
                this.gameObject.AddComponent<DamageEB>();
            break;
            case AllPresetBullets.Wave:
                // newBullet.GetComponent<BaseBullets>().statsSO=;
                this.gameObject.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("WaveBullet"));
                this.gameObject.AddComponent<KnockbackEB>();

            break;
            case AllPresetBullets.Null:
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
        
    }
}
