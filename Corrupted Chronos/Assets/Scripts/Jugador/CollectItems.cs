using Unity.VisualScripting;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    private SphereCollider getterItems;
    // private Player player;

    void Awake()
    {
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
            ItemBase itemBase=other.gameObject.GetComponent<ItemBase>();
            //guardar scripteable object a una llista de player, en inventoryPlayer probablement
            //eliminar objecte real
            Debug.Log("objecte detectat");
            GameManager.Instance.inputManager.CallAddNewItem(itemBase.GetScipteableObject());
        }
    }
}
