using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

    PlacementSystem placementSystem;


    //COntendria llistes d'inventaris
    //connectar amb Player


    /* public struct inventory{
        gameobject qui és owner
        maxslots
        allslots contenedor

        void inicar el inventari
        void buidar el inventari

    }


    necesito informació de cada inventari, 
    -A qui li perteneix, diccionari
    -Slots disponibles, 
    -?ara mateix no, pero hauré de fer que hi hagin slots especials i restrintius
    */
    // int maxslots=3;
    //objetos que tenemos

    // List<ItemData>
    // ItemData[] listItems;
    //slots fisicos

    SlotInventory selectedSlot;
    SlotInventory[] listSlots;
    [SerializeField] private GameObject inventoryCanvas;
    private InputManager inputManager;

    [SerializeField] int nslots;
    [SerializeField] private SlotInventory prefabSlot;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placementSystem=GameManager.Instance.playerInstance.GetComponent<Player>().returnPlacementSystem();

        inputManager= GameManager.Instance.inputManager;
        inputManager.PrimaryClick+=moveItem;

        /* foreach (var item in inventoryCanvas.transform.GetChild(0).GetChild(1).GetComponentsInChildren<SlotInventory>())
        {
            Destroy(item);
        } */
        listSlots= CreateSlotsInventory();

        // listSlots= inventoryCanvas.transform.GetChild(0).GetChild(1).GetComponentsInChildren<SlotInventory>();

        // if(listSlots.Count()!=3) Debug.LogError("listSlots count és:"+ listSlots.Count());
    
        placementSystem.PlacedPart+=PlacedItem;
        placementSystem.CancelledPart+=CancelledItem;
    }

    // Update is called once per frame
    void Update()
    {
        // print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
        
        if (inputManager.playerInputActions.Inventory.enabled)
        {
            
            // Ray ray = cameraGarage.ScreenPointToRay(mousePos);
            // RaycastHit hit;
            
                
            
            // Vector3Int gridPosition = inputManager.MousePositionGarage;
        }

    }

  /*   void addNewItemToInventory(ItemData item)
    {
        Debug.Log("added new object inventory");
        if(AddItem(item));
        // listItems.Add(item);
    } */
    private SlotInventory[] CreateSlotsInventory()
    {
        SlotInventory[] listSlot= new SlotInventory[nslots];
        GameObject dad= inventoryCanvas.transform.GetChild(0).GetChild(1).gameObject;
        for (int i = 0; i < nslots; i++)
        {
            SlotInventory newSlot=Instantiate(prefabSlot, dad.transform);
            newSlot.gameObject.transform.SetParent(dad.transform,false);
            listSlot[i]=newSlot;
        }
        return listSlot;

    }


    public bool AddItem(AllObjectSO newItem)
    {   
        Debug.Log("newItem sprite"+newItem.takableData.sprite.name);

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

    #region called by player
    public void OnActivate()
    {
        selectedSlot=null;
        /* for (int i = 0; i < listSlots.Length; i++)
        {
            listSlots[i].ShowSprite();
        } */

    }

    public void OnDeactivate()
    {
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
                placementSystem.StartPlacementGeneral(0, ConfigurationNau.Parts);
            }
        }
        else
        {
            if(slotInventory==null)
            {
                DeselectItemSlot();
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
                    AllObjectSO aux=slotInventory.thisItem;
                    selectedSlot.DeactivateSelectedEffect();
                    slotInventory.AddItem(selectedSlot.thisItem);
                    selectedSlot.AddItem(aux);

                    selectedSlot=null;
                    //intercanvia
                }
            }
        }


    }

    private void DeselectItemSlot()
    {
        selectedSlot.DeactivateSelectedEffect();
        selectedSlot = null;
    }

    private void DeleteItemSlot()
    {
        selectedSlot.DeactivateSelectedEffect();
        selectedSlot.DeleteItem();
        selectedSlot = null;
    }

    #endregion


}
