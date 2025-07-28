using UnityEngine;

//probablement implementar un enum per diferents tipus de Bales
//potser canviar-li el nom

//classe general, del que venen els diferents projectils, revisar en un futur
public class ProjectilActions : MonoBehaviour
{
    
    protected int mal;
    [HideInInspector] public Vector2 iniPos;
    
    private float distMax = 50;
    private float speed =15f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public int RetornaMal()
    {
        return mal;
    }
    public void DefinirBala(int nouMalBala)
    {
        // Debug.Log($"mal0 {mal} naumal0{nouMalBala}");

        mal = nouMalBala;
        //Debug.Log($"mal1 {mal} naumal1{nouMalBala}");

    }
    
    
}
