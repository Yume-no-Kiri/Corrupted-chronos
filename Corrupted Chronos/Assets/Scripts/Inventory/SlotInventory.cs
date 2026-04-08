using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SlotInventory : MonoBehaviour
{
    /* 
     public struct infoSlot
    {
        objecte assignat o null

        void agregarItem
        void treure/moure item
    }  */
    private Button button;
    public AllObjectSO thisItem {get; private set; }
    private Image renderItem=null;
   
    private bool hasItem=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        SaveRenderItem();
    }

    private void SaveRenderItem()
    {
        if (renderItem == null)
        {
            // button=transform.GetComponentInChildren<Button>();
            renderItem = transform.GetChild(1).GetComponent<Image>();
            renderItem.sprite = null;
            renderItem.enabled = false;
            thisItem = null;
        }
    }

    void Start()
    {
        // button.onClick.AddListener(ButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPressed()
    {
        Debug.Log("buttonpressed");
    }
    public void AddItem(AllObjectSO newItem)
    {
        SaveRenderItem();

        thisItem=newItem;
        hasItem=true;
        ShowSprite();
    }

    public void DeleteItem()
    {
        HideSprite();
        thisItem=null;
        hasItem=false;
    }

    public bool HasItem(){
        return hasItem;
    }

    public bool CanBePlaced()
    {
        return thisItem.canBePlaced;
        // return false;
    }
    public void ShowSprite()
    {
        if(!HasItem())
        {
            return;
        }else{
            
            renderItem.sprite =thisItem.takableDataSO.sprite;
            renderItem.enabled=true;
        }
    }

    public void HideSprite()
    {
        // SaveRenderItem();
        if(!HasItem())
        {
            return;
        }else{
            renderItem.sprite =null;
            renderItem.enabled=false;
        }
    }

    public void ActivateSelectedEffect()
    {
        if (HasItem())
        {
            Color color;
            color = renderItem.color;
            color.a = 0.5f;

            renderItem.color=color;
        }
        
    }
    public void DeactivateSelectedEffect()
    {
        if (HasItem())
        {
            Color color;
            color = renderItem.color;
            color.a = 1f;

            renderItem.color=color;
        }
        
    }


}

