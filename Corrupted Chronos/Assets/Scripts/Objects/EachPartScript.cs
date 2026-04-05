using System;
using UnityEngine;


/*EXPLICACIÓ: Aquest script està a cada part, i afegeix el subscript de PartActions adient,
 aquests s'scripts s'activen, a data que escric aquest comentari, al script player
 */
public class EachPartScript : MonoBehaviour
{
    [Tooltip("Acaba'l amb .cs")]
    [SerializeField] private string typeName;
    [SerializeField] private bool ActivateOnStart=false;

    private Type componentType;
    
    //should be upgrated, mutlple firepoint, firepoints with size
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {
        if(ActivateOnStart) Activate();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GunBase Activate()
    { 
        
        
        GunBase ScriptPart=null;
        // ItemBase itemBase=null;
        componentType = Type.GetType(typeName);
        if (componentType != null)
        {
            //dd PartBase or ItemBase
            if (!componentType.IsSubclassOf(Type.GetType("AllObjectMB")))
            {
                Debug.LogError("tipus d'script a afegir no es correcte");
            }
            
            gameObject.AddComponent(componentType);
            

            if (!componentType.IsSubclassOf(Type.GetType("GunBase")))
            {
                ScriptPart= gameObject.GetComponent<GunBase>();
                if(ScriptPart)   ScriptPart.PassVariables(firepoint, bulletPrefab);
                else Debug.LogError("es fill de GunBase pero no es GunBase (wtf)");
            }
            

        }
        else
        {
            Debug.LogError($"Cannot find component type {typeName}");
        }
        return ScriptPart;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
