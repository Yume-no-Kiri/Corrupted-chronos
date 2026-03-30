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
    public ItemData thisItem {get; private set; }
    private Image renderItem;
   
    private bool hasItem=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // button=transform.GetComponentInChildren<Button>();
        renderItem=transform.GetChild(1).GetComponent<Image>();
        renderItem.sprite=null;
        renderItem.enabled=false;
        thisItem=null;
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
    public void AddItem(ItemData newItem)
    {
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
        // return thisItem.canBePlaced;
        return false;
    }
    public void ShowSprite()
    {
        if(!HasItem())
        {
            return;
        }else{
            renderItem.sprite =thisItem.sprite;
            renderItem.enabled=true;
        }
    }

    public void HideSprite()
    {
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

