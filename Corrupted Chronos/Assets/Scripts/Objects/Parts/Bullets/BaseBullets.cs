using Ink;
using UnityEngine;
using System;

//probablement implementar un enum per diferents tipus de Bales (No creo que sea necesario)
//potser canviar-li el nom

//classe general, del que venen els diferents projectils, revisar en un futur
public struct InformationBullet
{
    public int damage;
    public float penetration;
    public float distEffec;
    public float distMax;
    public float speed;

    public static InformationBullet Default(bulletStatsSO so)
    {
        return new InformationBullet
        {
            damage = so.damage,
            penetration = so.penetration,
            distEffec = so.distEffective,
            distMax = so.distMax,
            speed = so.speed
        };
    }
}


public class BaseBullets : MonoBehaviour
{
    [HideInInspector] public Vector3 iniPos;

    public bulletStatsSO statsSO;
    public GameObject myCreator;
    private Rigidbody rb;
    private SpriteRenderer spr;

    public InformationBullet statsBullet;

    public BaseBullets()
    {
    }

    void Awake()
    {       
        rb = GetComponent<Rigidbody>();
        statsBullet = InformationBullet.Default(statsSO);
        spr = GetComponent<SpriteRenderer>();
        if (statsSO.sprite != null)
        {
            spr.sprite = statsSO.sprite;
        }       
    }

    void Start()
    {
        iniPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * statsBullet.speed;
        float distance = Vector3.Distance(iniPos, this.transform.position);
        //Debug.Log("Distance: " + distance +"__iniPos: "+iniPos+ "__transform.position: " + this.transform.position);
        if (distance > statsBullet.distMax)
        {
            Destroy(this.gameObject);
        }
    }
    
    public int ReturnDamage()
    {
        return statsBullet.damage;
    }
    public void DefinirBala(int nouMalBala, float distancia)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");

        statsBullet.damage += nouMalBala;
        statsBullet.distMax+= distancia;
        //Debug.Log($"mal1 {mal} naumal1{nouMalBala}");

    }
    
    public void DefinirBala(InformationBullet statsBase)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");

        statsBullet=statsBase;
        //Debug.Log($"mal1 {mal} naumal1{nouMalBala}");
    }  
}
