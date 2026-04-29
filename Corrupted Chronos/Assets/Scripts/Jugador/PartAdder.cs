using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;




public class PartAdder : MonoBehaviour
{
    InventoryManager inventoryManager;

    //to activate, enable parts with the movement, shoting, will have things for the inventary too
    #region called from placement system
    private List<PartPosition> ToAddParts ;

    //this structure es kinda shitty
    private Dictionary<PartPosition, GameObject> AddedParts;

    private GameObject _nauGO;

    public GameObject adder{get; private set;}

    private List<GameObject> AddedItems;

    private struct PartPosition
    {
        public GameObject gameobject;
        public Vector3 position;
        public quaternion rotation;

        public int IDP;

        public PartPosition(GameObject gb, Vector3 pos, quaternion rot, int idp) : this()
        {
            this.gameobject = gb;
            this.position = pos;
            this.rotation= rot;
            this.IDP=idp;
        }
    }
    public void Awake()
    {
        ToAddParts= new List<PartPosition>();
        AddedParts= new Dictionary<PartPosition, GameObject>();
        AddedItems= new List<GameObject>();
    }

    void Start()
    {
        inventoryManager=gameObject.GetComponent<InventoryManager>();
    }

    public void AssignAdder(GameObject adderAux, GameObject nauAux)
    {
        adder=adderAux;
        _nauGO=nauAux;
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        
    }

    public void AddPart(GameObject gb, Vector3 pos, quaternion rot, int thisIdp)
    {
        ToAddParts.Add(new PartPosition(gb,pos,rot, thisIdp));
    }

    private GameObject CreatePart(PartPosition cpart , Vector3 originNau)
    {
        Vector3 offset= new Vector3(0.25f,0,0.25f);
        GameObject gb= cpart.gameobject;
        Vector3 position=cpart.position;
        quaternion rot= cpart.rotation;
        //Aquesta marabunda de codi funciona :D
        Vector3 newPos=position-ToAddParts[0].position;
        Debug.Log("position will spawn1:"+  newPos.ToString());

        GameObject instPart = Instantiate(gb,adder.transform);
        instPart.transform.localRotation=rot;
        instPart.transform.localPosition=newPos+offset;
        instPart.GetComponent<EachPartScript>().AssignIDP(cpart.IDP);

        //falta assignar la mateixa IDP
        Transform coll = instPart.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(true);
        return instPart;
        
    }
    
  
    public void ActivateParts()
    {
        //acabar
        GunBase pa;
        // foreach (Transform inventoryPart in AddedParts.Values.Tra)
        foreach (Transform part in adder.transform)
        {
            //no estic segur de que part actions segui lo millor per invocar aquests mètodes,
            //revisar explicació escrita en EachPartScript per futur REFACTORITZACIÓ
           



            pa= part.GetComponent<EachPartScript>().ActivateGun();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.LogWarning("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.LogWarning("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    GameManager.Instance.inputManager.OnShotLeft += pa.DoShot;
                    break;
                case TypePart.ShootableRight: 
                    GameManager.Instance.inputManager.OnShotRight += pa.DoShot;
                    break;
            }
        }
    }


    public void DeactivatePart()
    {
        //reworkejar això amb addedParts
        GunBase pa;

        foreach (Transform child in adder.transform)
        {
            
            pa = child.gameObject.GetComponent<GunBase>();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.LogError("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.LogError("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    GameManager.Instance.inputManager.OnShotLeft -= pa.DoShot;
                    break;
                case TypePart.ShootableRight:
                    GameManager.Instance.inputManager.OnShotRight -= pa.DoShot;
                    break;
            }
            
            GameObject col=  child.transform.Find("Collisions").gameObject;

            //revsiar detector de capa
            /* if (col != null)
            {
                //List<GameObject> dettors = new List<GameObject>();
                DetectorCapa[] script= col.transform.GetComponentsInChildren<DetectorCapa>();
                foreach (var s in script)
                {
                    if (DetectorsCapa.Contains(s.gameObject))
                    {
                        DetectorsCapa.Remove(s.gameObject);
                    }
                }
                
            } */
            
            
            Destroy(child.gameObject);
        }
        // ToAddParts.Clear();
        // AddedParts=new List<GameObject>();


    }

