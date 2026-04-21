using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllCatalog", menuName = "Scriptable Objects/AllCatalog")]
public class AllCatalog : ScriptableObject
{
    [Header("This wil be used when PlacementDatabase,TakableDataBase and AllObjectsDataBase will be initialized in code and not in inspector ")]
    //prefabs can't be assigned in code with a link, so this will do it, when initialezed the other database, they will reach gameManager, which will contain this script, and this will return the prefab here
    public List<GameObject> TakableCatalog=new List<GameObject>();

    // public List<GameObject> PlacedCatalog=new List<GameObject>();


    public GameObject GetTakableByName(string nom)
    {
        return TakableCatalog.Find(p => p.name == nom);
    }
    /* public GameObject GetPlacedByName(string nom)
    {
        return PlacedCatalog.Find(p => p.name == nom);
    } */
}
