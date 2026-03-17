using UnityEngine;

public class CollectItems : MonoBehaviour
{
    private SphereCollider getterItems;


    void Awake()
    {
        getterItems=GetComponent<SphereCollider>();
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
            //guardar scripteable object a una llista de player, en inventoryPlayer probablement
            //eliminar objecte real
            Debug.Log("objecte detectat");
        }
    }
}