    private void ActivateItems()
    {
        // foreach (var inventoryPart in AddedParts)
        
        foreach (Transform inventoryPart in adder.transform)
        {
            int actIDP=inventoryPart.GetComponent<EachPartScript>().ReturnIDP();
            Debug.Log("itemm return idp1:"+actIDP);
            InventoryManager.Inventory? inventory = inventoryManager.ReturnInventory(actIDP);

            if(inventory==null) {
                Debug.LogError("how thedefuc are you don't giving an inventory");
                continue;
            }
            SlotInventory[] slots=inventory?.ReturnListSlots();
            //  item.Key
            Debug.Log("itemm inventory not default");
            foreach (var slot in slots)
            {
                // int inventoryIDP= inventoryPart.GetComponent<GunBase>().ReturnIDP();
                Debug.Log("itemm return idp2:"+actIDP);
                if(slot.thisItem!=null){
                    GameObject go= Instantiate(slot.thisItem.takableDataSO.toInstanciate, inventory?.ReturnVisualizer().transform);

                    AddedItems.Add(go);
                    go.GetComponent<EachPartScript>().Activate();


                    ModifierItem modItem= go.GetComponent<AllObjectMB>().ReturnModifierItem();
                    inventoryPart.GetComponent<GunBase>().AddEffectsBullets(modItem.ItemBulletEffects);
                    modItem.modifyIdp=actIDP;
                    foreach (var pair in modItem.ItemGeneralStats)// ?? new Dictionary<Stat.StatTypeGeneral, StatModifier>())
                    {
                        statsManager.instance.AddModifier(pair.Key, pair.Value );
                    }
                    foreach (var pair in modItem.ItemGunStats)// ?? new Dictionary<Stat.StatTypeGun, StatModifier>())
                    {
                        statsManager.instance.AddModifier(actIDP, pair.Key, pair.Value );
                    }

                }

            }
            // AddedParts[actIDP].
            //accedir inventari
            //accedir cada item de inventari
            //cridar mètode activar dels items
            //i afegir a l'arma
        }
    }

    
    private void DesactivateItems()
    {
        /* foreach (var item in AddedItems)
        {
            ModifierItem modItem= item.GetComponent<AllObjectMB>().ReturnModifierItem();

            inventoryPart.GetComponent<GunBase>().RemoveEffectsBullets(modItem.ItemBulletEffects);
            // modItem.modifyIdp=inventoryIDP;
            foreach (var pair in modItem.ItemGeneralStats)// ?? new Dictionary<Stat.StatTypeGeneral, StatModifier>())
            {
                statsManager.instance.RemoveModifier(pair.Key, pair.Value );
            }
            foreach (var pair in modItem.ItemGunStats)// ?? new Dictionary<Stat.StatTypeGun, StatModifier>())
            {
                statsManager.instance.RemoveModifier(inventoryIDP, pair.Key, pair.Value );
            }
        } */

        /* foreach (Transform inventoryPart in adder.transform)
        {
            int actIDP=inventoryPart.GetComponent<EachPartScript>().ReturnIDP();
            Debug.Log("itemm return idp1:"+actIDP);
            InventoryManager.Inventory? inventory = inventoryManager.ReturnInventory(actIDP);

            if(inventory==null) {
                Debug.LogError("how thedefuc are you don't giving an inventory");
                continue;
            }
            SlotInventory[] slots=inventory?.ReturnListSlots();
            //  item.Key
            Debug.Log("itemm inventory not default");
            foreach (var slot in slots)
            {
                int inventoryIDP= inventoryPart.GetComponent<GunBase>().ReturnIDP();
                Debug.Log("itemm return idp2:"+actIDP);
                if(slot.thisItem!=null){
                    // GameObject go= Instantiate(slot.thisItem.takableDataSO.toInstanciate, inventory?.ReturnVisualizer().transform);

                    AddedItems.Add(go);
                    go.GetComponent<EachPartScript>().Activate();


                    ModifierItem modItem= go.GetComponent<AllObjectMB>().ReturnModifierItem();
                    inventoryPart.GetComponent<GunBase>().AddEffectsBullets(modItem.ItemBulletEffects);
                    modItem.modifyIdp=inventoryIDP;
                    foreach (var pair in modItem.ItemGeneralStats)// ?? new Dictionary<Stat.StatTypeGeneral, StatModifier>())
                    {
                        statsManager.instance.AddModifier(pair.Key, pair.Value );
                    }
                    foreach (var pair in modItem.ItemGunStats)// ?? new Dictionary<Stat.StatTypeGun, StatModifier>())
                    {
                        statsManager.instance.AddModifier(inventoryIDP, pair.Key, pair.Value );
                    }

                }

            }
            // AddedParts[actIDP].
            //accedir inventari
            //accedir cada item de inventari
            //cridar mètode activar dels items
            //i afegir a l'arma
        } */

        //reverse activateItems
        
        //accedir inventari
        //compare with the items that we have
        //...


        //maybe this func not necesari, maybe we can just in activate items
        // delete the items we created and spawn the news and fuck off
    
    }
    public void ResetPlacement()
    {
        ToAddParts.Clear();

   
        DeactivatePart();
    }

    public void SavePlacement()
    {
        if (ToAddParts.Count <= 0 && AddedParts==null) return;

        // AddedItems
        Vector3 origin= GameManager.Instance.playerInstance.transform.position;
        for(int i = 1; i < ToAddParts.Count; i++)
        {
            if (!AddedParts.ContainsKey(ToAddParts[i]))
            {
                AddedParts.Add(ToAddParts[i], CreatePart(ToAddParts[i],  origin));
            }
        }
        ActivateParts();
        //should do something to add the items and other habilities
        ActivateItems();

    }


    #endregion


}
