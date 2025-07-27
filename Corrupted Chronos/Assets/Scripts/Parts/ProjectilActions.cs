using UnityEngine;

//probablement implementar un enum per diferents tipus de vales



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
}
