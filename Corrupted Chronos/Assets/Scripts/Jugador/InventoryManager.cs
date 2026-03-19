using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //COntendria llistes d'inventaris
    //connectar amb Player


    /* public struct inventory{
        gameobject qui és owner
        maxslots
        allslots contenedor

        void inicar el inventari
        void buidar el inventari

    }*/

    /* public struct infoSlot
    {
        objecte assignat o null

        void agregarItem
        void treure/moure item
    } */

    /* necesito informació de cada inventari, 
    -A qui li perteneix, diccionari
    -Slots disponibles, 
    -?ara mateix no, pero hauré de fer que hi hagin slots especials i restrintius
    */
    List<ItemData> listItems= new List<ItemData>();



    void OnEnable()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.inputManager.AddNewItem+=addNewItemToInventory; //OnPartSelected += ReaccionarAPeca;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void addNewItemToInventory(ItemData item)
    {
        listItems.Add(item);
    }



    
}
