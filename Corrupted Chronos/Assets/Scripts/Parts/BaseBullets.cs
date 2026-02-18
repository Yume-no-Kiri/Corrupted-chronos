using Ink;
using UnityEngine;
using System;

//probablement implementar un enum per diferents tipus de Bales
//potser canviar-li el nom

//classe general, del que venen els diferents projectils, revisar en un futur
public struct InformationBullet
{
    public int mal;
    public float penetration;    //classe d'armadura? si l'enemic té menor armadura que la penetration, l'atravesa
    public float distEfec;  //distEfectiva: distania per rebre el mal maxim, passat distEfec, hi ha reducció del mal fins distMax, on mal bala desapareix
    public float distMax;
    public float speed;

    

    public InformationBullet ConstInformationBullet()
    {
        mal=5;
        penetration=5f;
        distEfec=10f;
        distMax = 20f;
        speed =10f;
        return this;
    }
}


public class BaseBullets : MonoBehaviour
{
    
    [HideInInspector] public Vector3 iniPos;
     

    //depen de que potser volem passar-li info al nostre creador, quan matem a un enemic, quan atravessem etc...
    GameObject myCreator;
    private Rigidbody rb;

   public InformationBullet statsBullet;
   /*  protected int mal; */

    //classe d'armadura? si l'enemic té menor armadura que la penetration, l'atravesa
   /*  private float penetration=5f;


    private float distEfec=10f;
    private float distMax = 20f;
    private float speed =10f;
 */
    
    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        statsBullet= new InformationBullet().ConstInformationBullet();
    }

    void Start()
    {
        iniPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity=transform.right*statsBullet.speed;
        float distance = Vector3.Distance(iniPos, this.transform.position);
        //Debug.Log("Distance: " + distance +"__iniPos: "+iniPos+ "__transform.position: " + this.transform.position);
        if (distance > statsBullet.distMax)
        {
            Destroy(this.gameObject);
        }
    }
    
    public int RetornaMal()
    {
        return statsBullet.mal;
    }
    public void DefinirBala(int nouMalBala, float distancia)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");

        statsBullet.mal += nouMalBala;
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
