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
    public PartBase Activate()
    { 
        
        
        PartBase ScriptPart=null;
        // ItemBase itemBase=null;
        componentType = Type.GetType(typeName);
        if (componentType != null)
        {
            //dd PartBase or ItemBase
            gameObject.AddComponent(componentType);
            ScriptPart= gameObject.GetComponent<PartBase>();
            if(ScriptPart)   ScriptPart.PassVariables(firepoint, bulletPrefab);
            // itemBase=

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
