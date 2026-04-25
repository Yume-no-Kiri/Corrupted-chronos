using System;
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

    // public AllPresetBullets allPresetBullets=AllPresetBullets.Null;
    public bool CreateMySelf=false;
    public bool CreatedByPlayer;

    public string NameBulletPreset;
    // public bool toSearch;
    protected void Awake(){}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        // if(CreateMySelf) Activate();
        // EmptyBullet=GameManager.Instance.bulletDatabase.emptyBullet;
        
    }

    public void Activate()
    {
        SelectPreset();
    }

    public void Setup(bool createdByPlayer, string nameBullet)
    {
        CreatedByPlayer=createdByPlayer;
        NameBulletPreset=nameBullet;
        SelectPreset();
    }
    public void SelectPreset()//AllPresetBullets presetBullets )
    {
       
        BulletStatsSO bulletStats= GameManager.Instance.bulletDatabase.ReturnBulletStatsSO(NameBulletPreset);
        if(bulletStats==null) Debug.LogError("don't find name, revise Bullet Data Base");
        this.gameObject.GetComponent<BaseBullets>().AssignSO(bulletStats);
        if(CreatedByPlayer) {  this.gameObject.GetComponent<BaseBullets>().AssignTarget("Player");}
        else{ this.gameObject.GetComponent<BaseBullets>().AssignTarget("Enemy");}
        foreach (var item in bulletStats.Effects)
        {
            Type type=Type.GetType(item.nameEffect);
            Component script=this.gameObject.AddComponent(type);
            EffectsBullets effectsBullets=script as EffectsBullets;
            if (effectsBullets!=null)
            {
                effectsBullets.Setup(item.setup);
            }
        }

        // this.gameObject.GetComponent<BaseBullets>().AssignSO(GameManager.Instance.bulletDatabase.ReturnBulletStatsSO("BasicBullet"));


        // return newBullet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
