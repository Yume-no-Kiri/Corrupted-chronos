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

    // public string NameBulletPreset;
    // public bool toSearch;
    protected void Awake(){}

    AllInformationBullet finalStats= new AllInformationBullet();

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

    //called by wave generator AND ENEMIES 
    public void Setup(bool createdByPlayer, string nameBullet)
    {
        CreatedByPlayer=createdByPlayer;
        // NameBulletPreset=nameBullet;
        
        AllInformationBullet? bulletStats= statsManager.instance.listBulletStats.ReturnBulletStatsSO(nameBullet);
        if(bulletStats==null) Debug.LogError("don't find name, revise Bullet Data Base");
        finalStats= bulletStats.Value;
        SelectPreset();//bulletStats.Value);
    }

    //calles ONLY by the guns of the player
    public void Setup(bool createdByPlayer, AllInformationBullet stats)
    {
        CreatedByPlayer=createdByPlayer;
        finalStats=stats;
        SelectPreset();
    }

    public void SelectPreset(  )
    {
        this.gameObject.GetComponent<BaseBullets>().AssignSO(finalStats);
        if(CreatedByPlayer) {  this.gameObject.GetComponent<BaseBullets>().AssignTarget("Player");}
        else{ this.gameObject.GetComponent<BaseBullets>().AssignTarget("Enemy");}
        foreach (var item in finalStats.Effects)
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
