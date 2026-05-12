using System;
using System.Collections.Generic;
using UnityEngine;


/*EXPLICACIÓ: Aquest script està a cada part, i afegeix el subscript de PartActions adient,
 aquests s'scripts s'activen, a data que escric aquest comentari, al script player
 */
public class EachPartScript : MonoBehaviour
{
    [Tooltip("Acaba'l amb .cs")]
    [SerializeField] ListNameParts listParts;
    [SerializeField] ListNameObjects listObjects;
    
    
    private string typeName;
    // [SerializeField] private bool ActivateOnStart=false;

    private Type componentType;
    
    //should be upgrated, mutlple firepoint, firepoints with size
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private SpriteRenderer sprite;
    private int IDPtoGive=-1;
    private bool componentAdded=false;
    void Awake()
    {
        if(listParts==ListNameParts.Null && listObjects == ListNameObjects.Null)
        {
            Debug.LogError(" eachPartSscript don't know what to create");
        }else if (listParts==ListNameParts.Null && listObjects != ListNameObjects.Null)
        {
            typeName= listObjects.ToString();
        }else if (listParts!=ListNameParts.Null && listObjects == ListNameObjects.Null)
        {
            typeName= listParts.ToString();
        }
    }

    void Start()
    {
        

        Activate();
        // if(ActivateOnStart) Activate();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Activate()
    { 
        
        
        GunBase ScriptPart=null;
        // ItemBase itemBase=null;sempre p
        componentType = Type.GetType(typeName);
        if (componentType != null)
        {
            //dd PartBase or ItemBase
            if (!componentType.IsSubclassOf(Type.GetType("AllObjectMB")))
            {
                Debug.LogError("tipus d'script a afegir no es correcte");
            }
            
            if (!this.GetComponent(componentType))
            {
                gameObject.AddComponent(componentType);

            }
            if (componentType.IsSubclassOf(Type.GetType("GunBase")))
            {
                ScriptPart= gameObject.GetComponent<GunBase>();
                if (IDPtoGive == -1)
                {
                    IDPtoGive=GameManager.Instance.GiveNextIdp();
                }
                ScriptPart.AssignIDP(IDPtoGive);
            }
            componentAdded=true;
            
        }
        else
        {
            Debug.LogError($"Cannot find component type {typeName}");
        }
        // return ScriptPart;
        
    }

    public GunBase ActivateGun() 
    {
        //maybe this could be separeted
        GunBase GunScriptPart=null;

        if(!componentAdded) Activate();
        if (componentType.IsSubclassOf(Type.GetType("GunBase")))
        {
            GunScriptPart= gameObject.GetComponent<GunBase>();
            
            if(GunScriptPart) { 
                GameManager.Instance.inputManager.OnShotLeft -= GunScriptPart.DoShot;
                GameManager.Instance.inputManager.OnShotRight -= GunScriptPart.DoShot;

                GunScriptPart.PassVariables(firepoint, bulletPrefab,sprite);
            }// else Debug.LogError("es fill de GunBase pero no es GunBase (wtf)");
        }/* else{
            
            Debug.LogWarning("possible ereror");
        } */
        return GunScriptPart;
    }


    /* public void AddEffectsBullets(List<EffectsAdd> effectsAdds)
    {
        if (GunScriptPart != null)
        {
            
        }
    }
    public void RemoveEffectsBullets(List<EffectsAdd> effectsAdds)
    {
        
    } */

    public void AssignIDP(int newIDP)
    {
        if (IDPtoGive == -1)
        {
            IDPtoGive=newIDP;
        }
        else
        {
            Debug.LogError("trying to assign IDP when it's already assign");
        }
        
    }
    public string returnName()
    {
        return typeName;
    }
    public int ReturnIDP()
    {
        return IDPtoGive;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
