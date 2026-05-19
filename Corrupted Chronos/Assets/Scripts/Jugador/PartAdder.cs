using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;




public class PartAdder : MonoBehaviour
{
    InventoryManager inventoryManager;

    //to activate, enable parts with the movement, shoting, will have things for the inventary too
    private List<PartPosition> ToAddParts ;
    private List<PartPosition> NewerParts;


    //this structure es kinda shitty
    private Dictionary<PartPosition, GameObject> AddedParts;

    private GameObject _nauGO; 

    public GameObject adder{get; private set;}

    // private List<GameObject> AddedItems;
    private Dictionary<int, List<GameObject>> AddedItemsByGun;

    //feels like this should be in other part of the project but I don't found it, so I will create it here, I need it to remove items of guns, I will rework this in the future
    // private Dictionary<int, GameObject> itemIDP;



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
        DefaultValues();
    }

    private void DefaultValues()
    {
        ToAddParts = new List<PartPosition>();
        AddedParts = new Dictionary<PartPosition, GameObject>();
        NewerParts = new List<PartPosition>();

        AddedItemsByGun = new Dictionary<int, List<GameObject>>();
        // GameObjectIDP= new Dictionary<int, GameObject>();
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


    #region buttons
    public void ResetPlacement()
    {
        SavePlacement();

     
        StartCoroutine(DeleteInventories());
    }

   

    public void SavePlacement()
    {
        if (ToAddParts.Count <= 0 && AddedParts==null) return;
        NewerParts= new List<PartPosition>();

        Vector3 origin= GameManager.Instance.playerInstance.transform.position;
        for(int i = 1; i < ToAddParts.Count; i++)
        {
            if (!AddedParts.ContainsKey(ToAddParts[i]))
            {
                AddedParts.Add(ToAddParts[i], CreatePart(ToAddParts[i],  origin));
                NewerParts.Add(ToAddParts[i]);
                // Debug.Log("creating part");
            }
        }
        ActivateParts();


        ActivateItems();

    }


    #endregion


    #region SavePlacement
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
    
    //aquí està el problema
    public void ActivateParts()
    {
        //acabar
        GunBase pa;

        foreach (var item in NewerParts)
        {
            pa= AddedParts[item].GetComponent<EachPartScript>().ActivateGun();
            pa.SubscribeEvent();

        }



        // foreach (Transform inventoryPart in AddedParts.Values.Tra)
        /* foreach (Transform part in adder.transform)
        {
            //no estic segur de que part actions segui lo millor per invocar aquests mètodes,
            //revisar explicació escrita en EachPartScript per futur REFACTORITZACIÓ
           

            // Debug.Log("partchild: "+ part.name);

            pa= part.GetComponent<EachPartScript>().ActivateGun();
            pa.SubscribeEvent();
            
        } */
    }

    private void DeactivateItems()
    {
        foreach (var gun in AddedItemsByGun)
        {
            // item.GetComponent<EachPartScript>().Activate();
            int actIDP=gun.Key;

            foreach (var item in gun.Value)
            {

             /*    AllObjectMB itemMB = item.GetComponent<AllObjectMB>();
                ModifierItem modItem = itemMB.ReturnModifierItem(); */

                // InventoryManager.Inventory? inventory = inventoryManager.ReturnInventory(actIDP);

                RemoveItemModifier(item, actIDP,true);

                // pa = child.gameObject.GetComponent<GunBase>();
                // TakableDataSO takableDataSO= itemMB.ReturnTakableDataSO();            


            }
            gun.Value.Clear();
            // AddedItemsByGun.Remove(actIDP);
        }
        AddedItemsByGun.Clear();

    }

    private void RemoveItemModifier(GameObject item, int actIDP, bool ReturnItem)
    {

        AllObjectMB itemMB = item.GetComponent<AllObjectMB>();
        ModifierItem modItem = itemMB.ReturnModifierItem();

        GameObject gunGO = returnGameObjectForIDP(actIDP);
        if (gunGO == null) Debug.LogError("gameobject item didn't found");

        //falta això
        gunGO.GetComponent<GunBase>().RemoveEffectsBullets(modItem.ItemBulletEffects);
        modItem.modifyIdp = actIDP;
        foreach (var pair in modItem.ItemGeneralStats)// ?? new Dictionary<Stat.StatTypeGeneral, StatModifier>())
        {
            statsManager.instance.RemoveModifier(pair.Key, pair.Value);
        }
        foreach (var pair in modItem.ItemGunStats)// ?? new Dictionary<Stat.StatTypeGun, StatModifier>())
        {
            statsManager.instance.RemoveModifier(actIDP, pair.Key, pair.Value);
        }

        if(ReturnItem) inventoryManager.AddItemSpaceShip(itemMB.ReturnAllObjectSO());
        Destroy(item);

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
            //  item.Key
            // Debug.Log("itemm inventory not default");

            // if(AddedItemsByGun.ContainsKey(actIDP)){
            if(AddedItemsByGun.TryGetValue(actIDP, out List<GameObject> oldItems))
            {
                foreach (var item in oldItems)//AddedItemsByGun[actIDP])
                {
                    RemoveItemModifier(item,actIDP,false);
                }
                // AddedItemsByGun[actIDP].Clear();
                oldItems.Clear();
            }

            SlotInventory[] slots=inventory?.ReturnListSlots();

            List<GameObject> addedItems= new List<GameObject>();
            foreach (var slot in slots)
            {
                //eliminar el que hi havia abans 
                if (slot.thisItem != null)
                {
                    AddItemFromSlotModifier(inventoryPart, actIDP, inventory, slot, ref addedItems);
                }
            }
            if(!AddedItemsByGun.ContainsKey(actIDP)){ AddedItemsByGun.Add(actIDP,addedItems);}
            else { AddedItemsByGun[actIDP]=addedItems; }
            // AddedParts[actIDP].
            //accedir inventari
            //accedir cada item de inventari
            //cridar mètode activar dels items
            //i afegir a l'arma
        }
    }

    private void AddItemFromSlotModifier(Transform inventoryPart, int actIDP, InventoryManager.Inventory? inventory, SlotInventory slot, ref List<GameObject> addedItems)
    {
       
        GameObject go = Instantiate(slot.thisItem.takableDataSO.toInstanciate, inventory?.ReturnVisualizer().transform);

        addedItems.Add(go);
        go.GetComponent<EachPartScript>().Activate();


        ModifierItem modItem = go.GetComponent<AllObjectMB>().ReturnModifierItem();
        inventoryPart.GetComponent<GunBase>().AddEffectsBullets(modItem.ItemBulletEffects);
        modItem.modifyIdp = actIDP;
        foreach (var pair in modItem.ItemGeneralStats)// ?? new Dictionary<Stat.StatTypeGeneral, StatModifier>())
        {
            statsManager.instance.AddModifier(pair.Key, pair.Value);
        }
        foreach (var pair in modItem.ItemGunStats)// ?? new Dictionary<Stat.StatTypeGun, StatModifier>())
        {
            statsManager.instance.AddModifier(actIDP, pair.Key, pair.Value);
        }

        
    }



    /* private void RemoveItemFromSlotModifier(Transform inventoryPart, int actIDP, InventoryManager.Inventory? inventory, SlotInventory slot)
    {
        if (slot.thisItem != null)
        {
            GameObject go = Instantiate(slot.thisItem.takableDataSO.toInstanciate, inventory?.ReturnVisualizer().transform);

            AddedItemsByGun.Add(go);
            go.GetComponent<EachPartScript>().Activate();


            ModifierItem modItem = go.GetComponent<AllObjectMB>().ReturnModifierItem();
            inventoryPart.GetComponent<GunBase>().AddEffectsBullets(modItem.ItemBulletEffects);
            modItem.modifyIdp = actIDP;
            foreach (var pair in modItem.ItemGeneralStats)// ?? new Dictionary<Stat.StatTypeGeneral, StatModifier>())
            {
                statsManager.instance.AddModifier(pair.Key, pair.Value);
            }
            foreach (var pair in modItem.ItemGunStats)// ?? new Dictionary<Stat.StatTypeGun, StatModifier>())
            {
                statsManager.instance.AddModifier(actIDP, pair.Key, pair.Value);
            }

        }
    } */

    #endregion

    #region reset placement
    public void DeactivatePart()
    {
        //reworkejar això amb addedParts
        GunBase pa;

        foreach (Transform child in adder.transform)
        {
            
            pa = child.gameObject.GetComponent<GunBase>();
            pa.DesubcribeEvent();
            // Destroy(child.gameObject);
            
        }
    }

     public void DeleteParts()
    {
        //reworkejar això amb addedParts
        GunBase pa;

        foreach (Transform child in adder.transform)
        {
            pa = child.gameObject.GetComponent<GunBase>();
            // pa.ReturnTakableDataSO();            
            // Destroy(AddedItemsByGun[]);

            inventoryManager.AddItemSpaceShip(pa.ReturnAllObjectSO());
            Destroy(child.gameObject);
        }

        DefaultValues();
    }    

    private IEnumerator DeleteInventories()
    {
        
        /* List<int> invetaris= inventoryManager.ReturnListIDPwithInventory();

        foreach (var idp in invetaris)
        {
            if (!AddedItemsByGun.ContainsKey(idp))
            {
                inventoryManager.DeleteIventory(idp);
            }
        } */
        yield return null;
        DeactivateItems();
        yield return null;

        DeactivatePart();
        yield return null;

        DeleteParts();
        yield return null;

        inventoryManager.DeleteAllInventories();
    }

     private GameObject returnGameObjectForIDP(int actIDP)
    {
        foreach (var item in AddedParts)
        {
            if(item.Key.IDP==actIDP)
            {
                return item.Value;
            }
        }
        return null;
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
    
    #endregion

}
