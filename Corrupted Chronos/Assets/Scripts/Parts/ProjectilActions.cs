using UnityEngine;

//probablement implementar un enum per diferents tipus de Bales
//potser canviar-li el nom

//classe general, del que venen els diferents projectils, revisar en un futur
public class ProjectilActions : MonoBehaviour
{
    
    protected int mal;
    [HideInInspector] public Vector3 iniPos;
    
    private float distMax = 20f;
    private float speed =10f;

    private Rigidbody rb;
    
    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {
        iniPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity=transform.right*speed;
        float distance = Vector3.Distance(iniPos, this.transform.position);
        //Debug.Log("Distance: " + distance +"__iniPos: "+iniPos+ "__transform.position: " + this.transform.position);
        if (distance > distMax)
        {
            Destroy(this.gameObject);
        }
    }
    
    public int RetornaMal()
    {
        return mal;
    }
    public void DefinirBala(int nouMalBala, float distancia)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");

        mal += nouMalBala;
        distMax+= distancia;
        //Debug.Log($"mal1 {mal} naumal1{nouMalBala}");

    }
    
    
}
