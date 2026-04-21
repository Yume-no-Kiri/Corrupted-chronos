using Unity.VisualScripting;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    private SphereCollider getterItems;

    private InventoryManager inventoryManager;
    // private Player player;

    void Awake()
    {
        inventoryManager=GetComponentInParent<InventoryManager>();
        getterItems=GetComponent<SphereCollider>();
        // player=GetComponentInParent<Player>();
    }
    void Start()
    {
        //el rang en que s'agafen objectes podria ser una variable modificable
        getterItems.radius=2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("objecte enter"+other.gameObject.tag);
        if (other.gameObject.CompareTag("Collectable"))
        {
            AllObjectMB itemBase=other.gameObject.GetComponent<AllObjectMB>();
            //guardar scripteable object a una llista de player, en inventoryPlayer probablement
            //eliminar objecte real
            Debug.Log("objecte detectat");
            if (inventoryManager.AddItemSpaceShip(itemBase.ReturnAllObjectSO()))
            {
                Debug.Log("itemBase.GetScipteableObject() sprite"+itemBase.ReturnTakableDataSO().sprite.name);
                Destroy(other.gameObject);            
            }
        }
    }
}
