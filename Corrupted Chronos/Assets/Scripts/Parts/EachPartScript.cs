using System;
using UnityEngine;


/*EXPLICACIÓ: Aquest script està a cada part, i afegeix el subscript de PartActions adient,
 aquests s'scripts s'activen, a data que escric aquest comentari, al script player
 */
public class EachPartScript : MonoBehaviour
{
    [Tooltip("Acaba'l amb .cs")]
    [SerializeField] private string typeName;
    private Type componentType;
    
    //variables per partaccions.cs
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bulletPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PartActions Activate()
    { 
        
        //el script que afegeix només s'activa quan player.cs ho diu
        //Possible millora futura: Crear molts scripts, per cada acció, una part està formada per multiples d'aquests scripts,
        //Part actions o EachPartScript(aquest script), poddrien controlar amb llistes totes aquestes coses
        PartActions ScriptPart=null;
        componentType = Type.GetType(typeName);
        if (componentType != null)
        {
            gameObject.AddComponent(componentType);
            ScriptPart= gameObject.GetComponent<PartActions>();
            ScriptPart.PassVariables(firepoint, bulletPrefab);
            
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
