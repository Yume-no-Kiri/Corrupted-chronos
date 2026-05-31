using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Unity.Mathematics;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

    PlacementSystem placementSystem;
    private InputManager inputManager;


    //I think, not fully sure about it:
    //to save a part into the inventari de idp gets deleted because it turns into scripteable object, so an inventaary has to be empty to quit a part
    Inventory inventorySpaceship=new Inventory();
    
    Dictionary<int, Inventory> IDPInventory= new Dictionary<int, Inventory>(); //idp, inventory
    
    public struct Inventory{
        // gameobject qui és owner
        int maxSlots;
        GameObject visualizer;
        SlotInventory[] listSlots;
        //    -?ara mateix no, pero hauré de fer que hi hagin slots especials i restrintius

        /*  void inicar el inventari
        void buidar el inventari */

        public static Inventory CreateInventory(GameObject canvas, int nslots, SlotInventory[] slots)
        {
            Inventory inventory= new Inventory();
            inventory.maxSlots=nslots;
            inventory.visualizer=canvas;
            inventory.listSlots=slots;

            return inventory;
        }

        public bool AddItem(AllObject newItem)
        {   
            Debug.Log("newItem sprite"+newItem.takableDataSO.sprite.name);

            for (int i = 0; i < listSlots.Length; i++)
            {
                if (!listSlots[i].HasItem())
                {
                    listSlots[i].AddItem(newItem);
                    return true; 
                }
            }
            return false; 
        }
        
        public void Delete()
        {
            Destroy(visualizer);
        }

        public void ShowOff()
        {
            visualizer.SetActive(false);
        }

        public void ShowOn()
        {
            visualizer.SetActive(true);
        }

        internal bool isDefault()
        {
            if(visualizer==null) return true;
            return false;
        }

        internal SlotInventory[] ReturnListSlots()
        {
            return listSlots;
        }

        internal GameObject ReturnVisualizer()
        {
            return visualizer;
        }
    }
    
    SlotInventory selectedSlot;
    
    // SlotInventory[] listSlots;
    [SerializeField] private GameObject inventoryCanvas;

    [SerializeField] private GameObject inventoryGeneral;
    [SerializeField] private GameObject inventoryPart;

    [SerializeField] int MaxSlotsShip;
    [SerializeField] private SlotInventory prefabSlot;

    private int InventoryOpened=-1;

    private Coroutine dontCallTwice=null; //bugfix

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placementSystem=GameManager.Instance.playerInstance.GetComponent<Player>().returnPlacementSystem();

        inputManager= GameManager.Instance.inputManager;
        inputManager.PrimaryClick+=moveItem;
        placementSystem.IDPinCursor+=OpenInventory;
       

        CreateSpaceShipInventory();

        // listSlots= CreateSlotsInventory();

        // listSlots= inventoryCanvas.transform.GetChild(0).GetChild(1).GetComponentsInChildren<SlotInventory>();

        // if(listSlots.Count()!=3) Debug.LogError("listSlots count és:"+ listSlots.Count());
    
        placementSystem.PlacedPart+=PlacedItem;
        placementSystem.CancelledPart+=CancelledItem;
    }

    void OnDestroy()
    {
        inputManager.PrimaryClick-=moveItem;
        placementSystem.IDPinCursor-=OpenInventory;


        placementSystem.PlacedPart-=PlacedItem;
        placementSystem.CancelledPart-=CancelledItem;
    }
    // Update is called once per frame
    void Update()
    {
        // print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
        
        /* if (inputManager.playerInputActions.Inventory.enabled)
        {
            */ 
            // Ray ray = cameraGarage.ScreenPointToRay(mousePos);
            // RaycastHit hit;
            
                
            
            // Vector3Int gridPosition = inputManager.MousePositionGarage;
/*         }
 */
    }

    void OnDisable()
    {
        DeselectItemSlot();
        selectedSlot=null;
    }



    public void OpenInventory(int actIDP)
    {
        if(actIDP==0 ||!IDPInventory.ContainsKey(actIDP) ) {
            // Debug.LogError("idp of ship");
            return;
        }
        if (InventoryOpened != -1)
        {
            IDPInventory[actIDP].ShowOff();
            InventoryOpened=-1;
        }
        if (IDPInventory.ContainsKey(actIDP))
        {
            IDPInventory[actIDP].ShowOn();
            InventoryOpened=actIDP;
        }
    }

    public void CreateInventory(int newIDP)
    {
        Debug.Log("inventory create for itemm:"+newIDP);
        GameObject canvas= Instantiate(inventoryPart,inventoryCanvas.transform);
        IDPInventory.Add(newIDP, Inventory.CreateInventory(canvas, 3, CreateSlotsInventory(canvas, 3)));
        IDPInventory[newIDP].ShowOff();
    }

    private void CreateSpaceShipInventory()
    {
        
        
        SlotInventory[] listSlot= new SlotInventory[MaxSlotsShip];
        //this is inventoryGeneral
        // inventoryGeneral=inventoryCanvas.transform.GetChild(1).gameObject;
        GameObject dad= inventoryGeneral.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < MaxSlotsShip; i++)
        {
            SlotInventory newSlot=Instantiate(prefabSlot, dad.transform);
            newSlot.gameObject.transform.SetParent(dad.transform,false);
            listSlot[i]=newSlot;
        }
        inventorySpaceship=Inventory.CreateInventory(inventoryGeneral,MaxSlotsShip,listSlot);
        // return listSlot;
    }


    private SlotInventory[] CreateSlotsInventory(GameObject canvas, int nslots)
    {
        
        // inventoryCanvas

        SlotInventory[] listSlot= new SlotInventory[nslots];
        //this is inventoryGeneral
        GameObject dad= canvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < nslots; i++)
        {
            SlotInventory newSlot=Instantiate(prefabSlot, dad.transform);
            newSlot.gameObject.transform.SetParent(dad.transform,false);
            listSlot[i]=newSlot;
        }
        return listSlot;

    }


    public bool AddItemSpaceShip(AllObject newItem)
    {   
        return inventorySpaceship.AddItem(newItem);

        /* Debug.Log("newItem sprite"+newItem.takableDataSO.sprite.name);

        for (int i = 0; i < listSlots.Length; i++)
        {
            if (!listSlots[i].HasItem())
            {
                listSlots[i].AddItem(newItem);
                return true; 
            }
        }
        return false;  */
    }

    public Inventory? ReturnInventory(int actIDP)
    {
        if(IDPInventory.ContainsKey(actIDP))
        {
            return IDPInventory[actIDP];
        }
        return null;
    }

    #region called by player
    public void OnActivate()
    {
        selectedSlot=null;
        // IDPInventory.Keys.Count()
        if (IDPInventory.Keys.Count > 1)
        {
            for (int i = 1; i < IDPInventory.Keys.Count ; i++)
            {
                if(IDPInventory.ContainsKey(i)) IDPInventory[i].ShowOff();

            }


        }

        /* for (int i = 0; i < listSlots.Length; i++)
        {
            listSlots[i].ShowSprite();
        } */

    }

    public void OnDeactivate()
    {
        DeselectItemSlot();
        selectedSlot=null;
    }
    #endregion

    #region selectedSlotItem
    
    public void PlacedItem()
    {
        DeleteItemSlot();
    }
    public void CancelledItem()
    {
        DeselectItemSlot();
    }

    //això funcionaria inclús sent de diferents inventaris
    private void moveItem(SlotInventory slotInventory)
    {
        // if(!inputManager.IsPointerOverUI()) return;
        if(!inputManager.IsPointerOverUI()) return;
        if (!selectedSlot)
        { 
            if(slotInventory==null) {
                Debug.Log("click");
                return;
            }
             if(!slotInventory.HasItem()) return;
            // if(!slotInventory.HasItem())
            Debug.Log("eo slotInventory:"+ slotInventory.name);
            selectedSlot=slotInventory;
            slotInventory.ActivateSelectedEffect();
            if (slotInventory.CanBePlaced())
            {
                //this should come from the information of the part
                // placementSystem.StartPlacementGeneral(0, ConfigurationNau.Parts);
                placementSystem.StartPlacementGeneral(slotInventory.thisItem.placementDataItemSO);

            }
        }
        else
        {
            if(slotInventory==null)
            {
                // DeselectItemSlot();
                return;
            }
            else if(slotInventory!=selectedSlot){
                if(!slotInventory.HasItem()){
                    //mou a slot buit
                    selectedSlot.DeactivateSelectedEffect();
                    slotInventory.AddItem(selectedSlot.thisItem);
                    selectedSlot.DeleteItem();
                    selectedSlot=null;
                }
                else
                {
                    //intercanvia
                    AllObject aux=slotInventory.thisItem;
                    selectedSlot.DeactivateSelectedEffect();
                    slotInventory.AddItem(selectedSlot.thisItem);
                    selectedSlot.AddItem(aux);

                    selectedSlot=null;
                    //intercanvia
                }
                placementSystem.ButtonStopStructure();
            }
        }


    }

    private void DeselectItemSlot()
    {
        if(!selectedSlot) return;
        if (dontCallTwice==null)
        {
            Debug.Log("entering deselect");
            selectedSlot.DeactivateSelectedEffect();
            selectedSlot = null;
            dontCallTwice= StartCoroutine(DontCallTwice());
        }
        
    
    }

    private IEnumerator DontCallTwice()
    {
        yield return new WaitForSeconds(0.2f);
        dontCallTwice=null;
    }

    private void DeleteItemSlot()
    {
        selectedSlot.DeactivateSelectedEffect();
        selectedSlot.DeleteItem();
        selectedSlot = null;
    }

    public List<int> ReturnListIDPwithInventory()
    {
        List<int> listIDP= new List<int>();
        foreach (var item in IDPInventory)
        {
            listIDP.Add(item.Key);
        }
        return listIDP;

    }

    public void DeleteIventory(int idp)
    {
        IDPInventory[idp].Delete();

    }

    internal void DeleteAllInventories()
    {
        foreach (var idp in IDPInventory)
        {
            DeleteIventory(idp.Key);
        }

    }

    #endregion


}
